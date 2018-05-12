using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MacD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiểm tra đường dẫn file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    // Lưu đường dẫn file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    // Kiễm tra hình ảnh tồn tại
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    sach.Ngaycapnhat = DateTime.Now;
                    // Lưu vào CSDL
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Sach");
        }

        //[HttpPost]
        //public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileupload)
        //{
        //    var fileName = Path.GetFileName(fileupload.FileName);
        //    var path = Path.Combine(Server.MapPath("~/img"), fileName);
        //    if(System.IO.File.Exists(path))
        //    {
        //        ViewBag.Thongbao = "hinh anh da ton tai";
        //    }
        //    else
        //    {
        //        fileupload.SaveAs(path);
        //    }
        //    ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
        //    ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
        //    return View();
        //}

        public ActionResult Sach(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSite = 6;

            return View(data.SACHes.ToList().OrderBy(n=>n.Masach).ToPagedList(pageNumber,pageSite));
        }

        //Hiển thị sản phẩm
        public ActionResult Chitietsach(int id)
        {
            //Lấy ra đối tượng sách theo mã
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lấy ra đối tượng sách cần xóa theo mã
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SACHes.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }

        //Chỉnh sửa sản phẩm
        public ActionResult Suasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            ViewBag.Mota = sach.Mota;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            SACH s = data.SACHes.ToList().Find(n => n.Masach == sach.Masach);
            if (ModelState.IsValid)
            {
               if(fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(sach);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        s.Hinhminhhoa = fileName;
                    }

                }
               
                s.Tensach = sach.Tensach;
                s.Dongia = sach.Dongia;
                s.Donvitinh = sach.Donvitinh;
                s.Mota = sach.Mota;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Soluongban = sach.Soluongban;
                s.solanxem = sach.solanxem;
                s.moi = sach.moi;
                s.Ngaycapnhat = DateTime.Now;
                data.SubmitChanges();
            }
            return RedirectToAction("Sach");
        }

    [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = " phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "phai dang nhap mat khau";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoangadmin"] = ad;
                    return RedirectToAction("index", "Admin");
                }
                else
                    ViewBag.Thongbao = "ten dang nhap hoa mat khau khong dung";

            }
            return View();
        }   
    }
}