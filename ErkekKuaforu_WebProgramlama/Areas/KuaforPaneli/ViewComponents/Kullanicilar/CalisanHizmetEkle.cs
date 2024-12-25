using ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.ViewComponents.Kullanicilar
{
    public class CalisanHizmetEkle : ViewComponent
    {
        private readonly IHizmetRepo _hizmetRepo;

        public CalisanHizmetEkle(IHizmetRepo hizmetRepo)
        {
            _hizmetRepo = hizmetRepo;
        }
        public IViewComponentResult Invoke(string kisiId)
        {
            var hizmetler = _hizmetRepo.GetAll();
            var hizmetList = new List<(int Id, string Name)>();
            foreach( var hizmet in hizmetler)
            {
                hizmetList.Add((hizmet.Id, hizmet.Isim));
            }
            ViewBag.Hizmetler = hizmetler.Select(h => new SelectListItem
            {
                Value = h.Id.ToString(), // Enum değerini string olarak al
                Text = h.Isim.ToString() // Enum adını al
            })
                        .ToList();
            return View(new KullaniciHizmetModel() { KisiId = kisiId });
        }
    }
}
