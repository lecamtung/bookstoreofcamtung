using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

using PagedList;
using PagedList.Mvc;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: BookStore

        private List<SACH> Laysachmoi(int count )
        {
            
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page ,string SearchString )
        {

            var sachmoi = Laysachmoi(15);
            int pageSize = 6;
            int pageNum = (page ?? 1);

            if (!string.IsNullOrEmpty(SearchString))
            {
                sachmoi = data.SACHes.Where(s => s.Tensach.Contains(SearchString)).ToList();
            }   
                return View(sachmoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult NhaXuatban ()
        {
            var nhaxuatban = from cd in data.NHAXUATBANs select cd;
            return PartialView(nhaxuatban);
        }

        public ActionResult SPTheochude(int id)
        {
            var sach=from s in data.SACHes where s.MaCD==id select s;
            return View(sach);
        }
        public ActionResult SPNhaXuatBan(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }

        public ActionResult Details (int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return View(sach.Single());
        }


    }
}