//------------------------------------------------------------------------------
// <auto-generated>
//    Bu kod bir şablondan oluşturuldu.
//
//    Bu dosyada el ile yapılan değişiklikler uygulamanızda beklenmedik davranışa neden olabilir.
//    Kod yeniden oluşturulursa, bu dosyada el ile yapılan değişikliklerin üzerine yazılacak.
// </auto-generated>
//------------------------------------------------------------------------------

namespace saglikveri
{
    using System;
    using System.Collections.Generic;
    
    public partial class Yaptim
    {
        public int yaptimİd { get; set; }
        public int blogid { get; set; }
        public int kullanicid { get; set; }
        public bool yaptimbunu { get; set; }
        public System.DateTime yaptimTarih { get; set; }
    
        public virtual Bloglar Bloglar { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}