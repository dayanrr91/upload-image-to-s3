using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadImageToAmazonS3.Interfaces;
using UploadImageToAmazonS3.Models;

namespace UploadImageToAmazonS3.Controllers
{
    [Route("api/amazon")]
    public class AmazonController
    {
        private readonly IAmazonService _amazonService;
        public AmazonController(IAmazonService amazonService)
        {
            _amazonService = amazonService;
        }        

        [HttpPost]
        public async Task<string> UploadImageToS3([FromBody]ImageItem imageItem)
        {
            return await _amazonService.UploadImage(imageItem.fullBase64);
        }
    }
}
