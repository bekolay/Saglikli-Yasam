using saglikveri;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace yaglikli_yasa.Models
{
    public static class Data
    {

        public static saglikveri.Bloglar[] slider
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.Bloglars.Include(p => p.Kategori).Include(p => p.Kullanici).OrderByDescending(p => p.blogTarih).Where(p => p.blogaktif == true).Take(3).ToArray();
                }
            }
        }

        public static saglikveri.Bloglar[] birinci
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.Bloglars.Include(p => p.Kategori).Include(p => p.Kullanici).OrderByDescending(p => p.blogTarih).Where(p => p.blogaktif == true).Take(1).ToArray();
                }
            }
        }

        public static saglikveri.Bloglar[] ikinci
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.Bloglars.Include(p => p.Kategori).Include(p => p.Kullanici).OrderByDescending(p => p.blogTarih).Skip(1).Where(p => p.blogaktif == true).Take(4).ToArray();
                }
            }
        }

        public static saglikveri.Bloglar[] gosterim
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.Bloglars.Include(p => p.Kategori).Include(p => p.Kullanici).OrderByDescending(p => p.blogGosterim).Where(p => p.blogaktif == true).Take(3).ToArray();
                }
            }
        }

        public static saglikveri.sagliktablo[] saglikt
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.sagliktabloes.Include(p => p.Kullanici).OrderByDescending(p => p.saglikTarih).Where(p => p.Saglikaktif == true).Take(5).ToArray();
                }
            }
        }

        public static saglikveri.Kategori[] kategorii
        {
            get
            {
                using (saglikEntities db = new saglikEntities())
                {
                    return db.Kategoris.Include(p => p.Kullanici).OrderByDescending(p => p.Kategoritarih).Where(p => p.KategoriAktif == true).Take(50).ToArray();
                }
            }
        }

    }
}