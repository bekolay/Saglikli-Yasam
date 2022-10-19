using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using saglikveri;
using WebMatrix.WebData;

namespace yaglikli_yasa.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class AyarlarsController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/Ayarlars
        public ActionResult Index()
        {
            var ayarlars = db.Ayarlars.Include(a => a.Kullanici);
            return View(ayarlars.ToList());
        }

        // GET: Admin/Ayarlars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ayarlar ayarlar = db.Ayarlars.Find(id);
            if (ayarlar == null)
            {
                return HttpNotFound();
            }
            return View(ayarlar);
        }

        // GET: Admin/Ayarlars/Create
        public ActionResult Create()
        {
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi");
            return View();
        }

        // POST: Admin/Ayarlars/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ayarlarid,Siteadi,Sitemail,Smtp,port,ayarlarsifre,ayarlartarih,kullanicid")] Ayarlar ayarlar)
        {
            if (ModelState.IsValid)
            {
                db.Ayarlars.Add(ayarlar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", ayarlar.kullanicid);
            return View(ayarlar);
        }

        // GET: Admin/Ayarlars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ayarlar ayarlar = db.Ayarlars.Find(id);
            if (ayarlar == null)
            {
                return HttpNotFound();
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", ayarlar.kullanicid);
            return View(ayarlar);
        }

        // POST: Admin/Ayarlars/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ayarlarid,Siteadi,Sitemail,Smtp,port,ayarlarsifre,ayarlartarih,kullanicid")] Ayarlar ayarlar)
        {
            if (ModelState.IsValid)
            {
                ayarlar.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                ayarlar.ayarlarid = ayarlar.ayarlarid;
                ayarlar.ayarlartarih = ayarlar.ayarlartarih;
                db.Entry(ayarlar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", ayarlar.kullanicid);
            return View(ayarlar);
        }

        // GET: Admin/Ayarlars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ayarlar ayarlar = db.Ayarlars.Find(id);
            if (ayarlar == null)
            {
                return HttpNotFound();
            }
            return View(ayarlar);
        }

        // POST: Admin/Ayarlars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ayarlar ayarlar = db.Ayarlars.Find(id);
            db.Ayarlars.Remove(ayarlar);
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
