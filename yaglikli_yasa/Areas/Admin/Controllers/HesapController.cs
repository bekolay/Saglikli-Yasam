using saglikveri;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using yaglikli_yasa.Areas.Admin.Models;

namespace yaglikli_yasa.Areas.Admin.Controllers
{
    public class HesapController : Controller
    {
        private saglikEntities db = new saglikEntities();

        // GET: Admin/Hesap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //string password = Tools.Md5(model.Password);
                string password = model.kullaniciSifre.Md5();

                Kullanici user = db.Kullanicis.FirstOrDefault(p => p.kullaniciadi == model.kullaniciadi && p.kullaniciSifre == password);

                if (user == null)
                {
                    ViewBag.Error = "Kullanıcı Adınız veya Şifreniz Hatalı";
                    return View();
                }

                if (user.KullaniciAktifMi == false)
                {
                    ViewBag.Error = "Banlandınız acaba kim bilir ne yaptınız :)";
                    return View();
                }


                FormsAuthentication.RedirectFromLoginPage(user.kullaniciadi.ToString(), model.BeniHatirla);
            }

            return Redirect(ReturnUrl ?? "/Hesap/AnaSayfa");
        }

        public ActionResult Sifre()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Sifre([Bind(Include = "kullanicid,kullaniciadi,kullaniciadisoyadi,kullaniciemail,kullanicitarih,kullaniciSifre,KullaniciSifretekrar,KullaniciEskiSifre,KullaniciAktifMi,KullaniciUrl")] Kullanici user)
        {
            string guvenlik = user.KullaniciEskiSifre;

            if (ModelState.IsValid)
            {
                Ayarlar epos = db.Ayarlars.FirstOrDefault();


                Kullanici Kullanici = db.Kullanicis.FirstOrDefault(p => p.kullaniciadi == user.kullaniciadi && p.KullaniciEskiSifre == guvenlik);

                if (Kullanici == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı bulunamadı ve ya eski şifren yanlış");
                    return View();
                }


                string randomParola = "";
                Random rastgele = new Random();
                for (int i = 0; i < 6; i++)
                {
                    randomParola += rastgele.Next(0, 9);
                }



                

                MailMessage mail = new MailMessage(); // mail adında MailMessage nesnesi yaratıyoruz.

                mail.From = new MailAddress(epos.Sitemail); //Mailin kimden gittiğini belirtiyoruz

                mail.To.Add(Kullanici.kullaniciemail); //Mailin kime gideceğini belirtiyoruz

                mail.Subject = epos.Siteadi + " - Şifre Sıfırlama"; //Mail konusu 

                mail.Body = "İyi günler " + Kullanici.kullaniciadisoyadi + " Bir şifre sıfırlama talebinde bulunduz ve şifreniz : " + randomParola;

                SmtpClient sc = new SmtpClient(); //sc adında SmtpClient nesnesi yaratıyoruz.

                sc.EnableSsl = true; //SSL’i etkinleştirdik.
                sc.UseDefaultCredentials = false;
                sc.Port = 587; //Gmail için geçerli Portu bildiriyoruz
                sc.Host = "smtp.gmail.com"; //Gmailin smtp host adresini belirttik


                sc.Credentials = new NetworkCredential(epos.Sitemail, epos.ayarlarsifre); //Gmail hesap kontrolü için bilgilerimizi girdik

                

                sc.Send(mail); //Mailinizi gönderiyoruz.

                ModelState.AddModelError("", "Göderildi");
                Kullanici.kullaniciSifre = randomParola.Md5();
                db.SaveChanges();
                return View();
            }
            return View();
        }

        public ActionResult Cikis()
        {
            Kullanici kullanici3 = db.Kullanicis.FirstOrDefault(p => p.kullaniciadi == p.kullaniciadi);

            Session["Kullanici3"] = kullanici3.kullaniciadisoyadi;

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}