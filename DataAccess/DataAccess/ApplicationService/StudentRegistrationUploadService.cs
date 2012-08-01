using System;
using System.Linq;
using System.Data;

namespace DataAccess.ApplicationService
{
   public class StudentRegistrationUploadService
    {
       public static int Upload(string strFileName)
       {
           RegistrationUploadDataAccess registrationupload = new RegistrationUploadDataAccess();
           return registrationupload.Upload(strFileName);
       }

       public static DataTable GetBatchUpload(int iBatchID, int pageSize, int page, string sidx, string sord)
       {
           RegistrationUploadDataAccess registrationupload = new RegistrationUploadDataAccess();
          return registrationupload.GetBatchUpload(iBatchID, pageSize, page, sidx, sord);
       } 
       public static int GetTotalCount(int iBatchID)
       {
           RegistrationUploadDataAccess registrationupload = new RegistrationUploadDataAccess();
           return registrationupload.GetTotalCount(iBatchID);
       }
    }
}
