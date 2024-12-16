using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class GirisModel
    {
        [EmailAddress(ErrorMessage = "Email adresi girmelisiniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrenizi giriniz.")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
    }
}
