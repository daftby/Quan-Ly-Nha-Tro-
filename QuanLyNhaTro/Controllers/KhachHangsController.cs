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
    public class KhachHangsController : Controller
    {
        private qlphongtrosvEntities db = new qlphongtrosvEntities();

        // GET: KhachHangs
        public ActionResult Index()
        {
            var khachHangs = db.KhachHangs.Include(k => k.DSPhong);
            return View(khachHangs.ToList());
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            DSPhong dsPhong = db.DSPhongs.Where(x => x.MaPhong == khachHang.MaPhong).SingleOrDefault();
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            if (dsPhong == null)
            {
                ViewBag.dSPhong = 0;
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaPhong = new SelectList(db.DSPhongs, "MaPhong", "TenPhong");
            return View();
        }

        // POST: KhachHangs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTen,CMND,NgheNghiep,QueQuan,NgaySinh,MaPhong")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPhong = new SelectList(db.DSPhongs, "MaPhong", "TenPhong", khachHang.MaPhong);
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhong = new SelectList(db.DSPhongs, "MaPhong", "TenPhong", khachHang.MaPhong);
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTen,CMND,NgheNghiep,QueQuan,NgaySinh,MaPhong")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhong = new SelectList(db.DSPhongs, "MaPhong", "TenPhong", khachHang.MaPhong);
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            var phong = db.DSPhongs.Where(c => c.MaPhong == khachHang.MaPhong).SingleOrDefault();
            
            var hd = db.HoaDons.Where(x => x.MaKH == id).SingleOrDefault();
            var cthd = db.CTHDs.Where(x => x.MaKH == id).SingleOrDefault();

            phong.TinhTrangPhong = true;      
            db.Entry(phong).State = EntityState.Modified;

            if (cthd != null)
            {
                db.CTHDs.Remove(cthd);
                db.SaveChanges();
            }
            
            if (hd != null)
            {
                db.HoaDons.Remove(hd);
                db.SaveChanges();
            }
            
          
            db.KhachHangs.Remove(khachHang);
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
