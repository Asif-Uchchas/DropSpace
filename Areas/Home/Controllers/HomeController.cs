using DropSpace.Data.Entity.Droper;
using DropSpace.ERPServices.PersonData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DropSpace.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPersonData _persondata;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IPersonData persondata, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }



        [HttpPost]
        [RequestSizeLimit(2147483648)] // 2GB limit
        public async Task<IActionResult> Index([FromForm] PersonsData personsData, [FromForm] IFormFileCollection files)
        {
            if ((string.IsNullOrWhiteSpace(personsData?.name) || string.IsNullOrWhiteSpace(personsData?.mobile)) && (files == null || files.Count == 0))
            {
                ViewBag.ErrorMessage = "Invalid input data. Either PersonsData with valid name and mobile must be provided, or files should be uploaded.";
                return View();
            }

            int personsDataId = await _persondata.AddPersonsDataAsync(personsData);

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

                ViewBag.SuccessMessage = "Data uploaded successfully.";
                if (warningFiles.Any())
                {
                    ViewBag.WarningMessage = $"The following files were not saved: {string.Join(", ", warningFiles)}";
                }
            }
            else
            {
                ViewBag.SuccessMessage = "PersonsData saved successfully, but no files were uploaded.";
            }

            return View();
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
