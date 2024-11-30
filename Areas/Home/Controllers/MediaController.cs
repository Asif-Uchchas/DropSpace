using DropSpace.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text.Json;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;
using Microsoft.AspNetCore.Authorization;
using DropSpace.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    [Authorize(Roles = "Public")]
    public class MediaController : Controller
    {
        private readonly IPersonData _persondata;
        private readonly UserManager<ApplicationUser> _userManager;
        private static string rootPath;

        public MediaController(IPersonData persondata,UserManager<ApplicationUser> userManager)
        {
            _persondata = persondata;
            _userManager = userManager;
            rootPath = "D:\\FileManagement";
        }
        public async Task<IActionResult> Index(string mobile, string otp)
        {
            ViewBag.userName = mobile;
            ViewBag.otp = otp;
            var model = new MediaViewModel
            {
                Files = new List<MediaFileViewModel>()
            };

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                var personData = await _persondata.GetPersonDataWithFilesByMobileAsync(IdMasking.Decode(mobile), IdMasking.Decode(otp));
                foreach (var item0 in personData)
                {
                    foreach (var item1 in item0.UploadedFiles)
                    {

                        var serverPath = Path.Combine("D:\\FileManagement", IdMasking.Decode(mobile), item1.AttachmentUrl);

                        // Validate file exists
                        if (System.IO.File.Exists(serverPath))
                        {
                            model.Files.Add(new MediaFileViewModel
                            {
                                // Use a web-accessible path
                                Url = $"/{item1.AttachmentUrl}",
                                FileType = GetFileType(item1.AttachmentUrl),
                                crimeType = item1.crimeType.crimeType,
                                shortUrl = item1.shortUrl,
                                newFileName = item1.newFileName,
                                oldFileName = item1.oldFileName
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
            var personData = await _persondata.GetPersonDataWithFilesByMobileAsync(model.mId, model.otp);
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

        public async Task<IActionResult> StreamVideo(string link)
        {
            try
            {
                var file = await _persondata.GetUrlFromShortUrl(IdMasking.Decode(link));
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var filePath = Path.Combine(rootPath, User.Identity.Name, file.attachmentUrl);
                if (user.isActive == 1)
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("Video file not found.");
                    }

                    var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    return File(stream, "video/"+Path.GetExtension(filePath), file.oldFileName);
                }
                else
                {
                    return Json("Account is disabled");
                }
            }
            catch (Exception)
            {
                return Json("You are not allowed");
            }
        }
    }
}