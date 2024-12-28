using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class Calisan
    {
        [Key]
        public int Id { get; set; }
        public string KisiId { get; set; }
        [ForeignKey(nameof(KisiId))]
        public Kisi Kisi { get; set; }
        [Display(Name = "Giriş Saati")]
        public TimeSpan? TimeStart { get; set; }
        [Display(Name = "Çıkış Saati")]
        public TimeSpan? TimeEnd { get; set; }
        [Display(Name ="Gün")]
        public DayOfWeek? OnDay { get; set; }
    }
}
