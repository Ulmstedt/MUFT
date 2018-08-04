namespace MUFT
{
    class SimpleFileInfo
    {
        private static long TERABYTE = 1000000000000;
        private static long GIGABYTE = 1000000000;
        private static long MEGABYTE = 1000000;
        private static long KILOBYTE = 1000;

        public string Path { get; set; }
        public string Name { get; set; }
        private long size;
        public long Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                SizeString = SizeToString(value);
            }
        }
        public string SizeString { get; private set; }
        public long Checksum { get; set; }

        public SimpleFileInfo(string path, string name, long size, long checksum)
        {
            this.Path = path;
            this.Name = name;
            this.Size = size;
            this.Checksum = checksum;
        }

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
    }
}
