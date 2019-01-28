using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DublicateFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string startupPath = Environment.CurrentDirectory;
            string testFilesPath = Path.Combine(startupPath, "TestFiles");

            string[] fileArray = Directory.GetFiles(testFilesPath,"*.*",SearchOption.AllDirectories);

            var duplicates = new Dictionary<string, List<string>>();

            foreach(var filePath in fileArray)
            {
                var duplicateFiles = new List<string>();

                for (int i = 0; i < fileArray.Length; i++)
                {
                    if (!filePath.Equals(fileArray[i]) 
                        && Path.GetExtension(filePath) == Path.GetExtension(fileArray[i]))
                    {
                        if (CompareFiles(filePath, fileArray[i]))
                        {
                            duplicateFiles.Add(fileArray[i]);
                        }
                    }
                }

                if(duplicateFiles.Any())
                    duplicates.Add(filePath, duplicateFiles);
            }
        }

        static bool CompareFiles(string baseFile, string testFile)
        {
            var result = false;
            byte[] baseFileBytes = null;
            byte[] testFileBytes = null;

            if (File.Exists(baseFile) && File.Exists(testFile))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(baseFile))
                    {
                        baseFileBytes = md5.ComputeHash(stream);
                    }

                    using (var stream = File.OpenRead(testFile))
                    {
                        testFileBytes = md5.ComputeHash(stream);
                    }
                }
            }

            if (baseFileBytes != null && testFileBytes != null)
            {
                result = baseFileBytes.SequenceEqual(testFileBytes);
            }

            return result;
        }
    }
}
