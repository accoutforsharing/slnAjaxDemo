using Microsoft.AspNetCore.Mvc;
using prjAjaxDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjAjaxDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        public ApiController(DemoContext context)
        {
            _context = context;
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
    }
}
