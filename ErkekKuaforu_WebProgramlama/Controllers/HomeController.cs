using System.Diagnostics;
using System.Text.RegularExpressions;
using Azure;
using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ErkekKuaforu_WebProgramlama.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Kisi> userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<Kisi> userManager, IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            this.userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index([FromBody] IstekVerileriModel istekModel)
        {
            var istek = $"Yuz tipi {istekModel.YuzTipi}, sac tipi {istekModel.SacTipi}, sac uzunlugu {istekModel.SacUzunlugu}, alin genisligi {istekModel.AlinYapisi}" +
                $" olan bir erkek icin sadece bir tane sac modeli oner ve cevap olarak sadece sac modelinin adini soyle.";

            string apiKey = "";
            string model = "gpt-4o";
            // OpenAI Client oluþtur
            var _openAIClient = new OpenAIClient(new OpenAIAuthentication(apiKey));
            // Ýstek oluþtur
            var chatRequest = new ChatRequest(
                new List<Message>
                {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, istek)
                },
                model
            );
            // ChatCompletion isteði gönder
            var chatResponse = await _openAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            var cevap1 = chatResponse.FirstChoice.Message.Content.ToString();
            var istek2 = istek + $"Bu sac modeli {cevap1} modelinden farkli olsun.";
            chatRequest = new ChatRequest(
                new List<Message>
                {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, istek2)
                },
                model
            );
            chatResponse = await _openAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            var cevap2 = chatResponse.FirstChoice.Message.Content.ToString();
            var istek3 = istek + $"Bu sac modeli {cevap1} ve {cevap2} modellerinden farkli olsun.";
            chatRequest = new ChatRequest(
                new List<Message>
                {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, istek3)
                },
                model
            );
            chatResponse = await _openAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            var cevap3 = chatResponse.FirstChoice.Message.Content.ToString();
            var foto1 = "";
            var foto2 = "";
            var foto3 = "";
            var request = new ImageGenerationRequest($"Bir erkek icin on profilden {cevap1} ornegi", null, 1, null, ImageResponseFormat.Url, "256x256", null, null);
            var imageResult = await _openAIClient.ImagesEndPoint.GenerateImageAsync(request);
            foreach ( var image in imageResult)
            {
                foto1 = image;
            }
            request = new ImageGenerationRequest($"Bir erkek icin on profilden {cevap2} ornegi", null, 1, null, ImageResponseFormat.Url, "256x256", null, null);
            imageResult = await _openAIClient.ImagesEndPoint.GenerateImageAsync(request);
            foreach (var image in imageResult)
            {
                foto2 = image;
            }
            request = new ImageGenerationRequest($"Bir erkek icin on profilden {cevap3} ornegi", null, 1, null, ImageResponseFormat.Url, "256x256", null, null);
            imageResult = await _openAIClient.ImagesEndPoint.GenerateImageAsync(request);
            foreach (var image in imageResult)
            {
                foto3 = image;
            }
            var kullanici = await userManager.FindByEmailAsync(User.Identity.Name);
            var ilkVeri = new IlkVeriModel();
            ilkVeri.KisiId = kullanici.Id;
            ilkVeri.Cevap1 = cevap1;
            ilkVeri.Cevap2 = cevap2;
            ilkVeri.Cevap3 = cevap3;
            ilkVeri.Foto1 = foto1;
            ilkVeri.Foto2 = foto2;
            ilkVeri.Foto3 = foto3;
            var httpClient = _httpClientFactory.CreateClient();
            var apidenEkle = await httpClient.PostAsJsonAsync("https://localhost:7225/api/Istek/Ekle", ilkVeri);

            if (apidenEkle.IsSuccessStatusCode)
            {
                return RedirectToAction("Cevaplar", "Home");
            }
            else
            {
                return StatusCode((int)apidenEkle.StatusCode, "Veri kaydetme sýrasýnda bir hata oluþtu.");
            }
        }
        [Authorize]
        public async Task<IActionResult> Cevaplar()
        {
            var kullanici = await userManager.FindByEmailAsync(User.Identity.Name);
            var client = _httpClientFactory.CreateClient();
            var cevaplariAl = await client.GetAsync($"https://localhost:7225/api/Istek/Getir/{kullanici.Id}");
            if (!cevaplariAl.IsSuccessStatusCode)
            {
                return StatusCode((int)cevaplariAl.StatusCode, "Veritabanýmýzda kayýtlý geçmiþ sorgunuz yok");
            }

            // 2. Adým: Cevabý al ve JSON olarak deseralize et
            var apiModel = await cevaplariAl.Content.ReadFromJsonAsync<List<ApiModel>>();
            return View(apiModel.OrderByDescending(m => m.Tarih).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
