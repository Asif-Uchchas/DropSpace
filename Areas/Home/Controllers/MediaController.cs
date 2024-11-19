//using DropSpace.Areas.Home.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http;
//using System.Text.Json;

//namespace DropSpace.Areas.Home.Controllers
//{
//    public class MediaController : Controller
//    {

//        private readonly HttpClient _httpClient;

//        public MediaController(IHttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public async Task<IActionResult> Index(string mobile)
//        {
//            try
//            {
//                var apiUrl = $"https://115.127.99.113:98/api/FileUpload/GetPersonDataByMobile?mobile={mobile}";

//                var handler = new HttpClientHandler();
//                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

//                var response = await _httpClient.GetAsync(apiUrl);
//                response.EnsureSuccessStatusCode();

//                var content = await response.Content.ReadAsStringAsync();
//                var data = JsonSerializer.Deserialize<List<dynamic>>(content);

//                var viewModel = new MediaViewModel
//                {
//                    Files = data
//                        .SelectMany(p => (p.uploadedFiles as JsonElement).EnumerateArray())
//                        .Select(f => new MediaFileViewModel
//                        {
//                            Url = f.GetProperty("attachmentUrl").GetString(),
//                            FileType = GetFileType(f.GetProperty("attachmentUrl").GetString())
//                        })
//                        .ToList()
//                };

//                return View(viewModel);
//            }
//            catch (Exception ex)
//            {
//                return View("Error", ex.Message);
//            }
//        }

//        private string GetFileType(string url)
//        {
//            return Path.GetExtension(url).ToLower() switch
//            {
//                ".mp4" or ".avi" or ".mov" or ".wmv" => "video",
//                ".jpg" or ".jpeg" or ".png" or ".gif" => "image",
//                ".mp3" or ".wav" or ".ogg" => "audio",
//                ".pdf" => "pdf",
//                _ => "other"
//            };
//        }
//    }
//}
