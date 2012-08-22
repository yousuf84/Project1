using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.ApplicationService
{
   public class MockTestApplicationService
    {
       public static bool Upload(string strFileName, int iClass)
       {
           MockTestUploadDataAccess mocktestupload = new MockTestUploadDataAccess();
          return mocktestupload.Upload(strFileName, iClass);
       }
    }
}
