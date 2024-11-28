using DropSpace.Areas.Home.Models;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.PersonData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;
using DropSpace.Models;
using DropSpace.Repository.Contracts;
using DropSpace.Services.Filehandling.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly IPersonData _persondata;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMasterData _masterdata;
        private readonly IGenericRepository<CrimeInfo> _repoCrimeInfo;
        private readonly IFileHandlingService _fileHandlingService;
        //private const long MAX_FILE_SIZE = 100 * 1024 * 1024; // 100 MB
        //private const int MAX_TOTAL_FILES = 1; 

        public HomeController(IPersonData persondata, IMasterData masterData, IGenericRepository<CrimeInfo> repoCrimeInfo, IWebHostEnvironment webHostEnvironment, IFileHandlingService fileHandlingService)
        {
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
            _masterdata = masterData;
            _repoCrimeInfo = repoCrimeInfo;
            _fileHandlingService = fileHandlingService;
        }


        public async Task<IActionResult> Index(string? userName)
        {
            ViewBag.userName = userName;
            var crimeType = await _repoCrimeInfo.FindAll();
            var uploadSettings = await _fileHandlingService.GetFileUploadSettingsAsync();
            var fileLimit = uploadSettings.fileLimits.FirstOrDefault();
            ViewBag.type = crimeType.Select(x => new CrimeTypeViewModel
            {
                Id = IdMasking.Encode(x.Id.ToString()),
                crimeTypeNameBn=x.crimeType
            });
            ViewBag.fileLimit = fileLimit?.dayFileNo;
            ViewBag.fileSizeLimit = fileLimit?.dayFileSize;
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(2147483648)] // 2GB limit
        public async Task<IActionResult> Index([FromForm] PersonsDataViewModel personsData, [FromForm] IFormFileCollection files)
        {
            var uploadSettings = await _fileHandlingService.GetFileUploadSettingsAsync();
            var fileLimits = uploadSettings.fileLimits.FirstOrDefault();
            var crimeTypes = await _repoCrimeInfo.FindAll();
            ViewBag.type = crimeTypes.Select(x => new CrimeTypeViewModel
            {
                Id = IdMasking.Encode(x.Id.ToString()),
                crimeTypeNameBn = x.crimeType
            });
            ViewBag.fileLimit = fileLimits?.dayFileNo;
            ViewBag.fileSizeLimit = fileLimits?.dayFileSize;
            if (string.IsNullOrWhiteSpace(personsData?.typeId) && (files == null || files.Count == 0))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "invalid_input";
                return View();
            }

            // Find the relevant file limit for the uploaded file type
            var fileType = uploadSettings.fileTypes.FirstOrDefault();
            if (fileType == null)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "File type not found.";
                return View();
            }

            var fileLimit = uploadSettings.fileLimits.FirstOrDefault(fl => fl.fileTypeId == fileType.fileTypeId);
            if (fileLimit == null)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "File type limits not found.";
                return View();
            }

            // Validate file count
            if (files.Count > fileLimit.totalFileNo)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = $"Too many files uploaded. Maximum allowed is {fileLimit.totalFileNo}.";
                return View();
            }

            // Validate file sizes
            foreach (var file in files)
            {
                if (file.Length > fileLimit.dayFileSize * 1024 * 1024) // Convert megabytes to bytes
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = $"File {file.FileName} exceeds the maximum size of {fileLimit.totalFileSize} MB.";
                    return View();
                }
            }

            
            int crimeType = Convert.ToInt32(IdMasking.Decode(personsData?.typeId));
            int? unionId = personsData?.unionId != null ? (int?)Convert.ToInt32(IdMasking.Decode(personsData.unionId)) : null;
            int? villageId = personsData?.villageId != null ? (int?)Convert.ToInt32(IdMasking.Decode(personsData.villageId)) : null;

            var personalData = new PersonsData
            {
                latitude = personsData.latitude,
                longitude = personsData.longitude,
                mobile = personsData.mobile,
                unionId = unionId,
                villageId = villageId,
            };
            int personsDataId = await _persondata.AddPersonsDataAsync(personalData);

            if (files != null && files.Count > 0)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ufile");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                List<string> warningFiles = new List<string>();
                List<UploadedFiles> uploadedFilesList = new List<UploadedFiles>();

                foreach (var file in files)
                {
                    // Save all file types
                    string filePath = Path.Combine(uploadFolder, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedFilesList.Add(new UploadedFiles
                    {
                        personsDataId = personsDataId,
                        crimeTypeId = crimeType,
                        attachmentUrl = Path.Combine("ufile", file.FileName)
                    });
                }

                if (uploadedFilesList.Any())
                {

                    await _persondata.AddUploadedFilesAsync(uploadedFilesList);
                }

                ViewBag.MessageType = "success";
                ViewBag.Message = "data_uploaded_successfully";
            }
            else
            {
                ViewBag.MessageType = "success";
                ViewBag.Message = "persons_data_saved_no_files";
            }

            if (personsData.mobile != null && personsData.mobile != "")
            {
                ViewBag.userName = IdMasking.Encode(personsData.mobile);
            }
            return View();
        }

        



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetAllCountries()
        {
            var data= await _masterdata.GetAllCountries();
            return Json(data.Select(x => new CountryViewModel
            {
                countryNameBn = x.countryNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetDivisionByCountryId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetDivisionsByCountryId(Convert.ToInt32(IdMasking.Decode(model.cId)));
            return Json(data.Select(x => new DivisionViewModel
            {
                divisionNameBn = x.divisionNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetDistrictByDivisionId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetDistrictsByDivisonId(Convert.ToInt32(IdMasking.Decode(model.dId)));
            return Json(data.Select(x => new DistrictViewModel
            {
                districtNameBn = x.districtNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetThanaByDistrictId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetThanasByDistrictId(Convert.ToInt32(IdMasking.Decode(model.dId)));
            return Json(data.Select(x => new PoliceStationViewModel
            {
                policeThanaNameBn = x.thanaNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetActiveThanaByDistrictId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetActiveThanasByDistrictId(Convert.ToInt32(IdMasking.Decode(model.dId)));
            return Json(data.Select(x => new PoliceStationViewModel
            {
                policeThanaNameBn = x.thanaNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetUnionWardsByThanaId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetUnionWardsByThanaId(Convert.ToInt32(IdMasking.Decode(model.tId)));
            return Json(data.Select(x => new UnionWordViewModel
            {
                unionWordNameBn = x.unionNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetActiveUnionWardsByThanaId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetActiveUnionWardsByThanaId(Convert.ToInt32(IdMasking.Decode(model.tId)));
            return Json(data.Select(x => new UnionWordViewModel
            {
                unionWordNameBn = x.unionNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetAllVillageByUnionId([FromBody] MasterDataViewModel model)
        {
            var data = await _masterdata.GetAllVillageByUnionId(Convert.ToInt32(IdMasking.Decode(model.uId)));
            return Json(data.Select(x => new VillageViewModel
            {
                villageNameBn = x.villageNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetAllActiveVillageByUnionId([FromBody] MasterDataViewModel model)
        {
            var data= await _masterdata.GetAllActiveVillageByUnionId(Convert.ToInt32(IdMasking.Decode(model.uId)));
            return Json(data.Select(x => new VillageViewModel
            {
                villageNameBn = x.villageNameBn,
                Id = IdMasking.Encode(x.Id.ToString())
            }));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //[RequestSizeLimit(2147483648)] // 2GB limit
        //public async Task<IActionResult> Index([FromForm] PersonsDataViewModel personsData, [FromForm] IFormFileCollection files)
        //{

        //    var uploadSettings = await _fileHandlingService.GetFileUploadSettingsAsync();

        //    if (string.IsNullOrWhiteSpace(personsData?.typeId) && (files == null || files.Count == 0))
        //    {
        //        ViewBag.MessageType = "error";
        //        ViewBag.Message = "invalid_input";
        //        return View();
        //    }

        //    // Find the relevant file limit for the uploaded file type
        //    var fileType = uploadSettings.fileTypes.FirstOrDefault(ft => ft.fileTypeName == personsData.typeId);
        //    if (fileType == null)
        //    {
        //        ViewBag.MessageType = "error";
        //        ViewBag.Message = "File type not found.";
        //        return View();
        //    }

        //    var fileLimit = uploadSettings.fileLimits.FirstOrDefault(fl => fl.fileTypeId == fileType.fileTypeId);
        //    if (fileLimit == null)
        //    {
        //        ViewBag.MessageType = "error";
        //        ViewBag.Message = "File type limits not found.";
        //        return View();
        //    }

        //    // Validate file count
        //    if (files.Count > fileLimit.totalFileNo)
        //    {
        //        ViewBag.MessageType = "error";
        //        ViewBag.Message = $"Too many files uploaded. Maximum allowed is {fileLimit.totalFileNo}.";
        //        return View();
        //    }

        //    // Validate file sizes
        //    foreach (var file in files)
        //    {
        //        if (file.Length > fileLimit.totalFileSize * 1024 * 1024) // Convert megabytes to bytes
        //        {
        //            ViewBag.MessageType = "error";
        //            ViewBag.Message = $"File {file.FileName} exceeds the maximum size of {fileLimit.totalFileSize} MB.";
        //            return View();
        //        }
        //    }

        //    // Validate file count
        //    if (files.Count > MAX_TOTAL_FILES)
        //    {
        //        ViewBag.MessageType = "error";
        //        ViewBag.Message = "Too many files uploaded. Maximum allowed is " + MAX_TOTAL_FILES + ".";
        //        return View();
        //    }

        //    // Validate file sizes
        //    foreach (var file in files)
        //    {
        //        if (file.Length > MAX_FILE_SIZE)
        //        {
        //            ViewBag.MessageType = "error";
        //            ViewBag.Message = "File " + file.FileName + " exceeds the maximum size of " + (MAX_FILE_SIZE / (1024 * 1024)) + " MB.";
        //            return View();
        //        }
        //    }

        //    // Existing code for processing the request...
        //    int crimeType = Convert.ToInt32(IdMasking.Decode(personsData?.typeId));
        //    int? unionId = null;
        //    int? villageId = null;
        //    if (personsData?.unionId != null && personsData?.unionId != "")
        //    {
        //        unionId = Convert.ToInt32(IdMasking.Decode(personsData?.unionId));
        //    }
        //    if (personsData?.villageId != null && personsData?.villageId != "")
        //    {
        //        villageId = Convert.ToInt32(IdMasking.Decode(personsData?.villageId));
        //    }
        //    var personalData = new PersonsData
        //    {
        //        latitude = personsData.latitude,
        //        longitude = personsData.longitude,
        //        mobile = personsData.mobile,
        //        unionId = unionId,
        //        villageId = villageId,
        //    };
        //    int personsDataId = await _persondata.AddPersonsDataAsync(personalData);

        //    if (files != null && files.Count > 0)
        //    {
        //        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ufile");
        //        if (!Directory.Exists(uploadFolder))
        //        {
        //            Directory.CreateDirectory(uploadFolder);
        //        }

        //        List<string> warningFiles = new List<string>();
        //        List<UploadedFiles> uploadedFilesList = new List<UploadedFiles>();

        //        foreach (var file in files)
        //        {
        //            if (Path.GetExtension(file.FileName).ToLower() == ".mp4")
        //            {
        //                string filePath = Path.Combine(uploadFolder, file.FileName);
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await file.CopyToAsync(stream);
        //                }

        //                uploadedFilesList.Add(new UploadedFiles
        //                {
        //                    personsDataId = personsDataId,
        //                    crimeTypeId = crimeType,
        //                    attachmentUrl = Path.Combine("ufile", file.FileName)
        //                });
        //            }
        //            else
        //            {
        //                warningFiles.Add(file.FileName);
        //            }
        //        }

        //        if (uploadedFilesList.Any())
        //        {
        //            await _persondata.AddUploadedFilesAsync(uploadedFilesList);
        //        }

        //        ViewBag.MessageType = "success";
        //        ViewBag.Message = "data_uploaded_successfully";
        //        if (warningFiles.Any())
        //        {
        //            ViewBag.WarningMessage = string.Join(", ", warningFiles);
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.MessageType = "success";
        //        ViewBag.Message = "persons_data_saved_no_files";
        //    }

        //    if (personsData.mobile != null && personsData.mobile != "")
        //    {
        //        ViewBag.userName = IdMasking.Encode(personsData.mobile);
        //    }
        //    var crimeTypes = await _repoCrimeInfo.FindAll();
        //    ViewBag.type = crimeTypes.Select(x => new CrimeTypeViewModel
        //    {
        //        Id = IdMasking.Encode(x.Id.ToString()),
        //        crimeTypeNameBn = x.crimeType
        //    });
        //    return View();
        //}
    }
}
