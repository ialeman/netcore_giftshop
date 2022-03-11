using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SS.Template.Core;

namespace SS.Template.Infrastructure
{
    public sealed class LocalFileSystem : IFileSystem
    {
        public string Directory { get; }

        public LocalFileSystem(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Base local file directory {directory} does not exist.");
            }

            Directory = directory;
        }

        public Task<bool> Delete(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var fileName = Path.Combine(Directory, path);
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Exists(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var fileName = Path.Combine(Directory, path);
            var exists = File.Exists(fileName);
            return Task.FromResult(exists);
        }

        public async Task<bool> Read(Stream outputStream, string path)
        {
            ValidateReadParams(outputStream, path);

            return await ReadInternal(outputStream, path);
        }

        private async Task<bool> ReadInternal(Stream outputStream, string path)
        {
            var fileName = Path.Combine(Directory, path);
            if (File.Exists(fileName))
            {
                using (var sourceStream = File.Open(fileName, FileMode.Open))
                {
                    await sourceStream.CopyToAsync(outputStream);
                }

                return true;
            }

            return false;
        }

        private static void ValidateReadParams(Stream outputStream, string path)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
        }

        public async Task Save(Stream contentStream, string path,
            IDictionary<string, string> metadata = null)
        {
            ValidateSaveParams(contentStream, path);

            await SaveInternal(contentStream, path);
        }

        private async Task<FileStream> SaveInternal(Stream contentStream, string path)
        {
            var fullPath = Path.Combine(Directory, path);
            var directory = Path.GetDirectoryName(fullPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            var destinationStream = File.Create(fullPath);
            await contentStream.CopyToAsync(destinationStream);
            return destinationStream;
        }

        private static void ValidateSaveParams(Stream contentStream, string path)
        {
            if (contentStream is null)
            {
                throw new ArgumentNullException(nameof(contentStream));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
        }

        public Task Copy(string source, string destination, IDictionary<string, string> metadata = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var sourceFileName = Path.Combine(Directory, source);
            if (File.Exists(sourceFileName))
            {
                var targetFileName = Path.Combine(Directory, destination);
                var targetDirectory = Path.GetDirectoryName(targetFileName);
                if (!System.IO.Directory.Exists(targetDirectory))
                {
                    System.IO.Directory.CreateDirectory(targetDirectory);
                }

                File.Copy(sourceFileName, targetFileName, true);
            }

            return Task.CompletedTask;
        }
    }
}
