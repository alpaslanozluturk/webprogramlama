using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Controllers
{
    public class KimlikController : Controller
    {
        private readonly UserManager<Kisi> _userManager;
        private readonly SignInManager<Kisi> _signInManager;

        public KimlikController(UserManager<Kisi> userManager, SignInManager<Kisi> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Giris(GirisModel girisModel)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Kayit(KayitModel kayitModel)
        {
            return View();
        }
        public IActionResult Cikis()
        {
            return View();
        }
    }
}
