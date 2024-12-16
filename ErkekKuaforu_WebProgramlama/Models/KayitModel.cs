using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class KayitModel
    {
        [Required(ErrorMessage = ("İsim soyisim girilmelidir."))]
        [Display(Name = "İsim Soyisim")]
        public string IsimSoyisim { get; set; }
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
    }
}
