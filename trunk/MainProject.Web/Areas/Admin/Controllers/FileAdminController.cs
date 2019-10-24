using MainProject.Framework.Helper;
using MainProject.Web.BaseControllers;
using System.Web;
using System.Web.Mvc;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class FileAdminController : BaseController
    {
        // GET: Admin/File
        // http://dotnetstep.blogspot.com/2012/05/create-and-read-excel-file-using-oledb.html

        public ActionResult Index()
        {
            return View();
        }


        // Write by MK for inport hard data Area, City, Country, POD, POL for price list
        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            var data = ExcelHelper.Import(file);

            if (data != null)
            {
                //var query = data.Where(x => x.Field<string>("Zone") != null).Select(x => new Temp
                //{
                //    Zone = x.Field<string>("Zone"),
                //    City = x.Field<string>("CITY "),
                //    Country = x.Field<string>("Country"),
                //    AirPortName = x.Field<string>("AirPortName")
                //}).ToList();

                //var areas = query.Select(x => x.Zone).Distinct().Select(x => new Area() { Name = x }).ToList();
                //var countries = query.Select(x => new Country() { Name = x.Country, Area = areas.FirstOrDefault(y => y.Name.Equals(x.Zone)) }).ToList();
                //var cities = query.Select(x => new City() { Name = x.City, Country = countries.FirstOrDefault(y => y.Name.Equals(x.Country)) }).ToList();
                //var pod = query.Where(x => !x.Country.Equals("VIETNAM")).Select(x => new POD() { Name = x.AirPortName, Country = countries.FirstOrDefault(y => y.Name.Equals(x.Country)) }).ToList();
                //var pol = query.Where(x => x.Country.Equals("VIETNAM")).Select(x => new POL() { Name = x.AirPortName, City = cities.FirstOrDefault(y => y.Name.Equals(x.City)) }).ToList();

                ////DbContext.Areas.AddRange(areas);
                ////DbContext.Countries.AddRange(countries);
                ////DbContext.Cities.AddRange(cities);
                ////DbContext.PODs.AddRange(pod);
                ////DbContext.POLs.AddRange(pol);
                ////DbContext.SaveChanges();

                TempData["message"] = string.Format("<strong style='color:green'>Import excel thành công!</strong>");
            }
            else
            {
                TempData["message"] = string.Format("<strong style='color:green'>Không đúng định dạng file hoặc đã xảy ra lỗi!</strong>");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ExportExcel()
            => Json(ExcelHelper.Export(), JsonRequestBehavior.AllowGet);
    }
}