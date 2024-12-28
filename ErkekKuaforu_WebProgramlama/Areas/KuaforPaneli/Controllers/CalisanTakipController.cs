using ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models;
using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Controllers
{
    [Area("KuaforPaneli")]
    [Authorize(Roles ="Admin")]
    public class CalisanTakipController : Controller
    {
        private readonly IRandevuRepo randevuRepo;
        private readonly UserManager<Kisi> userManager;
        private readonly IHizmetRepo hizmetRepo;
        private readonly ICalisanHizmetRepo calisanHizmetRepo;
        private readonly ICalisanRepo calisanRepo;

        public CalisanTakipController(IRandevuRepo randevuRepo, UserManager<Kisi> userManager, IHizmetRepo hizmetRepo, ICalisanHizmetRepo calisanHizmetRepo, ICalisanRepo calisanRepo)
        {
            this.randevuRepo = randevuRepo;
            this.userManager = userManager;
            this.hizmetRepo = hizmetRepo;
            this.calisanHizmetRepo = calisanHizmetRepo;
            this.calisanRepo = calisanRepo;
        }

        public IActionResult Index()
        {
            var randevular = randevuRepo.GetAllByCondition(r => r.isStatus == true, "");
            var listModel = new List<CalisanRandevuModel>();
            foreach(var randevu in randevular)
            {
                var calisanhizmetids = randevu.CalisanHizmetleri.Split(',');
                foreach(var calisanHizmetId in calisanhizmetids)
                {
                    var calisanhizmeti = calisanHizmetRepo.GetByIdWithProps(ch=>ch.Id==int.Parse(calisanHizmetId),"Hizmet,Kisi");
                    var model = new CalisanRandevuModel()
                    {
                        CalisanId = calisanhizmeti.KisiId,
                        CalisanIsmi = calisanhizmeti.Kisi.Isim + " " + calisanhizmeti.Kisi.Soyisim,
                        HizmetIsmi = calisanhizmeti.Hizmet.Isim,
                        HizmetUcreti = calisanhizmeti.Hizmet.Ucret,
                        HizmetSuresi = calisanhizmeti.Sure,
                        RandevuTarihi = randevu.Tarih.Date
                    };
                    listModel.Add(model);
                }
            }
            var sonuc = listModel
            .GroupBy(r => r.RandevuTarihi.Date)// Tarihe göre gruplama
            .Select(gunGrubu => new HizmetOzetModel
            {
                Tarih = gunGrubu.Key.ToString("dd.MM.yyyy"),
                Calisanlar = gunGrubu
                    .GroupBy(r => new { r.CalisanId, r.CalisanIsmi }) // Çalışana göre gruplama
                    .Select(calisanGrubu => new CalisanHizmetModel
                    {
                        Calisan = calisanGrubu.Key.CalisanIsmi,
                        Hizmetler = calisanGrubu
                            .GroupBy(h => h.HizmetIsmi)// Hizmete göre gruplama
                            .Select(hizmetGrubu => new HizmetDetayModel
                            {
                                Hizmet = hizmetGrubu.Key,
                                ToplamSure = hizmetGrubu.Sum(h => h.HizmetSuresi),
                                ToplamUcret = hizmetGrubu.Sum(h => h.HizmetUcreti)
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
            return View(sonuc);
        }
    }
}
