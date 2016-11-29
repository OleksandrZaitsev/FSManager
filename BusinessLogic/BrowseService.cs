using System.Collections.Generic;
using System.IO;

namespace BusinessLogic
{
    public class BrowseService
    {
        private DirInfo dirInfo = new DirInfo();

        public DirInfo GetDirectoryInfo(string dir)
        {
            if (dir != null && dir.Length >= 2)
            {
                dir = dir.Insert(dir.IndexOf('/'), ":");
            }

            if (dir == null)
            {
                dirInfo = GetRootDrives();
            }
            else if (dir.EndsWith(".."))
            {
                dirInfo = GetDirectoryItems(GetPreviousDir(dir));
            }
            else
            {
                dirInfo = GetDirectoryItems(dir);
            }

            SizeStatCalc dirStatCalc = new SizeStatCalc();
            DirInfo countInfo = dirStatCalc.GetFilesSizes(dir);

            if (countInfo.Error != null)
            {
                dirInfo.Error = countInfo.Error;
            }

            else
            {
                dirInfo.Sizes = countInfo.Sizes;
            }

            return dirInfo;
        }

        private string GetPreviousDir(string dir)
        {
            dir = dir.TrimEnd('\\');
            dir = dir.Remove(dir.LastIndexOf(@"\") + 1);

            return dir;
        }

        private DirInfo GetRootDrives()
        {
            dirInfo.FSItems = new List<FSItem>();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo d in drives)
            {
                FSItem item = new FSItem();
                item.Name = d.Name.Replace(@"\", "");
                item.IsDir = true;

                dirInfo.FSItems.Add(item);
            }

            return dirInfo;
        }

        private DirInfo GetDirectoryItems(string dir)
        {
            dirInfo.FSItems = new List<FSItem>();

            try
            {
                string[] folderList = Directory.GetDirectories(dir);
                for (int i = 0; i < folderList.Length; i++)
                {
                    FSItem folder = new FSItem();
                    folder.Name = new DirectoryInfo(folderList[i]).Name;
                    folder.IsDir = true;

                    dirInfo.FSItems.Add(folder);
                }

                string[] fileList = Directory.GetFiles(dir);
                for (int i = 0; i < fileList.Length; i++)
                {
                    FSItem file = new FSItem();
                    file.Name = Path.GetFileNameWithoutExtension(fileList[i]);
                    file.IsDir = false;

                    dirInfo.FSItems.Add(file);
                }
            }
            catch (IOException)
            {
                dirInfo.Error = "NoDrive";
                return dirInfo;
            }
            catch (System.UnauthorizedAccessException)
            {
            }

            return dirInfo;
        }
    }
}
