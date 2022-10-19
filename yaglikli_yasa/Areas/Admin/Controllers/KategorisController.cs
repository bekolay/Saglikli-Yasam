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
using yaglikli_yasa.Areas.Admin.Models;

namespace yaglikli_yasa.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class KategorisController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/Kategoris
        public ActionResult Index()
        {
            var kategoris = db.Kategoris.Include(k => k.Kullanici);
            return View(kategoris.ToList());
        }

        // GET: Admin/Kategoris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // GET: Admin/Kategoris/Create
        public ActionResult Create()
        {
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi");
            return View();
        }

        // POST: Admin/Kategoris/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kategorid,kategoriAdi,KategoriUrl,KategoriAktif,Kategoritarih,kullanicid")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                kategori.KategoriAktif = true;
                kategori.Kategoritarih = DateTime.Now;
                kategori.KategoriUrl = kategori.kategoriAdi.Seo();
                kategori.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                db.Kategoris.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", kategori.kullanicid);
            return View(kategori);
        }

        // GET: Admin/Kategoris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", kategori.kullanicid);
            return View(kategori);
        }

        // POST: Admin/Kategoris/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kategorid,kategoriAdi,KategoriUrl,KategoriAktif,Kategoritarih,kullanicid")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                kategori.KategoriAktif = kategori.KategoriAktif;
                kategori.Kategoritarih = kategori.Kategoritarih;
                kategori.KategoriUrl = kategori.kategoriAdi.Seo();
                kategori.kategorid = kategori.kategorid;
                kategori.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                db.Entry(kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", kategori.kullanicid);
            return View(kategori);
        }

        // GET: Admin/Kategoris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: Admin/Kategoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategori kategori = db.Kategoris.Find(id);
            db.Kategoris.Remove(kategori);
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
