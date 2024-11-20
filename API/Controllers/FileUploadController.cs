using DropSpace.Data.Entity;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.AuthService.Interfaces;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.PersonData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DropSpace.API.Controllers
{

    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IUserInfoes userInfoes;
        private readonly IPersonData _persondata;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUploadController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserInfoes userInfoes,  IPersonData persondata, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.userInfoes = userInfoes;
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
        }
        [RequestSizeLimit(2147483648)]
        [HttpPost]
        public async Task<IActionResult> UploadData([FromForm] PersonsData personsData, [FromForm] IFormFileCollection files)
        {
            if ((string.IsNullOrWhiteSpace(personsData?.name) || string.IsNullOrWhiteSpace(personsData?.mobile)) && (files == null || files.Count == 0))
            {
                return BadRequest("Invalid input data. Either PersonsData with valid name and mobile must be provided, or files should be uploaded.");
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
                            //attachmentUrl = Path.Combine("ufile", file.FileName)
                            attachmentUrl = Path.Combine(file.FileName)
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

                return Ok(new
                {
                    Message = "Data uploaded successfully.",
                    Warnings = warningFiles.Any() ? $"The following files were not saved: {string.Join(", ", warningFiles)}" : string.Empty
                });
            }
            return Ok(new
            {
                Message = "PersonsData saved successfully, but no files were uploaded.",
                Warnings = string.Empty
            });

        }

        [HttpPost]
        public async Task<IActionResult> GetPersonDataByMobile(string mobile)
        {
            var personData = await _persondata.GetPersonDataWithFilesByMobileAsync(mobile);
            if (personData == null)
            {
                return NotFound(new { message = "No person data found for the provided mobile number." });
            }
            return Ok(personData);
        }


    }
}
