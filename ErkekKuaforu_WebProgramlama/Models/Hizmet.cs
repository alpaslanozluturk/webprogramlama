using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Hizmet İsmi")]
        public string Isim { get; set; }
        [Display(Name = "Hizmet Ücreti")]
        public int Ucret { get; set; }
        public ICollection<CalisanHizmet> CalisanHizmets { get; set; }
    }
}
