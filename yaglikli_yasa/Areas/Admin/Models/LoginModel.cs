using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace yaglikli_yasa.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Kullanıcı Adı Boş Geçilemez")]
        public string kullaniciadi { get; set; }

        [Required(ErrorMessage="Şifre Boş Geçilemez")]
        public string kullaniciSifre { get; set; }

        public bool BeniHatirla { get; set; }
    }
}