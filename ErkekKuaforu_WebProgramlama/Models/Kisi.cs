using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class Kisi : IdentityUser
    {
        [Required(ErrorMessage ="İsim alanı zorunlu")]
        [Display(Name ="İsim")]
        public string Isim { get; set; }
        [Display(Name = "Soyisim")]
        [Required(ErrorMessage = "Soyisim alanı zorunlu")]
        public string Soyisim { get; set; }
        public ICollection<CalisanHizmet> CalisanHizmets { get; set; }
        public ICollection<Calisan> Calisans { get; set; }
    }
}
