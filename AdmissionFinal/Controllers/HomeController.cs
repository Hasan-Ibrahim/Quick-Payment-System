using System.Web.Mvc;

namespace AdmissionFinal.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");

            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            else if (User.IsInRole("Hall"))
            {

                return RedirectToAction("Index", "Hall");

            }
            else if (User.IsInRole("Bank"))
            {
                return RedirectToAction("Index", "Bank");
            }
            else if (User.IsInRole("Department"))
            {
                return RedirectToAction("Index", "Department");
            }

            else
            {
                return View();
            }
            
        }
    }
}
