using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models
{
    public class KullaniciDuzenleModel
    {
        public string Id { get; set; }
        public string IsimSoyisim { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public SelectList? Roller { get; set; }
    }
}
