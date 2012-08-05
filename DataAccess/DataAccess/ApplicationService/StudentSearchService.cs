using System;
using System.Linq;
using System.Data;

namespace DataAccess.ApplicationService
{
    public class StudentSearchService
    {
        public static DataTable  Search(string StudentGivenName, string StudentSurName, string FatherName, string SchoolName, int pageSize, int page, string sidx, string sord)
        {
            RegistrationUploadDataAccess registrationupload = new RegistrationUploadDataAccess();
           return registrationupload.Search(StudentGivenName, StudentSurName, FatherName, SchoolName, pageSize, page, sidx, sord);
        }


    }
}
