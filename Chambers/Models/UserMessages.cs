using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chambers.Models
{
    public class UserMessages
    {

        public const string UploadSuccess = "Uploaded Successfully";
        public const string UpdatedSuccess = "Updated Sccessfully";
        public const string ReorderSuccess = "Re ordered Sccessfully";
        public const string FailedUpload = "Upload Failed";
        public const string UnknowError = "Unknow Error contact admin";
        public const string DeleteSuccess = "Deleted Successfully";
        public const string DeleteFailed = "Deleted Failed";
        public const string DocExists = "Document already exists";
        public const string NonPdf = "Not a pdf document";
        public const string MaxPdfSize = "Upload < 5MB pdf document";
        public const string FileNotExistsToDelete = "File does not exists to delete";
    }
}
