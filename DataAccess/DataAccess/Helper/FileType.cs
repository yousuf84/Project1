using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Helper
{
    public class FileType : ValidationAttribute
    {
        private readonly string strType;
        public FileType(string strType)
        {
            this.strType = strType;
        }
         public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }
        var postedFileName = ((HttpPostedFileBase)value).FileName.ToLower();
        if (postedFileName.Substring(postedFileName.LastIndexOf('.') + 1) != strType)
        {
            return false;
        }
        return true;
    }

    }
}
