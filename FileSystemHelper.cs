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

        public static IEnumerable<FileInfo> FindFiles(string directory, string pattern = "*.*")
        {
            var allFiles = Enumerable.Empty<FileInfo>();

            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(directory));

            while (stack.Any())
            {
                var current = stack.Pop();
                var files = getFiles(current, pattern);
                allFiles = allFiles.Concat(files);

                foreach (var subdirectory in getSubdirectories(current))
                {
                    stack.Push(subdirectory);
                }
            }

            return allFiles;
        }

        private static IEnumerable<FileInfo> getFiles(DirectoryInfo directory, string pattern)
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

        private static IEnumerable<DirectoryInfo> getSubdirectories(DirectoryInfo directory)
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
    }
}
