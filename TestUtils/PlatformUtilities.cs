using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public static class PlatformUtilities
    {
        /// <summary>
        /// This method should be used when there are no other means of obtaining the name of a file (not its full path). This is typically in non-windows environments.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ShortFileName(string path)
        {
            int lastIndexOfBackslash = path.LastIndexOf(@"\");
            return path.Substring(lastIndexOfBackslash + 1, path.Length - lastIndexOfBackslash - 1);
        }
    }
}
