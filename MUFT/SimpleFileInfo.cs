namespace MUFT
{
    class SimpleFileInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
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
                SizeString = Utilities.SizeToString(value);
            }
        }
        public string SizeString { get; private set; }

        public SimpleFileInfo(string path, string name, long size, bool isDirectory)
        {
            this.Path = path;
            this.Name = name;
            this.Size = size;
            this.IsDirectory = isDirectory;
        }
    }
}
