using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using saglikveri;
using WebMatrix.WebData;
using yaglikli_yasa.Areas.Admin.Models;

namespace yaglikli_yasa.Controllers
{
    public class KategoriController : Controller
    {
        // GET: Kategori
        public ActionResult Index([Bind(Include = "kategorid,kategoriAdi,KategoriUrl,KategoriAktif,Kategoritarih,kullanicid")] Kategori kategori, string id, int? page)
        {
            int _page = page ?? 1;
            using (saglikEntities db = new saglikEntities())
            {
                //BlogKategori k = db.BlogKategori.Include(p => p.Kullanic).FirstOrDefault(p => p.KategoriAktif && p.KategoriUrl == id);
                var k = db.Kategoris.Include(p => p.Bloglars).Include(p=> p.Kullanici).FirstOrDefault(p => p.KategoriUrl == id);
                ViewBag.Kategori = k.kategoriAdi;
                ViewBag.url = k.KategoriUrl;


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                return View(k.Bloglars.Where(p => p.blogaktif == true).OrderByDescending(p => p.blogTarih).ToPagedList(_page, 5));
            }
        }
    }
}