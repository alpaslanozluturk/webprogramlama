﻿using ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Models;
using ErkekKuaforu_WebProgramlama.Models;
using ErkekKuaforu_WebProgramlama.Veritabani.Repolar;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ErkekKuaforu_WebProgramlama.Areas.KuaforPaneli.Controllers
{
    [Area("KuaforPaneli")]
    public class KullanicilarController : Controller
    {
        private readonly UserManager<Kisi> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ICalisanRepo _calisanRepo;
        private readonly ICalisanHizmetRepo _calisanHizmetRepo;

        public KullanicilarController(UserManager<Kisi> userManager, RoleManager<Rol> roleManager, ICalisanRepo calisanRepo, ICalisanHizmetRepo calisanHizmetRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _calisanRepo = calisanRepo;
            _calisanHizmetRepo = calisanHizmetRepo;
        }
        public async Task<IActionResult> Index(string? calisanlar)
        {
            // Kullanıcıları al
            var tumKullanicilar = await _userManager.Users.ToListAsync();
            var kullaniciListesi = new List<KullanicilarRollerleModel>();

            foreach (var kullanici in tumKullanicilar)
            {
                // Kullanıcının rollerini al
                var roller = await _userManager.GetRolesAsync(kullanici);
                if (calisanlar == null)
                {
                    kullaniciListesi.Add(new KullanicilarRollerleModel
                    {
                        Id = kullanici.Id,
                        IsimSoyisim = kullanici.Isim + " " + kullanici.Soyisim,
                        Email = kullanici.Email,
                        Rol = roller.SingleOrDefault()
                    });
                }
                else
                {
                    if(roller.SingleOrDefault()== "Manikurist" || roller.SingleOrDefault() == "Masor" || roller.SingleOrDefault() == "Kuafor")
                    {
                        kullaniciListesi.Add(new KullanicilarRollerleModel
                        {
                            Id = kullanici.Id,
                            IsimSoyisim = kullanici.Isim + " " + kullanici.Soyisim,
                            Email = kullanici.Email,
                            Rol = roller.SingleOrDefault()
                        });
                    }
                }
            }

            return View(kullaniciListesi);
        }
        [HttpGet]
        public async Task<IActionResult> Duzenle(string kullaniciid)
        {
            var kullanici = await _userManager.FindByIdAsync(kullaniciid);
            var kullaniciRoller = await _userManager.GetRolesAsync(kullanici);
            var mevcutRoller = _roleManager.Roles;
            var rolListesi = new SelectList(mevcutRoller, "Name", "Name");
            var kullaniciDuzenle = new KullaniciDuzenleModel()
            {
                Id = kullaniciid,
                IsimSoyisim = kullanici.Isim + " " + kullanici.Soyisim,
                Email = kullanici.Email,
                Rol = kullaniciRoller.SingleOrDefault(),
                Roller = rolListesi
            };
            return View(kullaniciDuzenle);
        }
        [HttpPost]
        public async Task<IActionResult> Duzenle(KullaniciDuzenleModel duzenleModel)
        {
            var kullanici = await _userManager.FindByIdAsync(duzenleModel.Id);
            // Kullanıcının mevcut rollerini al
            var kullaniciRolleri = await _userManager.GetRolesAsync(kullanici);
            var silmeSonucu = await _userManager.RemoveFromRolesAsync(kullanici, kullaniciRolleri);
            if (!silmeSonucu.Succeeded)
            {
                // Hata mesajı ekle
                ModelState.AddModelError("", "Rolleri silme işlemi başarısız.");
                return View(duzenleModel);
            }
            var eklemeSonucu = await _userManager.AddToRoleAsync(kullanici, duzenleModel.Rol);

            if (!eklemeSonucu.Succeeded)
            {
                // Hata mesajı ekle
                ModelState.AddModelError("", "Yeni rol atama işlemi başarısız.");
                return View(duzenleModel);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CalisanDuzenle(string kullaniciid)
        {
            var kisi = await _userManager.Users
                .Include(k => k.Calisans)
                .Include(k => k.CalisanHizmets) // İlişkili tabloları yükleyin
                .ThenInclude(ch => ch.Hizmet)
                .FirstOrDefaultAsync(k => k.Id == kullaniciid);
            return View(kisi);
        }
        [HttpPost]
        public IActionResult MesaiEkle(Calisan calisan)
        {
            var varMi = _calisanRepo.GetByIdWithProps(c=>c.KisiId== calisan.KisiId && c.OnDay == calisan.OnDay);
            if (varMi != null)
            {
                varMi.TimeStart = calisan.TimeStart;
                varMi.TimeEnd = calisan.TimeEnd;
                _calisanRepo.Update(varMi);
            }
            else
            {
                var data = new Calisan()
                {
                    KisiId = calisan.KisiId,
                    OnDay = calisan.OnDay,
                    TimeEnd = calisan.TimeEnd,
                    TimeStart = calisan.TimeStart
                };
                _calisanRepo.Add(data);
            }
            _calisanRepo.Save();
            return RedirectToAction("CalisanDuzenle", "Kullanicilar", new {area="KuaforPaneli",kullaniciid=calisan.KisiId});
        }
        public IActionResult CalisanMesaiSil(int calisanid)
        {
            var mesai = _calisanRepo.GetById(calisanid);
            _calisanRepo.Delete(mesai);
            _calisanRepo.Save();
            return RedirectToAction(nameof(CalisanDuzenle),"Kullanicilar", new {area="KuaforPaneli", kullaniciid=mesai.KisiId});
        }
        [HttpPost]
        public IActionResult KullaniciHizmetEkle(KullaniciHizmetModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var calhiz = _calisanHizmetRepo.GetByIdWithProps(ch=>ch.HizmetId == model.SecilenHizmet && ch.KisiId == model.KisiId);
            if(calhiz != null)
            {
                calhiz.Sure=model.Sure;
                _calisanHizmetRepo.Update(calhiz);
                _calisanHizmetRepo.Save();
                return RedirectToAction("CalisanDuzenle", "Kullanicilar", new { area = "KuaforPaneli", kullaniciid = model.KisiId });
            }
            var calisanhizmet = new CalisanHizmet()
            {
                KisiId = model.KisiId,
                HizmetId = model.SecilenHizmet,
                Sure = model.Sure
            };
            _calisanHizmetRepo.Add(calisanhizmet);
            _calisanHizmetRepo.Save();
            return RedirectToAction("CalisanDuzenle", "Kullanicilar", new { area = "KuaforPaneli", kullaniciid = model.KisiId });
        }public IActionResult CalisanHizmetSil(int calisanhizmetid)
        {
            var calisanhizmet = _calisanHizmetRepo.GetById(calisanhizmetid);
            _calisanHizmetRepo.Delete(calisanhizmet);
            _calisanHizmetRepo.Save();
            return RedirectToAction("CalisanDuzenle", "Kullanicilar", new { area = "KuaforPaneli", kullaniciid = calisanhizmet.KisiId });
        }
    }
}