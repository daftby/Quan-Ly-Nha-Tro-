using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace QuanLyNhaTro.Models
{
    public class ViewModel
    {
        public KhachHang khachHang { get; set; }
        public CTHD cthd { get; set; }
        public HoaDon hoaDon { get; set; }
        public DSPhong dsPhong { get; set; }
        [DisplayFormat(DataFormatString ="{0:0.##0}")]
        public double ThanhTien { get; set; }
    }
}