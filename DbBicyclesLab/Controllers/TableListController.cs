using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbBicyclesLab.Controllers
{
    public class TableListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string widgetName)
        {
            return PartialView(widgetName);
        }
    }
}
