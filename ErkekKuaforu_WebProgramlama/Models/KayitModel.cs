using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class KayitModel
    {
        [EmailAddress(ErrorMessage = "Email adresi girmelisiniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Şifre girilmelidir."))]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
        [Compare(nameof(Sifre), ErrorMessage = "Şifreler eşleşmedi!")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ("Şifre Tekrar alanı girilmelidir."))]
        [Display(Name = "Şifre Tekrar")]
        public string SifreDogrulama { get; set; }
        [Required(ErrorMessage = "İsim alanı zorunlu")]
        [Display(Name = "İsim")]
        public string Isim { get; set; }
        [Display(Name = "Soyisim")]
        [Required(ErrorMessage = "Soyisim alanı zorunlu")]
        public string Soyisim { get; set; }
    }
}
