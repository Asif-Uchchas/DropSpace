using DropSpace.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text.Json;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    public class MediaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MediaController> _logger;

        public MediaController(
            HttpClient httpClient,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ILogger<MediaController> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string mobile)
        {
            var model = new MediaViewModel
            {
                Files = new List<MediaFileViewModel>()
            };

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                try
                {
                    var baseUrl = _configuration["API:baseUrl"];
                    var apiUrl = $"{baseUrl}/api/FileUpload/GetPersonDataByMobile?mobile={mobile}";

                    // Remove custom handler, use default HttpClient
                    var response = await _httpClient.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<List<JsonElement>>(responseContent);

                        if (data != null)
                        {
                            foreach (var person in data)
                            {
                                if (person.TryGetProperty("uploadedFiles", out var filesArray))
                                {
                                    foreach (var file in filesArray.EnumerateArray())
                                    {
                                        var url = file.GetProperty("attachmentUrl").GetString();
                                        var fileName = Path.GetFileName(url);
                                        var serverPath = Path.Combine(_webHostEnvironment.WebRootPath, "ufile", fileName);

                                        // Validate file exists
                                        if (System.IO.File.Exists(serverPath))
                                        {
                                            model.Files.Add(new MediaFileViewModel
                                            {
                                                // Use a web-accessible path
                                                Url = $"/ufile/{fileName}",
                                                FileType = GetFileType(fileName)
                                            });
                                        }
                                        else
                                        {
                                            _logger.LogWarning($"File not found: {serverPath}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"API request failed. Status: {response.StatusCode}. Error: {errorContent}");
                        ModelState.AddModelError(string.Empty, $"Failed to retrieve data. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching media files");
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
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