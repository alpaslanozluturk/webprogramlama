using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }
        public string Isim { get; set; }
        public int Ucret { get; set; }
        public ICollection<CalisanHizmet> CalisanHizmets { get; set; }
    }
}
