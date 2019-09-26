using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace viewTry.ViewModel
{
    public class SiteUserModel
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DOB { get; set; }
        public int RoleID { get; set; }
        public String RoleName { get; set; }
    }
}