using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaTro.Models;

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
                ViewBag.Main = main;
                ViewBag.Sub = sub;
                return View();

            }
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
