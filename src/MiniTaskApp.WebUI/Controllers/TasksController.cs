using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniTaskApp.WebUI.Controllers
{
    public class TasksController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            return View();
        }
    }
}