using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chambers.Models
{
    public class Error
    {

        public ErrorStatus Status { get; set; }
        
        public string Message { get; set; }
    }
    public enum ErrorStatus 
    { 
        Inactive =0,
        Active =1 ,
        Deleted = 2,
        Nonpdf = 3,
        UploadSuccess = 4,
        UploadFailed =5,
        DeleteFailed = 6,
        CreateFailed =7,
        DocExists = 8,
        ReorderSuccess = 9,
        MaxPdfSize = 10,
        UnknownError = 11,
        FileNotExistsToDelete = 12
    }
}
