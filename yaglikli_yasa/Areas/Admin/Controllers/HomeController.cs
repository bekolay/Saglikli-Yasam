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

namespace yaglikli_yasa.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        private saglikEntities db = new saglikEntities();
        public ActionResult Index()
        {
            Kullanici kullanici3 = db.Kullanicis.FirstOrDefault(p => p.kullaniciadi == p.kullaniciadi);

            Session["Kullanici3"] = kullanici3.kullaniciadisoyadi;

            return View();
        }

    }
}