using EmployeesPair.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace EmployeesPair.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);

                file.SaveAs(filePath);

                var employeePairs = DataManager.GetEmployeePairs(filePath);

                return View("EmployeePairs", employeePairs);
            }

            return RedirectToAction("Index");
        }
    }
}