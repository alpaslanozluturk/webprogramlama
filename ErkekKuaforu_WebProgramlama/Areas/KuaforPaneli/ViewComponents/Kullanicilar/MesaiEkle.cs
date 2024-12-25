using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.ViewComponents.Kullanicilar
{
    public class MesaiEkle : ViewComponent
    {
        public IViewComponentResult Invoke( string kisiId)
        {
            ViewBag.Gunler = Enum.GetValues(typeof(DayOfWeek))
                        .Cast<DayOfWeek>()
                        .Select(d => new SelectListItem
                        {
                            Value = ((int)d).ToString(), // Enum değerini string olarak al
                            Text = d.ToString() // Enum adını al
                        })
                        .ToList();
            return View(new Calisan() { KisiId= kisiId });
        }
    }
}
