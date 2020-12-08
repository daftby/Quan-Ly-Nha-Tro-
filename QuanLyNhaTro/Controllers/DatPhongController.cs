using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaTro.Models;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace QuanLyNhaTro.Controllers
{
    public class DatPhongController : Controller
    {
        // GET: DatPhong
        private qlphongtrosvEntities db = new qlphongtrosvEntities();

        // GET: DatPhong
        public ActionResult Index()
        {
            ListPhongTrong lstPhTrong = new ListPhongTrong();
           
            SelectList List = new SelectList(lstPhTrong.ListAll(), "maphong", "maphong");
            ViewBag.List = List;
            return View();
        }
        [HttpPost]
        public ActionResult Index(ListPhongTrong l, FormCollection f)
        {                                                      
            var MaPhong = f["DropDownList"].ToString();
            var Makh = "KH00" + (db.KhachHangs.Count() + 1).ToString();
            var HoTenKH = f.Get("txtHoTen");
            var CMNDKH = f.Get("txtCMND");
            var ngheNghiepKH = f.Get("txtNgheNghiep");
            var queQuanKH = f.Get("txtQueQuan");
            var ngaySinhKH = f.Get("txtNgaySinh");
            DateTime oDate = Convert.ToDateTime(ngaySinhKH);

            KhachHang kh = new KhachHang();
            kh.MaKH = Makh;
            kh.HoTen = HoTenKH;
            kh.CMND = CMNDKH;
            kh.NgheNghiep = ngheNghiepKH;
            kh.QueQuan = queQuanKH;
            kh.NgaySinh = oDate;
            kh.MaPhong = MaPhong;

            HoaDon hd = new HoaDon();
            hd.MaHD = "HD00" + (db.HoaDons.Count() + 1).ToString();
            hd.MaKH = kh.MaKH;
            hd.MaPhong = kh.MaPhong;
            hd.NgayLapHD = DateTime.Now;

            var phong = db.DSPhongs.Where(c => c.MaPhong == MaPhong).SingleOrDefault();
            if (ModelState.IsValid)
            {
                //cap nhat tinh trang phong
                phong.TinhTrangPhong = false;
                db.Entry(phong).State = EntityState.Modified;
                //them khach hang
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                //them 1 hoa don moi         
                db.HoaDons.Add(hd);
                db.SaveChanges();                
            }

            return RedirectToAction("Index");
        }
        
       
    }
}
