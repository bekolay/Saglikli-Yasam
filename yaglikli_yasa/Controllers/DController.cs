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

    public class DController : Controller
    {
        // GET: D
        private saglikEntities db = new saglikEntities();

        public ActionResult Index(string id)
        {
            var bloglar = db.Bloglars
            .Include(p => p.Kategori)
            .Include(p => p.Kullanici)
            .FirstOrDefault(p => p.blogaktif == true && p.blogUrl == id);


            //ViewBag.blogid = bloglar.BlogİD;
            //ViewBag.KullaniciId = WebSecurity.GetUserId(User.Identity.Name);
            ViewBag.Blogİd = bloglar.blogid;
            bloglar.blogGosterim += 1;
            db.SaveChanges();
            return View(bloglar);
        }
    }
}