using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class CalisanHizmet
    {
        [Key]
        public int Id { get; set; }
        public int HizmetId { get; set; }
        [ForeignKey(nameof(HizmetId))]
        public Hizmet Hizmet { get; set; }
        public string KisiId { get; set; }
        [ForeignKey(nameof(KisiId))]
        public Kisi Kisi { get; set; }
        public int Sure { get; set; }
    }
}
