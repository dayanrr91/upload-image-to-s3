using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadImageToAmazonS3.Interfaces
{
    public interface IAmazonService
    {
        Task<string> UploadImage(string fullBase64);
    }
}
