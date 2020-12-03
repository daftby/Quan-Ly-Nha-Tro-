using QuanLyNhaTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNhaTro.Controllers
{
    public class DSphongtrongController : Controller
    {
        private qlphongtrosvEntities db = new qlphongtrosvEntities();
        // GET: DSphongtrong
        public ActionResult Index()
        {
            var lstPhongTrong = db.DSPhongs.Where(p => p.TinhTrangPhong == true).ToList();
            return View(lstPhongTrong);
        }
    }
}