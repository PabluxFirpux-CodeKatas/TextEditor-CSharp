using System;
using System.IO;

namespace TextEditor.Files
{
    class File
    {
        private string _path;

        public File(String path)
        {
            _path = path;
        }

        public String getFullText()
        {
            if (!System.IO.File.Exists(_path)) return "";
            return System.IO.File.ReadAllText(_path);
        }

        public void saveFile(String text)
        {
            System.IO.File.WriteAllText(_path, text);
        }
    }
}