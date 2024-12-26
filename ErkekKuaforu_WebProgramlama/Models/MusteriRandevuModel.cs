namespace ErkekKuaforu_WebProgramlama.Models
{
    public class MusteriRandevuModel
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public TimeSpan Baslangic { get; set; }
        public TimeSpan Bitis { get; set; }
        public int Ucret { get; set; }
        public string CalisanHizmetleri { get; set; }
        public bool? isStatus { get; set; }
    }
}
