namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models
{
    public class HizmetOzetModel
    {
        public string Tarih { get; set; }
        public List<CalisanHizmetModel> Calisanlar { get; set; }
    }

    public class CalisanHizmetModel
    {
        public string Calisan { get; set; }
        public List<HizmetDetayModel> Hizmetler { get; set; }
    }

    public class HizmetDetayModel
    {
        public string Hizmet { get; set; }
        public int ToplamSure { get; set; }
        public int ToplamUcret { get; set; }
    }
}
