using DropSpace.Areas.Home.Models;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.PersonData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;
using DropSpace.Models;
using DropSpace.Repository.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(IPersonData persondata, IMasterData masterData, IGenericRepository<CrimeInfo> repoCrimeInfo, IWebHostEnvironment webHostEnvironment)
        {
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
            _masterdata = masterData;
            _repoCrimeInfo = repoCrimeInfo;
        }
        public async Task<IActionResult> Index(string? userName)
        {
            ViewBag.userName = userName;
            var crimeType = await _repoCrimeInfo.FindAll();
            ViewBag.type = crimeType.Select(x => new CrimeTypeViewModel
            {
                Id = IdMasking.Encode(x.Id.ToString()),
                crimeTypeNameBn=x.crimeType
            });
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(2147483648)] // 2GB limit
        public async Task<IActionResult> Index([FromForm] PersonsDataViewModel personsData, [FromForm] IFormFileCollection files)
        {
            if (string.IsNullOrWhiteSpace(personsData?.typeId) && (files == null || files.Count == 0))
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = "invalid_input";
                return View();
            }
            int crimeType = Convert.ToInt32(IdMasking.Decode(personsData?.typeId));
            int? unionId=null;
            int? villageId = null;
            if (personsData?.unionId!=null && personsData?.unionId != "")
            {
                unionId = Convert.ToInt32(IdMasking.Decode(personsData?.unionId));
            }
            if (personsData?.villageId != null && personsData?.villageId != "")
            {
                villageId = Convert.ToInt32(IdMasking.Decode(personsData?.villageId));
            }
            var personalData = new PersonsData
            {
                latitude = personsData.latitude,
                longitude = personsData.longitude,
                mobile = personsData.mobile,
                unionId= unionId,
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
                    if (Path.GetExtension(file.FileName).ToLower() == ".mp4")
                    {
                        string filePath = Path.Combine(uploadFolder, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        uploadedFilesList.Add(new UploadedFiles
                        {
                            personsDataId = personsDataId,
                            crimeTypeId=crimeType,
                            attachmentUrl = Path.Combine("ufile", file.FileName)
                        });
                    }
                    else
                    {
                        warningFiles.Add(file.FileName);
                    }
                }

                if (uploadedFilesList.Any())
                {
                    await _persondata.AddUploadedFilesAsync(uploadedFilesList);
                }

                ViewBag.MessageType = "success";
                ViewBag.Message = "data_uploaded_successfully";
                if (warningFiles.Any())
                {
                    ViewBag.WarningMessage = string.Join(", ", warningFiles);
                }
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
            var crimeTypes = await _repoCrimeInfo.FindAll();
            ViewBag.type = crimeTypes.Select(x => new CrimeTypeViewModel
            {
                Id = IdMasking.Encode(x.Id.ToString()),
                crimeTypeNameBn = x.crimeType
            });
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
    }
}
