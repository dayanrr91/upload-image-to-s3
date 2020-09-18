using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadImageToAmazonS3.Models
{
    public class AmazonSettings
    {
        public string ConfigAccess { get; set; }

        public string ConfigSecret { get; set; }

        public string BucketName { get; set; }
    }
}
