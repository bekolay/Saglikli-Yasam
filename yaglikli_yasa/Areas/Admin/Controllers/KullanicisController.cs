using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using saglikveri;
using yaglikli_yasa.Areas.Admin.Models;

namespace yaglikli_yasa.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class KullanicisController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/Kullanicis
        public ActionResult Index()
        {
            return View(db.Kullanicis.ToList());
        }

        // GET: Admin/Kullanicis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Kullanicis.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // GET: Admin/Kullanicis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Kullanicis/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kullanicid,kullaniciadi,kullaniciadisoyadi,kullaniciemail,kullanicitarih,kullaniciSifre,KullaniciSifretekrar,KullaniciEskiSifre,KullaniciAktifMi,KullaniciUrl")] Kullanici kullanici)
        {
            var Kullanici = db.Kullanicis.Where(a => a.kullaniciadi == kullanici.kullaniciadi).FirstOrDefault();
            //eğer yoksa kaydı ekle
            if (Kullanici == null)
            {
                if (kullanici.kullaniciSifre == null)
                {
                    ModelState.AddModelError("", "Şifre Girilmedi");
                    return View();
                }
                if (kullanici.KullaniciSifretekrar == null)
                {
                    ModelState.AddModelError("", "Şifre Girilmedi");
                    return View();
                }
                if (kullanici.kullaniciadi == null)
                {
                    ModelState.AddModelError("", "Kullanıcı Adı Girilmedi");
                    return View();
                }
                if (kullanici.kullaniciadisoyadi == null)
                {
                    ModelState.AddModelError("", "Ad Soyad Girilmedi");
                    return View();
                }
                if (kullanici.kullaniciemail == null)
                {
                    ModelState.AddModelError("", "E posta Girilmedi");
                    return View();
                }

                kullanici.kullaniciSifre = kullanici.kullaniciSifre.Md5();
                kullanici.KullaniciEskiSifre = kullanici.KullaniciSifretekrar;
                kullanici.KullaniciSifretekrar = kullanici.KullaniciSifretekrar.Md5();
                kullanici.KullaniciAktifMi = true;
                kullanici.kullanicitarih = DateTime.Now;
                kullanici.KullaniciUrl = kullanici.kullaniciadi.Seo();
                db.Kullanicis.Add(kullanici);
                db.SaveChanges();
                Roles.AddUserToRole(kullanici.kullaniciadi, "Admin");
                ModelState.AddModelError("", "Kayıt Başarılı");
                return RedirectToAction("Index");
            }
            //kullanıcı daha önceden varsa hata mesajı gönder..
            else
            {
                ModelState.AddModelError("", "Kullanıcı Adı Bulunmakta.");
                return View();
            }
        }

        // GET: Admin/Kullanicis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Kullanicis.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Admin/Kullanicis/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kullanicid,kullaniciadi,kullaniciadisoyadi,kullaniciemail,kullanicitarih,kullaniciSifre,KullaniciSifretekrar,KullaniciEskiSifre,KullaniciAktifMi,KullaniciUrl")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                kullanici.KullaniciSifretekrar = kullanici.KullaniciSifretekrar.Md5();
                kullanici.kullaniciSifre = kullanici.kullaniciSifre;
                kullanici.KullaniciAktifMi = kullanici.KullaniciAktifMi;
                kullanici.kullanicid = kullanici.kullanicid;
                db.Entry(kullanici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        // GET: Admin/Kullanicis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Kullanicis.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Admin/Kullanicis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kullanici kullanici = db.Kullanicis.Find(id);
            db.Kullanicis.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
