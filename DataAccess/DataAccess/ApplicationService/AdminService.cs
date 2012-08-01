using System;
using System.Linq;
namespace DataAccess.ApplicationService
{
    public class AdminService 
    {
       
        public static bool ValidateUser(string userName, string password)
        {
            AdminDataAccess admindataaccess = new AdminDataAccess();
            return admindataaccess.ValidateUser(userName, password);
        }
    }
}
