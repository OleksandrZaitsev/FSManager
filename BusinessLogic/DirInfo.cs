using System.Collections.Generic;

namespace BusinessLogic
{
    public class DirInfo
    {
        public List<FSItem> FSItems { get; set; }
        public SizeStat Sizes { get; set; }
        public string Error { get; set; }
    }
}
