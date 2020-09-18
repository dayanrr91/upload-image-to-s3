using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadImageToAmazonS3.Interfaces;
using UploadImageToAmazonS3.Models;

namespace UploadImageToAmazonS3.Services
{
    public class AmazonService : IAmazonService
    {
        private readonly AmazonSettings _amazonSettings;
        public AmazonService(IOptions<AmazonSettings> amazonSettings)
        {
            _amazonSettings = amazonSettings.Value ?? new AmazonSettings();
        }

        public async Task<string> UploadImage(string fullBase64)
        {
            // Use the region you want, in this case we use UsEast1
            var s3Client = new AmazonS3Client(_amazonSettings.ConfigAccess,
                                              _amazonSettings.ConfigSecret,
                                              RegionEndpoint.USEast1);

            try
            {
                var positions = fullBase64.Split(new string[] { ";base64," }, StringSplitOptions.None);
                if (positions.Length <= 1)
                {
                    return await Task.FromResult(((Func<string>)(() =>
                    {
                        return "This is not a valid base 64 format";
                    }))());
                }

                var base64 = positions[1];
                var fileName = await GetFileName(fullBase64);
                return await UploadImageIntoS3Bucket(s3Client, base64, fileName);
            }
            catch (Exception exc)
            {
                // we can log exceptions here, and check what happened
                return await Task.FromResult(((Func<string>)(() =>
                {
                    return exc.Message; ;
                }))());
            }
        }

        #region private methods
        private async Task<string> UploadImageIntoS3Bucket(AmazonS3Client s3Client, string base64, string fileName)
        {
            byte[] bytes = Convert.FromBase64String(base64);

            using (s3Client)
            {
                var request = new PutObjectRequest
                {
                    BucketName = _amazonSettings.BucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = string.Format($"Medium/{fileName}")
                };
                using (var ms = new MemoryStream(bytes))
                {
                    request.InputStream = ms;
                    await s3Client.PutObjectAsync(request);
                }
            }

            return await Task.FromResult(((Func<string>)(() =>
            {
                return "Successfully Upload image to s3";
            }))());
        }

        
        /// <summary>
        ///     Create a dynamic file name, using the current date, so we do not have duplicates
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        private async Task<string> GetFileName(string base64)
        {
            var extension = !string.IsNullOrEmpty(base64) ? base64.Split(new string[] { ";base64," }, StringSplitOptions.None)[0].Replace("data:image/", "") : "";
            string fileName = $"{ DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.{extension}";

            return await Task.FromResult(((Func<string>)(() =>
            {
                return fileName;
            }))());
        }

        #endregion
    }
}
