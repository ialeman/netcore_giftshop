using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using SS.Template.Core;

namespace SS.Template
{
    public sealed class S3FileSystem : IFileSystem
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucket;
        private readonly S3CannedACL _acl;

        public S3FileSystem(IAmazonS3 client, string bucket, S3CannedACL acl)
        {
            _client = client;
            _bucket = bucket;
            _acl = acl;
        }

        public Task Copy(string source, string destination, IDictionary<string, string> metadata = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string path)
        {
            ValidatePathParam(path);

            var response = await _client.DeleteObjectAsync(_bucket, path);
            return IsSuccessful(response.HttpStatusCode);
        }

        private static void ValidatePathParam(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
        }

        public async Task<bool> Exists(string path)
        {
            ValidatePathParam(path);

            var metadata = await _client.GetObjectMetadataAsync(_bucket, path);
            return IsSuccessful(metadata.HttpStatusCode);
        }

        public async Task<bool> Read(Stream outputStream, string path)
        {
            ValidateStreamParam(outputStream);

            ValidatePathParam(path);

            return await ReadInternal(outputStream, path);
        }

        private async Task<bool> ReadInternal(Stream outputStream, string path)
        {
            var response = await _client.GetObjectAsync(_bucket, path);

            if (IsSuccessful(response.HttpStatusCode))
            {
                using (response.ResponseStream)
                {
                    await response.ResponseStream.CopyToAsync(outputStream);
                    outputStream.Position = 0;
                }

                return true;
            }

            return false;
        }

        private static void ValidateStreamParam(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
        }

        public async Task Save(Stream contentStream, string path, IDictionary<string, string> metadata = null)
        {
            ValidateStreamParam(contentStream);

            ValidatePathParam(path);

            await SaveInternal(contentStream, path, metadata);
        }

        private async Task SaveInternal(Stream contentStream, string path, IDictionary<string, string> metadata)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucket,
                Key = path,
                InputStream = contentStream,
                CannedACL = _acl
            };

            if (metadata != null)
            {
                foreach (var kvp in metadata)
                {
                    request.Metadata.Add(kvp.Key, kvp.Value);
                }
            }

            var response = await _client.PutObjectAsync(request);
            if (!IsSuccessful(response.HttpStatusCode))
            {
                throw new IOException(nameof(PutObjectRequest));
            }
        }

        private static bool IsSuccessful(HttpStatusCode status)
        {
            return HttpStatusCode.OK <= status && status < HttpStatusCode.Ambiguous;
        }
    }
}
