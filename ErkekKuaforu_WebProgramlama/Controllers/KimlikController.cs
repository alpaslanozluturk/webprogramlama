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
        public async Task<IActionResult> Giris(GirisModel kisi)
        {
            if (!ModelState.IsValid)
            {
                return View(kisi);
            }
            var hasUser = await _userManager.FindByEmailAsync(kisi.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email ya da şifre yanlış!");
                return View(kisi);
            }
            var result = await _signInManager.PasswordSignInAsync(hasUser.UserName!, kisi.Sifre, isPersistent: true, true);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Kalan deneme hakkınız: {await _userManager.GetAccessFailedCountAsync(hasUser)}/4");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Kayit(KayitModel kisi)
        {
            if (!ModelState.IsValid)
            {
                return View(kisi);
            }
            var kullanici = new Kisi()
            {
                UserName = kisi.Email,
                Email = kisi.Email,
                Isim = kisi.Isim,
                Soyisim = kisi.Soyisim
            };
            var identityResult = await _userManager.CreateAsync(kullanici, kisi.SifreDogrulama);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(kisi);
            }
            return RedirectToAction(nameof(Giris), "Kimlik");
        }
        public IActionResult Cikis()
        {
            return View();
        }
    }
}
