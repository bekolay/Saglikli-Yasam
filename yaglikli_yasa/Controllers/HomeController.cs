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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private saglikEntities db = new saglikEntities();

        public ActionResult Ara(string kelime, int? page)
        {

            var k = db.Kategoris.Include(p => p.Bloglars).Include(p => p.Kullanici).FirstOrDefault(p => p.KategoriAktif == true);
            ViewBag.Kategori = k.kategoriAdi;
            ViewBag.url = k.KategoriUrl;

            int pageIndex = page ?? 1;
            int dataCount = 6;
            ViewBag.Kelime = kelime;
            var blog = db.Bloglars.Include(b => b.Kullanici).Include(p => p.Kategori).Where(p => p.blogaktif == true).OrderByDescending(p => p.blogAdi.Contains(kelime)).ToPagedList(pageIndex, dataCount);
            ViewBag.Kategori = k.kategoriAdi;
            ViewBag.url = k.KategoriUrl;
            var saglik = db.sagliktabloes.Include(b => b.Kullanici).Where(p => p.Saglikaktif == true).OrderByDescending(p => p.saglikAdi.Contains(kelime)).ToPagedList(pageIndex, dataCount);
            return View(blog);
        }
    }
}