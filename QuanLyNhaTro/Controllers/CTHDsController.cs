using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaTro.Models;

using System.Net.Mail;
namespace QuanLyNhaTro.Controllers
{
    public class CTHDsController : Controller
    {
        private qlphongtrosvEntities db = new qlphongtrosvEntities();

        // GET: CTHDs
        public ActionResult Index(string mahd)
        {
            using (qlphongtrosvEntities db = new qlphongtrosvEntities())
            {
                List<KhachHang> khachHang = db.KhachHangs.ToList();
                List<HoaDon> hoaDon = db.HoaDons.ToList();
                List<CTHD> cthd = db.CTHDs.ToList();
                List<DSPhong> dsPhong = db.DSPhongs.ToList();
                var hd = db.CTHDs.Where(x => x.MaHD == mahd).SingleOrDefault();
                var main = from h in hoaDon
                           join d in dsPhong on h.MaPhong equals d.MaPhong
                           where (h.MaHD ==  mahd)
                           select new ViewModel
                           {
                               hoaDon = h,
                               dsPhong = d
                           };
                var sub = from c in cthd
                          join k in khachHang on c.MaKH equals k.MaKH
                          where (c.MaHD == mahd)
                          select new ViewModel
                          {
                              cthd = c,
                              khachHang = k,
                              ThanhTien = Convert.ToDouble(c.HoaDon.DSPhong.GiaTienPhong + c.GiaDien * c.SoDien + c.GiaNuoc * c.SoNuoc)

                          };
                if (hd == null)
                {
                    ViewBag.TinhTranghd = 0;
                    ViewBag.ID = mahd;
                }
                ViewBag.Main = main;
                ViewBag.Sub = sub;
                return View();

            }
        }
        public ActionResult Order(string Email,string Phone,string mahd)
        {
            List<KhachHang> khachHang = db.KhachHangs.ToList();
            List<HoaDon> hoaDon = db.HoaDons.ToList();
            List<CTHD> cthd = db.CTHDs.ToList();
            List<DSPhong> dsPhong = db.DSPhongs.ToList();
            var hd = db.CTHDs.Where(x => x.MaHD == mahd).SingleOrDefault();
            var main = from h in hoaDon
                       join d in dsPhong on h.MaPhong equals d.MaPhong
                       where (h.MaHD == mahd)
                       select new ViewModel
                       {
                           hoaDon = h,
                           dsPhong = d
                       };
            var sub = from c in cthd
                      join k in khachHang on c.MaKH equals k.MaKH
                      where (c.MaHD == mahd)
                      select new ViewModel
                      {
                          cthd = c,
                          khachHang = k,
                          ThanhTien = Convert.ToDouble(c.HoaDon.DSPhong.GiaTienPhong + c.GiaDien * c.SoDien + c.GiaNuoc * c.SoNuoc)

                      };
            if (hd == null)
            {
                ViewBag.TinhTranghd = 0;
                ViewBag.ID = mahd;
            }
            ViewBag.Main = main;
            ViewBag.Sub = sub;

            string sMsg;

            sMsg = "<html><body><table border ='1'<caption> Thông Tin Đặt Phòng </caption>  <tr><th> Mã Hóa Đơn </th> <th> Tên Phòng </th> <th> Tên Khách Hàng </th> <th> Mã  Khách Hàng </th> <th> Mã Hóa Đơn  </th> <th> Số Điện </th> <th> Số Nước </th> <th> Giá Điện </th> <th> Giá Nước </th> <th> Giá Tiền Phòng </th> <th> Thành TIền </th> </tr>   ";

            foreach(var m in ViewBag.Main)
             {
                sMsg += "<tr>";
                sMsg += "<td>" + @m.hoaDon.MaHD + "</td>";
                sMsg += "<td>" + @m.hoaDon.MaHD + "</td>";

            }


            foreach (var item in ViewBag.Sub)
            {
                sMsg += "<td>" + @item.khachHang.HoTen + "</td>";
                sMsg += "<td>" + @item.khachHang.MaKH + "</td>";
                sMsg += "<td>" + @item.cthd.MaHD + "</td>";
                sMsg += "<td>" + @item.cthd.SoDien + "</td>";
                sMsg += "<td>" + @item.cthd.SoNuoc + "</td>";
                sMsg += "<td>" + @item.cthd.GiaDien + "</td>";
                sMsg += "<td>" + @item.cthd.GiaNuoc + "</td>";
                sMsg += "<td>" + @String.Format("{0:0,##0}", @item.khachHang.DSPhong.GiaTienPhong) + "</td>";
                sMsg += "<td>" + @String.Format("{0:0,##0}", @item.ThanhTien) + "</td>";
                sMsg += "</tr>";

              

            }



            sMsg += "</table>";
            // gởi email cho khách hàng
            /// Đây là địa chỉ Email của người gửi thư qua cho bạn   ,, Email này là email nhận thư //
            MailMessage mail = new MailMessage("mahoanghainguyenpro@gmail.com", Email, "Thông Tin Đặt Phòng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mahoanghainguyenpro", "Nguyen16102000");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "CTHDs");

        }

        [HttpGet]
        public ActionResult New(string id)
        {
            CTHD cthd = new CTHD();
            HoaDon hd = db.HoaDons.Where(x => x.MaHD == id).SingleOrDefault();
            cthd.MaHD = id;
            cthd.MaKH = hd.MaKH;
            var SoDien = Request["txtSD"];
            var SoNuoc = Request["txtSN"];
            var GiaDien = Request["txtGiaDien"];
            var GiaNuoc = Request["txtGiaNuoc"];
            
            cthd.SoDien = Int32.Parse(SoDien);
            cthd.SoNuoc = Int32.Parse(SoNuoc);
            cthd.GiaDien = Int32.Parse(GiaDien);
            cthd.GiaNuoc = Int32.Parse(GiaNuoc);

            db.CTHDs.Add(cthd);
            db.SaveChanges();

            return RedirectToActionPermanent("Index", "HoaDons");
        }


        // GET: CTHDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.SingleOrDefault(m => m.MaHD == id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            return View(cTHD);
        }

        // GET: CTHDs/Create
        public ActionResult Create()
        {
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH");
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            return View();
        }

        // POST: CTHDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,MaHD,SoDien,SoNuoc,GiaDien,GiaNuoc")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.CTHDs.Add(cTHD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", cTHD.MaKH);
            return View(cTHD);
        }

        // GET: CTHDs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.SingleOrDefault(m => m.MaHD == id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", cTHD.MaKH);
            return View(cTHD);
        }

        // POST: CTHDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,MaHD,SoDien,SoNuoc,GiaDien,GiaNuoc")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTHD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", cTHD.MaKH);
            return View(cTHD);
        }

        // GET: CTHDs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.SingleOrDefault(m => m.MaHD == id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            return View(cTHD);
        }

        // POST: CTHDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CTHD cTHD = db.CTHDs.Find(id);
            db.CTHDs.Remove(cTHD);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
