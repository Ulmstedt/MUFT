using System;
using System.IO;
using System.Security.Cryptography;

namespace MUFT
{
    static class Utilities
    {
        private static long TERABYTE = 1099511600000;
        private static long GIGABYTE = 1073741824;
        private static long MEGABYTE = 1048576;
        private static long KILOBYTE = 1024;

        // Converts a size to a formatted string
        public static string SizeToString(long size)
        {
            string sizeString;

            if (size > TERABYTE)
            {
                sizeString = ((float)size / TERABYTE).ToString("0.0") + " TB";
            }
            else if (size > GIGABYTE)
            {
                sizeString = ((float)size / GIGABYTE).ToString("0.0") + " GB";
            }
            else if (size > MEGABYTE)
            {
                sizeString = ((float)size / MEGABYTE).ToString("0.0") + " MB";
            }
            else if (size > KILOBYTE)
            {
                sizeString = ((float)size / KILOBYTE).ToString("0.0") + " KB";
            }
            else
            {
                sizeString = size + " B";
            }

            return sizeString;
        }

        public static string TimeToText(TimeSpan t)
        {
            return string.Format("{2:D2}:{1:D2}:{0:D2}", t.Seconds, t.Minutes, t.Hours);
        }

        public static string CalculateMD5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var fs = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-","");
                }
            }
        }

    }
}
