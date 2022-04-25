using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjAjaxDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjAjaxDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }


        public IActionResult Index(User user)
        {
            //System.Threading.Thread.Sleep(10000);

            if (string.IsNullOrEmpty(user.name))
                user.name = "Ajax";
            return Content($"<h2>Hello {user.name}, You are {user.age} years old.</h2>", "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult checkName(string name)
        {
            //DemoContext db = new DemoContext();
            //db.Members.Where(m => m.Name == name);
            //_context.Members.Where(m => m.Name == name);

            //var datas = _context.Members.Select(m => m.Name).ToArray();
            var datas = _context.Members.Any(m => m.Name == name);

            if (!string.IsNullOrEmpty(name))
            {
                //if (!datas.Contains(name))
                if (!datas)
                    return Content($"輸入的名稱[{name}], 可以使用");
                return Content("名稱已被使用");
            }
            return Content("null");
        }

        public IActionResult Register(Member member, IFormFile photo)
        {
            //D:\@Study\Homework\Restful+Ajax\slnAjaxDemo\prjAjaxDemo\wwwroot\Images\
            //string info = $"{_host.WebRootPath} - {_host.ContentRootPath}";
            //_host.WebRootPath => D:\@Study\Homework\Restful+Ajax\slnAjaxDemo\prjAjaxDemo\wwwroot

            //將檔案儲存到Images資料夾
            string ImagesFolder = Path.Combine(_host.WebRootPath, "Images", photo.FileName);
            using (var fileStream = new FileStream(ImagesFolder, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            //將圖檔轉成二進位 memoryStream
            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                photo.CopyTo(stream);
                bytes = stream.ToArray();
            }

            //寫進資料庫
            member.FileName = photo.FileName;
            member.FileData = bytes;
            _context.Members.Add(member);
            _context.SaveChanges();

            string info = $"{photo.FileName} - {photo.Length} - {photo.ContentType}";
            //string info = ImagesFolder;
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
            //return Content($"<h2>Hello {user.name}, You are {user.age} years old.\n Your mail is {user.email}</h2>", "text/plain", System.Text.Encoding.UTF8);
        }

        //讀出城市名稱
        public IActionResult City()
        {
            var cities = _context.Addresses.Select(c => new { c.City }).Distinct().OrderBy(c => c.City);
            return Json(cities);
        }

        //讀出鄉鎮區
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(d => d.City ==city).Select(d=>new { d.SiteId}).Distinct().OrderBy(d => d.SiteId);
            return Json(districts);
        }

        //讀出路名
        public IActionResult Roads(string siteId)
        {
            var roads = _context.Addresses.Where(r => r.SiteId == siteId).Select(r => new { r.Road }).Distinct().OrderBy(r => r.Road);
            return Json(roads);
        }

        public IActionResult GetImageByte(int id = 1)
        {
            Member member = _context.Members.Find(id);
            byte[] img = member.FileData;
            return File(img, "image/jpeg");
        }
    }
}
