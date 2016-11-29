using System;
using System.IO;

namespace BusinessLogic
{
    public class SizeStatCalc
    {
        private static int mb = 1024000;

        private DirInfo dirInfo = new DirInfo();

        public SizeStatCalc()
        {
            dirInfo.Sizes = new SizeStat();
        }

        public DirInfo GetFilesSizes(string dir)
        {
            if (dir != null)
            {
                DirectoryInfo root = new DirectoryInfo(dir);
                try
                {
                    GetFiles(root);
                    GetChildDir(root);
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (PathTooLongException)
                {
                }
                catch (IOException)
                {
                    dirInfo.Error = "NoDrive";
                    return dirInfo;
                }
            }

            return dirInfo;
        }

        private void GetChildDir(DirectoryInfo rootDir)
        {
            foreach (DirectoryInfo dir in rootDir.GetDirectories())
            {
                try
                {
                    GetFiles(dir);
                    GetChildDir(dir);
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (PathTooLongException)
                {
                }
                catch (IOException)
                {
                    dirInfo.Error = "NoDrive";
                }
            }
        }

        private void GetFiles(DirectoryInfo dir)
        {
            var files = Directory.EnumerateFiles(dir.FullName, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                GetFileSize(file);
            }
        }

        private void GetFileSize(string curentFile)
        {
            FileInfo file = new FileInfo(curentFile);

            if (file.Length < 10 * mb)
                dirInfo.Sizes.LessTen++;
            else if (file.Length >= 10 * mb && file.Length <= 50 * mb)
                dirInfo.Sizes.BetweenTenFifty++;
            else if (file.Length > 100 * mb)
                dirInfo.Sizes.MoreThanHundred++;
        }
    }
}
