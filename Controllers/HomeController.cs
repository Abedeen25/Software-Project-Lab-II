using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using viewTry.Models;
using viewTry.ViewModel;

namespace viewTry.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<SiteUserModel> list = new List<SiteUserModel>();
            using (ViewTryDBEntities dc = new ViewTryDBEntities())
            {
                var v = (from a in dc.SiteUsers
                         join b in dc.UserRoles on a.RoleID equals b.Id
                         select new SiteUserModel
                         {
                             ID = a.Id,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             DOB = a.DOB,
                             RoleID = a.RoleID,
                             RoleName = b.RoleName
                         });
                list = v.ToList();
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult saveUser(int ID, string propertyName, string value)
        {
            var status = false;
            var message = "";

            //Update data to DB
            using (ViewTryDBEntities dc = new ViewTryDBEntities())
            {
                var user = dc.SiteUsers.Find(ID);

                object updateValue = value;
#pragma warning disable CS0219 // The variable 'isValid' is assigned but its value is never used
                bool isValid = true;
#pragma warning restore CS0219 // The variable 'isValid' is assigned but its value is never used

                if (propertyName == "RoleID")
                {
                    int newRoleID = 0;
                    if(int.TryParse(value,out newRoleID))
                    {
                        updateValue = newRoleID;
                        //Update value field
                        value = dc.UserRoles.Where(a => a.Id == newRoleID).First().RoleName;
                    }
                    else
                    {
                        isValid = false;
                    }
                }

                if(user != null)
                {
                    dc.Entry(user).Property(propertyName).CurrentValue = updateValue;
                    dc.SaveChanges();
                    status = true;
                }
                else
                {
                    message = "Error!";
                }
            }

            var response = new {value, status, message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

        public ActionResult GetUserRoles(int ID)
        {
            int selectedRoleID = 0;
            StringBuilder sb = new StringBuilder();
            using (ViewTryDBEntities dc = new ViewTryDBEntities())
            {
                var roles = dc.UserRoles.OrderBy(a => a.RoleName).ToList();
                foreach (var item in roles)
                {
                    sb.Append(string.Format("'{0}': '{1}',", item.Id, item.RoleName));
                }

                selectedRoleID = dc.SiteUsers.Where(a => a.Id == ID).First().RoleID;
            }

            sb.Append(string.Format("'selected': '{0}'", selectedRoleID));
            return Content("{"+sb.ToString()+"}");
        }
    }
}