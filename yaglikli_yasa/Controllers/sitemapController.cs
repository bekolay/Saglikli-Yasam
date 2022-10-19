using saglikveri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace yaglikli_yasa.Controllers
{
    public class sitemapController : Controller
    {
        private saglikEntities db = new saglikEntities();
        // GET: sitemap
        public ActionResult Index()
        {
            

            Response.Clear();
            //Response.ContentTpye ile bu Action'ın View'ını XML tabanlı olarak ayarlıyoruz.
            Response.ContentType = "text/xml";
            XmlTextWriter xr = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xr.WriteStartDocument();
            xr.WriteStartElement("urlset");//urlset etiketi açıyoruz
            xr.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xr.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xr.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");
            /* sitemap dosyamızın olmazsa olmazını ekledik. Şeması bu dedik buraya kadar.  */

            var saa = db.Ayarlars.OrderByDescending(p => p.ayarlartarih);
            foreach (var a in saa)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", "https://indirsaglam.com");
                xr.WriteElementString("lastmod", a.ayarlartarih.ToString("yyyy-MM-dd"));
                xr.WriteElementString("priority", "1");
                xr.WriteEndElement();
            }

            //Aynı şekilde burada da Bolgeleri SiteMap'e ekliyoruz.
            var k = db.Bloglars.Where(p => p.blogaktif == true).OrderByDescending(p => p.blogTarih);
            foreach (var b in k)
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", "https://indirsaglam.com/d/" + b.blogUrl);
                xr.WriteElementString("lastmod", b.blogTarih.ToString("yyyy-MM-dd"));
                xr.WriteElementString("priority", "0.6");
                xr.WriteEndElement();
            }

            xr.WriteEndDocument();
            //urlset etiketini kapattık
            xr.Flush();
            xr.Close();
            Response.End();
            return View();
        }
    }
}