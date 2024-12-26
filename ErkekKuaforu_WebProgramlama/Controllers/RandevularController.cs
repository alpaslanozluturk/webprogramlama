using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Controllers
{
    public class RandevularController : Controller
    {
        private readonly IRandevuRepo randevuRepo;
        private readonly UserManager<Kisi> userManager;
        private readonly IHizmetRepo hizmetRepo;
        private readonly ICalisanHizmetRepo calisanHizmetRepo;
        private readonly ICalisanRepo calisanRepo;
        public RandevularController(IRandevuRepo randevuRepo, UserManager<Kisi> userManager, IHizmetRepo hizmetRepo, ICalisanHizmetRepo calisanHizmetrepo, ICalisanRepo calisanRepo)
        {
            this.randevuRepo = randevuRepo;
            this.userManager = userManager;
            this.hizmetRepo = hizmetRepo;
            this.calisanHizmetRepo = calisanHizmetrepo;
            this.calisanRepo = calisanRepo;
        }

        public async Task<IActionResult> Index()
        {
            var kullanici = await userManager.FindByEmailAsync(User.Identity!.Name!);
            var randevular = randevuRepo.GetAllByCondition(r => r.MusteriId == kullanici.Id);
            var modellistesi = new List<MusteriRandevuModel>();
            foreach(var randevu in randevular)
            {
                var model = new MusteriRandevuModel()
                {
                    Id = randevu.Id,
                    Tarih = randevu.Tarih,
                    Baslangic = randevu.GirisSaati,
                    Bitis = randevu.CikisSaati,
                    Ucret = randevu.ToplamUcret,
                    isStatus = randevu.isStatus,
                    CalisanHizmetleri = randevu.CalisanHizmetleri
                };
                modellistesi.Add(model);
            }
            return View(modellistesi);
        }
        [HttpGet]
        public IActionResult Ekle()
        {
            var hizmetler = hizmetRepo.GetAll("CalisanHizmets,CalisanHizmets.Kisi");
            return View(hizmetler);
        }
        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] RandevuEkleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model geçersiz.");
            }
            var bolme = model.CalisanHizmetleri.Split(',');

            // Sonuçları tutmak için bir liste
            var calisanHizmetleri = new List<int>();

            foreach (var eleman in bolme)
            {
                var ids = eleman.Split(':');
                if (ids.Length == 2 && int.TryParse(ids[1], out int calisanHizmetId))
                {
                    calisanHizmetleri.Add(calisanHizmetId);
                }
            }
            int totalSure = 0;
            int totalUcret = 0;
            string calhizler = "";
            foreach(var item in calisanHizmetleri)
            {
                var calhiz = calisanHizmetRepo.GetByIdWithProps(i => i.Id == item, "Hizmet");
                totalSure += calhiz.Sure;
                totalUcret += calhiz.Hizmet.Ucret;
                if (calhizler == "")
                {
                    calhizler += item.ToString();
                }
                else
                {
                    calhizler +=","+ item.ToString();
                }
            }
            var kullanici = await userManager.FindByEmailAsync(User.Identity!.Name!);
            var giris = TimeSpan.Parse(model.BaslangicSaati);
            var cikis = giris.Add(TimeSpan.FromMinutes(totalSure));
            var tarih = DateTime.Parse(model.Tarih).Date ;
            var randevular = randevuRepo.GetAllByCondition(r=>r.Tarih==tarih);
            var musteriRandevuCtrl = randevular.Where(mr => mr.MusteriId == kullanici.Id && ((mr.GirisSaati<=giris && giris<=mr.CikisSaati)||(mr.GirisSaati <= cikis && cikis <= mr.CikisSaati)));
            if(musteriRandevuCtrl.FirstOrDefault() != null) return Ok(new {success = false , message = "Müşterinin aynı zaman aralığında mevcut randevusu var" });
            var gun = tarih.DayOfWeek;
            foreach(var calisanHizmet in calisanHizmetleri)
            {
                var calhiz = calisanHizmetRepo.GetByIdWithProps(i=>i.Id == calisanHizmet,"Hizmet");
                var calisan = calisanRepo.GetByIdWithProps(c=>c.KisiId==calhiz.KisiId && c.OnDay==gun);
                if (calisan == null) return Ok(new { success = false, message = $"{calhiz.Hizmet.Isim} hizmetini veren çalışanımız haftanın ilgili gününde izinli." });

                if (calisan.TimeStart > giris  || calisan.TimeEnd < cikis) return Ok(new {success=false,message= $"Randevunuz {calhiz.Hizmet.Isim} hizmetini veren çalışanımız için mesai saatleri dışında." });
            }
            var yeniRandevu = new Randevu()
            {
                MusteriId =kullanici.Id,
                Tarih = tarih.Date,
                GirisSaati = giris,
                CikisSaati = cikis,
                ToplamUcret = totalUcret,
                CalisanHizmetleri =calhizler
            };
            randevuRepo.Add(yeniRandevu);
            randevuRepo.Save();
            return Ok(new {success=true,message= "Randevu başarıyla eklendi." });
        }

        [HttpGet]
        public IActionResult RandevuDetay(string calisanHizmet)
        {
            var calisanhizmetids = calisanHizmet.Split(',');
            var detayliste = new List<RandevuDetay>();
            foreach(var calisanhizmetid in calisanhizmetids)
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
            var calisanhizmeti = calisanHizmetRepo.GetByIdWithProps(x=>x.Id==calisanHizmetId,"Hizmet,Kisi");
            return (new RandevuDetay { Hizmet = calisanhizmeti.Hizmet.Isim, Calisan = calisanhizmeti.Kisi.Isim+" "+ calisanhizmeti.Kisi.Soyisim, Ucret = calisanhizmeti.Hizmet.Ucret.ToString(), Sure = calisanhizmeti.Sure.ToString() });
        }
        public IActionResult Sil(int id)
        {
            randevuRepo.Delete(randevuRepo.GetById(id));
            randevuRepo.Save();
            return RedirectToAction("Index","Randevular");
        }
        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            var randevu = randevuRepo.GetById(id);
            return View(randevu);
        }
        [HttpPost]
        public async Task<IActionResult> Guncelle(Randevu randevumodel)
        {
            if(randevumodel.Id == 0)
            {
                TempData["Message"] = "Randevu alınamadı";
                return View(randevumodel);
            }
            if (randevumodel.Tarih == null || randevumodel.GirisSaati == null)
            {
                TempData["Message"] = "Randevunuzun tarih ve saatini belirtiniz";
                return View(randevumodel);
            }
            var randevu = randevuRepo.GetById(randevumodel.Id);
            var bolme = randevu.CalisanHizmetleri.Split(',');

            // Sonuçları tutmak için bir liste
            var calisanHizmetleri = new List<int>();

            foreach (var eleman in bolme)
            {
                calisanHizmetleri.Add(int.Parse(eleman));
            }
            int totalSure = (int)randevu.CikisSaati.TotalMinutes - (int)randevu.GirisSaati.TotalMinutes;
            var kullanici = await userManager.FindByEmailAsync(User.Identity!.Name!);
            var giris = randevumodel.GirisSaati;
            var cikis = giris.Add(TimeSpan.FromMinutes(totalSure));
            var tarih = randevumodel.Tarih.Date;
            var randevular = randevuRepo.GetAllByCondition(r => r.Tarih == tarih &&r.Id!=randevumodel.Id);
            var musteriRandevuCtrl = randevular.Where(mr => mr.MusteriId == kullanici.Id && ((mr.GirisSaati <= giris && giris <= mr.CikisSaati) || (mr.GirisSaati <= cikis && cikis <= mr.CikisSaati)));
            if (musteriRandevuCtrl.FirstOrDefault() != null)
            {
                TempData["Message"] = "Müşterinin aynı zaman aralığında mevcut randevusu var";
                return View(randevumodel);
            }
            var gun = tarih.DayOfWeek;
            foreach (var calisanHizmet in calisanHizmetleri)
            {
                var calhiz = calisanHizmetRepo.GetByIdWithProps(i => i.Id == calisanHizmet, "Hizmet");
                var calisan = calisanRepo.GetByIdWithProps(c => c.KisiId == calhiz.KisiId && c.OnDay == gun);
                if (calisan == null)
                {
                    TempData["Message"] = $"{calhiz.Hizmet.Isim} hizmetini veren çalışanımız haftanın ilgili gününde izinli.";
                    return View(randevumodel);
                }

                if (calisan.TimeStart > giris || calisan.TimeEnd < cikis)
                {
                    TempData["Message"] = $"Randevunuz {calhiz.Hizmet.Isim} hizmetini veren çalışanımız için mesai saatleri dışında.";
                    return View(randevumodel);
                }
            }
            randevu.Tarih = tarih;
            randevu.GirisSaati = giris;
            randevu.CikisSaati = cikis;
            randevuRepo.Update(randevu);
            randevuRepo.Save();
            return RedirectToAction("Index","Randevular");
        }
    }
}
