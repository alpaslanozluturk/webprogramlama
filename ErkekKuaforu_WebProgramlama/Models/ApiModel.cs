using System.ComponentModel.DataAnnotations;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class ApiModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string KisiId { get; set; }
        public Kisi Kisi { get; set; }
        [Required]
        public string BirinciFotoIsim { get; set; }
        public string? BirinciFotoUrl { get; set; }
        public string? IkinciFotoIsim { get; set; }
        public string? IkinciFotoUrl { get; set; }
        public string? UcuncuFotoIsim { get; set; }
        public string? UcuncuFotoUrl { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
