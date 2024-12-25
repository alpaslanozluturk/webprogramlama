using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Controllers
{
    [Area("KuaforPaneli")]
    public class HizmetlerController : Controller
    {
        private readonly IHizmetRepo _hizmetRepo;

        public HizmetlerController(IHizmetRepo hizmetRepo)
        {
            _hizmetRepo = hizmetRepo;
        }

        public IActionResult Index()
        {
            return View(_hizmetRepo.GetAll("CalisanHizmets,CalisanHizmets.Kisi"));
        }
        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(Hizmet hizmet)
        {
            ModelState.Remove("Id");
            ModelState.Remove("CalisanHizmets");
            if (!ModelState.IsValid) return View(hizmet);
            _hizmetRepo.Add(hizmet);
            _hizmetRepo.Save();
            return RedirectToAction(nameof(Index), "Hizmetler", new {area="KuaforPaneli"});
        }

        [HttpGet]
        public IActionResult Guncelle(int hizmetid)
        {
            return View(_hizmetRepo.GetById(hizmetid));
        }
        [HttpPost]
        public IActionResult Guncelle(Hizmet hizmet)
        {
            ModelState.Remove("CalisanHizmets");
            if (!ModelState.IsValid) return View(hizmet);
            _hizmetRepo.Update(hizmet);
            _hizmetRepo.Save();
            return RedirectToAction(nameof(Index), "Hizmetler", new { area = "KuaforPaneli" });
        }
        public IActionResult Sil(int hizmetid)
        {
            var hizmet = _hizmetRepo.GetById(hizmetid);
            _hizmetRepo.Delete(hizmet);
            _hizmetRepo.Save();
            return RedirectToAction(nameof(Index), "Hizmetler", new { area = "KuaforPaneli" });
        }
    }
}
