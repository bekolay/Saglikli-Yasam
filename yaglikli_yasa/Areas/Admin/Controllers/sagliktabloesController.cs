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
    public class sagliktabloesController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/sagliktabloes
        public ActionResult Index()
        {
            var sagliktabloes = db.sagliktabloes.Include(s => s.Kullanici);
            return View(sagliktabloes.ToList());
        }

        // GET: Admin/sagliktabloes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sagliktablo sagliktablo = db.sagliktabloes.Find(id);
            if (sagliktablo == null)
            {
                return HttpNotFound();
            }
            return View(sagliktablo);
        }

        // GET: Admin/sagliktabloes/Create
        public ActionResult Create()
        {
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi");
            return View();
        }

        // POST: Admin/sagliktabloes/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "saglikid,kullanicid,saglikAdi,saglikAciklama,saglikKeyword,saglikResim,SaglikYazi,Saglikaktif,saglikTarih,SaglikGosterim,saglikUrl")] sagliktablo sagliktablo)
        {
            string DosyaAdi = Guid.NewGuid().ToString().Replace(sagliktablo.saglikResim, "");
            //Kaydetceğimiz resmin uzantısını aldık.
            string uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
            string TamYolYeri = "~/images/" + DosyaAdi + uzanti;
            //Eklediğimiz Resni Server.MapPath methodu ile Dosya Adıyla birlikte kaydettik.
            Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));

            if (sagliktablo.saglikResim == null)
            {
                ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", sagliktablo.kullanicid);
                ModelState.AddModelError("", "Resim Seçilmedi");
                return View();
            }

            if (ModelState.IsValid)
            {
                sagliktablo.saglikResim = DosyaAdi + uzanti;
                sagliktablo.saglikUrl = sagliktablo.saglikAdi.Seo();
                sagliktablo.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                int gösterim = sagliktablo.SaglikGosterim = 0;
                sagliktablo.saglikTarih = DateTime.Now;
                sagliktablo.Saglikaktif = true;
                db.sagliktabloes.Add(sagliktablo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", sagliktablo.kullanicid);
            return View(sagliktablo);
        }

        // GET: Admin/sagliktabloes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sagliktablo sagliktablo = db.sagliktabloes.Find(id);
            if (sagliktablo == null)
            {
                return HttpNotFound();
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", sagliktablo.kullanicid);
            return View(sagliktablo);
        }

        // POST: Admin/sagliktabloes/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "saglikid,kullanicid,saglikAdi,saglikAciklama,saglikKeyword,saglikResim,SaglikYazi,Saglikaktif,saglikTarih,SaglikGosterim,saglikUrl")] sagliktablo sagliktablo)
        {
            if (ModelState.IsValid)
            {
                sagliktablo.saglikUrl = sagliktablo.saglikAdi.Seo();
                sagliktablo.saglikResim = sagliktablo.saglikResim;
                sagliktablo.saglikid = sagliktablo.saglikid;
                sagliktablo.kullanicid = WebSecurity.GetUserId(User.Identity.Name);
                sagliktablo.SaglikGosterim = sagliktablo.SaglikGosterim;
                sagliktablo.saglikTarih = sagliktablo.saglikTarih;
                sagliktablo.Saglikaktif = sagliktablo.Saglikaktif;
                db.Entry(sagliktablo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kullanicid = new SelectList(db.Kullanicis, "kullanicid", "kullaniciadi", sagliktablo.kullanicid);
            return View(sagliktablo);
        }

        // GET: Admin/sagliktabloes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sagliktablo sagliktablo = db.sagliktabloes.Find(id);
            if (sagliktablo == null)
            {
                return HttpNotFound();
            }
            return View(sagliktablo);
        }

        // POST: Admin/sagliktabloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sagliktablo sagliktablo = db.sagliktabloes.Find(id);
            db.sagliktabloes.Remove(sagliktablo);
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
