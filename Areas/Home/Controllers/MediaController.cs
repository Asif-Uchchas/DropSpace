using DropSpace.Areas.Home.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace DropSpace.Areas.Home.Controllers
{
    [Area("Home")]
    public class MediaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MediaController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration; 
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
                    var apiUrl = baseUrl + "/api/FileUpload/GetPersonDataByMobile?mobile=" + mobile;
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    };

                    using var client = new HttpClient(handler);
                    // Make POST request with empty content since data is in query string
                    var response = await client.PostAsync(apiUrl, null);

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
                                        model.Files.Add(new MediaFileViewModel
                                        {
                                            Url = url,
                                            FileType = GetFileType(url)
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Failed to retrieve data. Status: {response.StatusCode}. Error: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FetchMedia(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                ModelState.AddModelError(string.Empty, "Mobile number cannot be empty.");
                return View("Index", new MediaViewModel { Files = new List<MediaFileViewModel>() });
            }

            try
            {
                var apiUrl = $"https://localhost:7145/api/FileUpload/GetPersonDataByMobile?mobile={mobile}";
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var client = new HttpClient(handler);
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve data from the server.");
                    return View("Index", new MediaViewModel { Files = new List<MediaFileViewModel>() });
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<JsonElement>>(content);

                var files = new List<MediaFileViewModel>();

                if (data != null)
                {
                    foreach (var person in data)
                    {
                        if (person.TryGetProperty("uploadedFiles", out var filesArray))
                        {
                            foreach (var file in filesArray.EnumerateArray())
                            {
                                var url = file.GetProperty("attachmentUrl").GetString();

                                // No need to set FileName explicitly; it will be derived from the Url
                                files.Add(new MediaFileViewModel
                                {
                                    Url = url,
                                    FileType = GetFileType(url)
                                });
                            }
                        }
                    }
                }

                var viewModel = new MediaViewModel { Files = files };
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View("Index", new MediaViewModel { Files = new List<MediaFileViewModel>() });
            }
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