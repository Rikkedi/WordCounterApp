using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public class FileDiscoverer
    {
        private DirectoryInfo rootDirectory;

        public FileDiscoverer(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                rootDirectory = new DirectoryInfo(path);
            }
            catch (SecurityException se)
            {
                Console.WriteLine("User does not have permission to access {0}", path);
                throw;
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Path contains invalid characters");
                throw;
            }
            catch (PathTooLongException ptle)
            {
                Console.WriteLine("Input path too long, aborting.");
                rootDirectory = null;
            }
        }

        private static IEnumerable<FileInfo> GetFileEnumeratorForDirectory(DirectoryInfo directory)
        {
            return FileDiscoverer.GetFileEnumeratorByExtension(directory, "*.*");
        }

        private static IEnumerable<FileInfo> GetFileEnumeratorByExtension(DirectoryInfo directory, String extension)
        {
            if (directory == null || !directory.Exists || String.IsNullOrEmpty(extension))
            {
                return null;
            }

            String loweredExtension = extension.ToLower();

            IEnumerable<FileInfo> files = null;

            try
            {
                files = directory.EnumerateFiles().Where<FileInfo>(f => f.Extension.ToLower().Equals(loweredExtension));

            }
            catch (SecurityException se)
            {
                // Just skip the file if we can't access it
                Console.WriteLine(se.Message);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine(dnfe.Message);
            }

            return files;
        }
    }
}
