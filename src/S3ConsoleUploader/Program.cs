using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3ConsoleUploader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var awsAccessKey = Environment.GetEnvironmentVariable("AwsAccessKey");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("AwsSecretAccessKey");
            var bucketName = Environment.GetEnvironmentVariable("BucketName");

            if (args.Length != 1 || string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretAccessKey) ||
                string.IsNullOrEmpty(bucketName))
            {
                Console.WriteLine("Not configured");
                return;
            }

            var path = args[0];

            if (File.Exists(path) == false)
            {
                Console.WriteLine("File not found");
                return;
            }

            using (
                IAmazonS3 s3Client = new AmazonS3Client(awsAccessKey, awsSecretAccessKey,
                    new AmazonS3Config {RegionEndpoint = RegionEndpoint.EUWest1}))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = Path.GetFileName(path),
                    FilePath = path
                };
                s3Client.PutObject(request);
                Console.WriteLine("Uploaded!");
            }
        }
    }
}