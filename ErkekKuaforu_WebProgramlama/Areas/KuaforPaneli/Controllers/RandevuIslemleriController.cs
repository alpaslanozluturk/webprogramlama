using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Controllers
{
    [Area("KuaforPaneli")]
    public class RandevuIslemleriController : Controller
    {
        private readonly IRandevuRepo randevuRepo;
        private readonly UserManager<Kisi> userManager;
        private readonly IHizmetRepo hizmetRepo;
        private readonly ICalisanHizmetRepo calisanHizmetRepo;
        private readonly ICalisanRepo calisanRepo;

        public RandevuIslemleriController(IRandevuRepo randevuRepo, UserManager<Kisi> userManager, IHizmetRepo hizmetRepo, ICalisanHizmetRepo calisanHizmetRepo, ICalisanRepo calisanRepo)
        {
            this.randevuRepo = randevuRepo;
            this.userManager = userManager;
            this.hizmetRepo = hizmetRepo;
            this.calisanHizmetRepo = calisanHizmetRepo;
            this.calisanRepo = calisanRepo;
        }

        public IActionResult Index()
        {
            var randevular = randevuRepo.GetAll("Musteri");
            return View(randevular);
        }
        [HttpGet]
        public IActionResult RandevuDetay(string calisanHizmet)
        {
            var calisanhizmetids = calisanHizmet.Split(',');
            var detayliste = new List<RandevuDetay>();
            foreach (var calisanhizmetid in calisanhizmetids)
            {
                var detaylar = GetDetaylar(int.Parse(calisanhizmetid));
                detayliste.Add(detaylar);
            }

            if (detayliste != null)
            {
                // Detayları döndür
                return Json(new { success = true, data = detayliste });
            }

            // Hata durumu
            return Json(new { success = false, message = "Detaylar bulunamadı" });
        }
        private RandevuDetay GetDetaylar(int calisanHizmetId)
        {
            var calisanhizmeti = calisanHizmetRepo.GetByIdWithProps(x => x.Id == calisanHizmetId, "Hizmet,Kisi");
            return (new RandevuDetay { Hizmet = calisanhizmeti.Hizmet.Isim, Calisan = calisanhizmeti.Kisi.Isim + " " + calisanhizmeti.Kisi.Soyisim, Ucret = calisanhizmeti.Hizmet.Ucret.ToString(), Sure = calisanhizmeti.Sure.ToString() });
        }
        public IActionResult RandevuDurumu(int id, int islem)
        {
            var randevu = randevuRepo.GetById(id);
            if (islem == 0)
                randevu.isStatus = false;
            if(islem==1)
                randevu.isStatus=true;

            randevuRepo.Update(randevu);
            randevuRepo.Save();
            return RedirectToAction("Index","RandevuIslemleri");
        }
    }
}
