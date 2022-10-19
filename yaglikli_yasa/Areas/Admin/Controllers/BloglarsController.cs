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
    public class BloglarsController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/Bloglars
        public ActionResult Index()
        {
            var bloglars = db.Bloglars.Include(b => b.Kategori).Include(b => b.Kullanici);
            return View(bloglars.ToList());
        }

        [HttpPost]
        public ActionResult Cogluresim(IEnumerable<HttpPostedFileBase> ChosenFiles)
        {
            foreach (var item in ChosenFiles)
            {
                item.SaveAs(HttpContext.Server.MapPath("~/images/" + item.FileName));
            }
            return RedirectToAction("Create");
        }

        // GET: Admin/Bloglars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloglar bloglar = db.Bloglars.Find(id);
            if (bloglar == null)
            {
                return HttpNotFound();
            }
            return View(bloglar);
        }

        // GET: Admin/Bloglars/Create
        public ActionResult Create()
        {
            ViewBag.kategorid = new SelectList(db.Kategoris, "kategorid", "kategoriAdi");
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi");
            return View();
        }

        // POST: Admin/Bloglars/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "blogid,blogAdi,blogUrl,blogAcilama,blogKeyword,bloResim,blogYazi,blogaktif,blogTarih,kullanicid,kategorid,blogGosterim")] Bloglar bloglar)
        {
            string DosyaAdi = bloglar.blogAdi.Seo().ToString().Replace(bloglar.bloResim, "");
            //Kaydetceğimiz resmin uzantısını aldık.
            string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
            string TamYolYeri = "~/images/" + DosyaAdi + uzanti;
            //Eklediğimiz Resni Server.MapPath methodu ile Dosya Adıyla birlikte kaydettik.
            Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));

            if (bloglar.bloResim == null)
            {
                ViewBag.kategorid = new SelectList(db.Kategoris, "kategorid", "kategoriAdi", bloglar.kategorid);
                ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", bloglar.kullanicid);
                ModelState.AddModelError("", "Resim Seçilmedi");
                return View();
            }

            if (ModelState.IsValid)
            {
                bloglar.bloResim = DosyaAdi + uzanti;
                bloglar.blogUrl = bloglar.blogAdi.Seo();
                bloglar.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                int gösterim = bloglar.blogGosterim = 0;
                bloglar.blogTarih = DateTime.Now;
                bloglar.blogaktif = true;
                db.Bloglars.Add(bloglar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kategorid = new SelectList(db.Kategoris, "kategorid", "kategoriAdi", bloglar.kategorid);
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", bloglar.kullanicid);
            return View(bloglar);
        }

        // GET: Admin/Bloglars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloglar bloglar = db.Bloglars.Find(id);
            if (bloglar == null)
            {
                return HttpNotFound();
            }
            ViewBag.kategorid = new SelectList(db.Kategoris, "kategorid", "kategoriAdi", bloglar.kategorid);
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", bloglar.kullanicid);
            return View(bloglar);
        }

        // POST: Admin/Bloglars/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "blogid,blogAdi,blogUrl,blogAcilama,blogKeyword,bloResim,blogYazi,blogaktif,blogTarih,kullanicid,kategorid,blogGosterim")] Bloglar bloglar)
        {
            if (ModelState.IsValid)
            {
                bloglar.blogUrl = bloglar.blogAdi.Seo();
                bloglar.bloResim = bloglar.bloResim;
                bloglar.blogid = bloglar.blogid;
                bloglar.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                bloglar.blogGosterim = bloglar.blogGosterim;
                bloglar.blogTarih = bloglar.blogTarih;
                bloglar.blogaktif = bloglar.blogaktif;
                db.Entry(bloglar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kategorid = new SelectList(db.Kategoris, "kategorid", "kategoriAdi", bloglar.kategorid);
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", bloglar.kullanicid);
            return View(bloglar);
        }

        // GET: Admin/Bloglars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloglar bloglar = db.Bloglars.Find(id);
            if (bloglar == null)
            {
                return HttpNotFound();
            }
            return View(bloglar);
        }

        // POST: Admin/Bloglars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bloglar bloglar = db.Bloglars.Find(id);
            db.Bloglars.Remove(bloglar);
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
