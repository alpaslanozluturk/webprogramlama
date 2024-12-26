using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Controllers
{
    [Area("KuaforPaneli")]
    public class HizmetlerController : Controller
    {
        private readonly IHizmetRepo _hizmetRepo;
        private readonly ICalisanHizmetRepo _calisanHizmetRepo;

        public HizmetlerController(IHizmetRepo hizmetRepo, ICalisanHizmetRepo calisanHizmetRepo)
        {
            _hizmetRepo = hizmetRepo;
            _calisanHizmetRepo = calisanHizmetRepo;
        }

        public IActionResult Index()
        {
            var hizmetler = _hizmetRepo.GetAll("CalisanHizmets,CalisanHizmets.Kisi");
            return View(hizmetler);
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
            var calisanHizmetleri = _calisanHizmetRepo.GetAllByCondition(ch=>ch.HizmetId==hizmetid);
            foreach(var ch in calisanHizmetleri)
            {
                _calisanHizmetRepo.Delete(ch);
            }
            var hizmet = _hizmetRepo.GetById(hizmetid);
            _hizmetRepo.Delete(hizmet);
            _hizmetRepo.Save();
            return RedirectToAction(nameof(Index), "Hizmetler", new { area = "KuaforPaneli" });
        }
    }
}
