using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ErkekKuaforu_WebProgramlama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IstekController : ControllerBase
    {
        private readonly IApiRepo _apiRepo;

        public IstekController(IApiRepo apiRepo)
        {
            _apiRepo = apiRepo;
        }

        [Route("Ekle")]
        [HttpPost]
        public IActionResult Ekle([FromBody] IlkVeriModel ilkVeriModel)
        {

            var veri = new ApiModel()
            {
                KisiId = ilkVeriModel.KisiId,
                BirinciFotoIsim = ilkVeriModel.Cevap1,
                IkinciFotoIsim = ilkVeriModel.Cevap2,
                UcuncuFotoIsim = ilkVeriModel.Cevap3,
                BirinciFotoUrl = ilkVeriModel.Foto1,
                IkinciFotoUrl = ilkVeriModel.Foto2,
                UcuncuFotoUrl = ilkVeriModel.Foto3
            };
            _apiRepo.Add(veri);
            _apiRepo.Save();

            return Ok(new { Id = veri.Id });
        }
        [HttpGet("Getir/{kullaniciID}")]
        [Route("Getir")]
        public IActionResult Getir(string kullaniciID)
        {
            var model = _apiRepo.GetAllByCondition(a=>a.KisiId==kullaniciID);
            if (model == null || !model.Any())
                return BadRequest("Kullanıcıya ait yapay zeka sorgusu bulunamadı");
            return Ok(model);
        }
    }
}
