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
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public DayOfWeek? OnDay { get; set; }
    }
}
