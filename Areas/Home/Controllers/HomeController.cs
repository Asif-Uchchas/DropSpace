using DropSpace.Areas.Home.Models;
using DropSpace.Data.Entity;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.MobilePhoneValidation.Interfaces;
using DropSpace.ERPServices.PersonData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;
using DropSpace.Models;
using DropSpace.Repository.Contracts;
using DropSpace.Services.Filehandling.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPersonData _persondata;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMasterData _masterdata;
        private readonly IMobilePhoneValidation _mobilePhoneValidation;
        private readonly IGenericRepository<CrimeInfo> _repoCrimeInfo;
        private readonly IFileHandlingService _fileHandlingService;
        private static Random random = new Random();
        private static string rootPath;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IPersonData persondata, IMasterData masterData, IGenericRepository<CrimeInfo> repoCrimeInfo, IWebHostEnvironment webHostEnvironment, IFileHandlingService fileHandlingService, IMobilePhoneValidation mobilePhoneValidation)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
            _masterdata = masterData;
            _repoCrimeInfo = repoCrimeInfo;
            _fileHandlingService = fileHandlingService;
            _mobilePhoneValidation = mobilePhoneValidation;
            rootPath = "D:\\FileManagement";
        }


        public async Task<IActionResult> Index(string? userName)
        {
            var crimeType = await _repoCrimeInfo.FindAll();
            if (userName != null && userName != "")
            {
                userName=IdMasking.Encode(User.Identity.Name);
                ViewBag.otp = await _mobilePhoneValidation.GetUserOtp(IdMasking.Decode(userName));
                ViewBag.userName = userName;
            }
            var model = new IndexViewModel
            {
                userName= userName,
                fileLimit= await _fileHandlingService.GetFileUploadSettingsAsync(),
                type= crimeType.Select(x => new CrimeTypeViewModel
                {
                    Id = IdMasking.Encode(x.Id.ToString()),
                    crimeTypeNameBn = x.crimeType
                })
            };
            return View(model);
        }

        [HttpPost]
        [RequestSizeLimit(2147483648)] // 2GB limit
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PersonsDataViewModel personsData, [FromForm] IFormFileCollection files)
        {
            var uploadSettings = await _fileHandlingService.GetFileUploadSettingsAsync();
            var fileLimits = uploadSettings.fileLimits.FirstOrDefault();
            var crimeTypes = await _repoCrimeInfo.FindAll();
            var model = new IndexViewModel
            {
                userName = IdMasking.Encode(personsData.mobile),
                fileLimit = await _fileHandlingService.GetFileUploadSettingsAsync(),
                type = crimeTypes.Select(x => new CrimeTypeViewModel
                {
                    Id = IdMasking.Encode(x.Id.ToString()),
                    crimeTypeNameBn = x.crimeType
                })
            };
            #region User Handle
            var userName = User.Identity.Name;
            var user = new ApplicationUser();
            if (userName == null)
            {
                userName = personsData.mobile;
            }
            user = await _userManager.FindByNameAsync(userName);
            var lastOtp = await _mobilePhoneValidation.GetUserLastOtp(personsData.mobile);
            if (user == null && IdMasking.Decode(personsData.newId)== lastOtp)
            {
                user = new ApplicationUser { UserName = personsData.mobile, userType = 1,isActive=1, createdAt = DateTime.Now, createdBy = "Otp Verified", isWhiteList = false };
                var result = await _userManager.CreateAsync(user, userName+"@#$%!@#$"+DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Public");
                    await _signInManager.SignInAsync(user, true);
                }
            }
            else if (user != null && IdMasking.Decode(personsData.newId) == lastOtp)
            {
                await _signInManager.SignInAsync(user, true);
            }
            #endregion
            #region Validation
            if (string.IsNullOrWhiteSpace(personsData?.typeId) && (files == null || files.Count == 0))
            {
                model.MessageType = "error";
                model.Message = "invalid_input";
                return View(model);
            }
            if (user==null)
            {
                model.MessageType = "error";
                model.Message = "You are not authorize";
                return View(model);
            }

            // Find the relevant file limit for the uploaded file type
            var fileType = uploadSettings.fileTypes.FirstOrDefault();
            if (fileType == null)
            {
                model.MessageType = "error";
                model.Message = "File type not found.";
                return View(model);
            }

            var fileLimit = uploadSettings.fileLimits.FirstOrDefault(fl => fl.fileTypeId == fileType.fileTypeId);
            if (fileLimit == null)
            {
                model.MessageType = "error";
                model.Message = "File type limits not found.";
                return View(model);
            }

            // Validate file count
            if (files.Count > fileLimit.totalFileNo)
            {
                model.MessageType = "error";
                model.Message = $"Too many files uploaded. Maximum allowed is {fileLimit.totalFileNo}.";
                return View(model);
            }

            // Validate file sizes
            if (files[0].Length > fileLimit.dayFileSize * 1024 * 1024) // Convert megabytes to bytes
            {
                model.MessageType = "error";
                model.Message = $"File {files[0].FileName} exceeds the maximum size of {fileLimit.totalFileSize} MB.";
                return View(model);
            }
            #endregion

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
                crimeName= personsData.crimeName,
                crimePlace= personsData.locationText,
                crimeDate = personsData.dateOf,
                crimeTime = personsData.timeOf,
            };
            int personsDataId = await _persondata.AddPersonsDataAsync(personalData);

            if (files != null && files.Count ==1)
            {

                string uploadFolder = Path.Combine(rootPath, personsData.mobile);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                List<string> warningFiles = new List<string>();
                List<UploadedFiles> uploadedFilesList = new List<UploadedFiles>();
                var ext = Path.GetExtension(files[0].FileName);
                var fileNewName = Guid.NewGuid()+DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = Path.GetFileNameWithoutExtension(files[0].FileName);
                string filePath = Path.Combine(uploadFolder, fileNewName+ ext);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files[0].CopyToAsync(stream);
                }
                int leng = 2;
                string shortUrl = "";
                bool chUrl =false;
                for (int i = 0; i < leng && chUrl==false; i++)
                {
                    var sUrl = RandomString(15);
                    var checkUrl = await _persondata.CheckShortUrl(sUrl);
                    if (!checkUrl)
                    {
                        chUrl=false;
                    }
                    else
                    {
                        shortUrl = sUrl;
                        chUrl = true;
                    }
                }
                uploadedFilesList.Add(new UploadedFiles
                {
                    personsDataId = personsDataId,
                    crimeTypeId = crimeType,
                    attachmentUrl = fileNewName + ext,
                    shortUrl= shortUrl,
                    newFileName= fileNewName+ ext,
                    oldFileName=files[0].FileName
                });

                if (uploadedFilesList.Any())
                {

                    await _persondata.AddUploadedFilesAsync(uploadedFilesList);
                }

                model.MessageType = "success";
                model.Message = "data_uploaded_successfully";
            }
            else
            {
                model.MessageType = "success";
                model.Message = "persons_data_saved_no_files";
            }

            if (personsData.mobile != null && personsData.mobile != "")
            {
                model.userName = IdMasking.Encode(personsData.mobile);
                ViewBag.otp = await _mobilePhoneValidation.GetUserOtp(personsData.mobile);
                ViewBag.userName = IdMasking.Encode(personsData.mobile);
            }
            return View(model);
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
        public async Task<IActionResult> GetAllDistrict()
        {
            var data = await _masterdata.GetAllDistrict();
            return Json(data.Select(x => new DistrictViewModel
            {
                districtNameBn = x.districtNameBn,
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
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHJKMNPQRSTVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
