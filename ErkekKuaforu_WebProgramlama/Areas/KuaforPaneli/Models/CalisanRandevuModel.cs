namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models
{
    public class CalisanRandevuModel
    {
        public string CalisanId { get; set; }
        public string CalisanIsmi { get; set; }
        public string HizmetIsmi { get; set; }
        public int HizmetSuresi { get; set; }
        public int HizmetUcreti { get; set; }
        public DateTime RandevuTarihi { get; set; }
    }
}
