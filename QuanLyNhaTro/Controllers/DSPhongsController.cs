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
    public class DSPhongsController : Controller
    {
        private qlphongtrosvEntities db = new qlphongtrosvEntities();

        // GET: DSPhongs
        public ActionResult Index()
        {
            return View(db.DSPhongs.ToList());
        }

        // GET: DSPhongs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dsphong = db.DSPhongs.ToList();
            var khachhang = db.KhachHangs.ToList();

            var chitietphong = from k in khachhang
                               join d in dsphong on k.MaPhong equals d.MaPhong
                               where (k.MaPhong == id)
                               select new ViewModel
                               {
                                   khachHang = k,
                                   dsPhong = d
                               };

            //DSPhong dSPhong = db.DSPhongs.Find(id);
            KhachHang kh = db.KhachHangs.Where(x => x.MaPhong == id).SingleOrDefault();
            DSPhong ds = db.DSPhongs.Where(x => x.MaPhong == id).SingleOrDefault();

            //if (dSPhong == null)
            //{
            //    return HttpNotFound();
            //}
            if (kh == null)
            {
                ViewBag.kh = 0;
                ViewBag.dsphong = ds;
            }
            ViewBag.ChiTietPhong = chitietphong;
            return View();
        }

        // GET: DSPhongs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DSPhongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhong,TenPhong,TinhTrangPhong,GiaTienPhong,HinhPhong,ThongTinChiTietPhong")] DSPhong dSPhong)
        {
            if (ModelState.IsValid)
            {
                db.DSPhongs.Add(dSPhong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dSPhong);
        }

        // GET: DSPhongs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSPhong dSPhong = db.DSPhongs.Find(id);
            if (dSPhong == null)
            {
                return HttpNotFound();
            }
            return View(dSPhong);
        }

        // POST: DSPhongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhong,TenPhong,TinhTrangPhong,GiaTienPhong,HinhPhong,ThongTinChiTietPhong")] DSPhong dSPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dSPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dSPhong);
        }

        // GET: DSPhongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSPhong dSPhong = db.DSPhongs.Find(id);
            if (dSPhong == null)
            {
                return HttpNotFound();
            }
            return View(dSPhong);
        }

        // POST: DSPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DSPhong dSPhong = db.DSPhongs.Find(id);
            db.DSPhongs.Remove(dSPhong);
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
