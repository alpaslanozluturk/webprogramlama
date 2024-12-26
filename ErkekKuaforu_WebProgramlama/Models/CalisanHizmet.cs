using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ErkekKuaforu_WebProgramlama.Models
{
    public class CalisanHizmet
    {
        [Key]
        public int Id { get; set; }
        public int HizmetId { get; set; }
        [ForeignKey(nameof(HizmetId))]
        [JsonIgnore]
        public Hizmet Hizmet { get; set; }
        public string KisiId { get; set; }
        [ForeignKey(nameof(KisiId))]
        public Kisi Kisi { get; set; }
        public int Sure { get; set; }
    }
}
