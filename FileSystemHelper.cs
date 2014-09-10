using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownLibrary
{
    public class FileSystemHelper
    {
        // return file with any parent folders created if missing
        public static FileInfo BuildPath(string basePath, params string[] parts)
        {
            var nextFolder = new DirectoryInfo(basePath);
            var folders = parts.Take(parts.Length - 1);

            foreach (var item in folders)
            {
                nextFolder = new DirectoryInfo(Path.Combine(nextFolder.FullName, item));

                if (!nextFolder.Exists)
                    nextFolder.Create();
            }

            return new FileInfo(Path.Combine(nextFolder.FullName, parts.Last()));
        }

        public static IEnumerable<FileInfo> FindAllFiles(string directory, string pattern = "*.*")
        {
            var allFiles = Enumerable.Empty<FileInfo>();

            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(directory));

            while (stack.Any())
            {
                var current = stack.Pop();
                var files = GetFiles(current, pattern);
                allFiles = allFiles.Concat(files);

                foreach (var subdirectory in GetSubdirectories(current))
                {
                    stack.Push(subdirectory);
                }
            }

            return allFiles;
        }

        public static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, string pattern)
        {
            try
            {
                return directory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<FileInfo>();
            }
        }

        public static IEnumerable<DirectoryInfo> GetSubdirectories(DirectoryInfo directory)
        {
            try
            {
                return directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<DirectoryInfo>();
            }
        }

        public static IEnumerable<FileSystemInfo> GetItems(DirectoryInfo directory, string filePattern = "*")
        {
            return GetSubdirectories(directory).OfType<FileSystemInfo>()
                .Concat(GetFiles(directory, filePattern)).OfType<FileSystemInfo>();
        }
    }
}
