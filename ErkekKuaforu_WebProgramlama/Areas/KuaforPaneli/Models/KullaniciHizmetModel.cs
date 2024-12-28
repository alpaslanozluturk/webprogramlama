using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models
{
    public class KullaniciHizmetModel
    {
        public string KisiId { get; set; }
        [Display(Name ="Hizmet Seçiniz")]
        public int SecilenHizmet { get; set; }
        [Display(Name = "Hizmet Süresi")]
        public int Sure { get; set; }
    }
}
