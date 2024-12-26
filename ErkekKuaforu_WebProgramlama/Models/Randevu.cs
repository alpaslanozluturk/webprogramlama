using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }
        public string MusteriId { get; set; }
        [ForeignKey(nameof(MusteriId))]
        public Kisi Musteri { get; set; }
        public DateTime Tarih { get; set; }
        public TimeSpan GirisSaati { get; set; }
        public TimeSpan CikisSaati { get; set; }
        public string CalisanHizmetleri { get; set; }
        public int ToplamUcret { get; set; }
        public bool? isStatus { get; set; }
    }
}
