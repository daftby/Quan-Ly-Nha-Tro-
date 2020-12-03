using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaTro.Models;
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
            KhachHang kh = new KhachHang();
            DSPhong p = new DSPhong();
            var MaPhong = f["DropDownList"].ToString();
            var Makh = f.Get("txtMakh");
            var HoTenKH = f.Get("txtHoTen");
            var CMNDKH = f.Get("txtCMND");
            var ngheNghiepKH = f.Get("txtNgheNghiep");
            var queQuanKH = f.Get("txtQueQuan");
            var ngaySinhKH = f.Get("txtNgaySinh");
            DateTime oDate = Convert.ToDateTime(ngaySinhKH);

            

            kh.MaKH = Makh;
            kh.HoTen = HoTenKH;
            kh.CMND = CMNDKH;
            kh.NgheNghiep = ngheNghiepKH;
            kh.QueQuan = queQuanKH;
            kh.NgaySinh = oDate;
            kh.MaPhong = MaPhong;

            if (ModelState.IsValid)
            {
                //var phong = db.DSPhongs.Where(p => p.MaPhong == MaPhong).SingleOrDefault();
                //phong.TinhTrangPhong = true;
                

                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kh);
        }
    }
}
