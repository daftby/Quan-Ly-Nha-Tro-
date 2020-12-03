using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhaTro.Models
{
  
    public class ListPhongTrong
    {
        private qlphongtrosvEntities db = new qlphongtrosvEntities();
        public List<DSPhong> ListAll()
        {
            var lst = db.DSPhongs.Where(p => p.TinhTrangPhong == true).ToList();
            return lst;

        }
    }
}