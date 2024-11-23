using DropSpace.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text.Json;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    public class MediaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPersonData _persondata;

        public MediaController(IPersonData persondata,IWebHostEnvironment webHostEnvironment)
        {
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string mobile)
        {
            var model = new MediaViewModel
            {
                Files = new List<MediaFileViewModel>()
            };

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                var personData = await _persondata.GetPersonDataWithFilesByMobileAsync(IdMasking.Decode(mobile));
                foreach (var item0 in personData)
                {
                    foreach (var item1 in item0.UploadedFiles)
                    {

                        var serverPath = Path.Combine(_webHostEnvironment.WebRootPath, item1.AttachmentUrl);

                        // Validate file exists
                        if (System.IO.File.Exists(serverPath))
                        {
                            model.Files.Add(new MediaFileViewModel
                            {
                                // Use a web-accessible path
                                Url = $"/{item1.AttachmentUrl}",
                                FileType = GetFileType(item1.AttachmentUrl)
                            });
                        }
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> GetPersonDataByMobile([FromBody] MasterDataViewModel model)
        {
            var personData = await _persondata.GetPersonDataWithFilesByMobileAsync(model.mId);
            if (personData == null)
            {
                return NotFound(new { message = "No person data found for the provided mobile number." });
            }
            return Json(personData);
        }
        private string GetFileType(string url)
        {
            if (string.IsNullOrEmpty(url)) return "other";
            return Path.GetExtension(url)?.ToLower() switch
            {
                ".mp4" or ".avi" or ".mov" or ".wmv" => "video",
                ".jpg" or ".jpeg" or ".png" or ".gif" => "image",
                ".mp3" or ".wav" or ".ogg" => "audio",
                ".pdf" => "pdf",
                _ => "other"
            };
        }
    }
}