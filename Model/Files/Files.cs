using System;
using System.IO;

namespace TextEditor.Files
{
    class File
    {
        private StreamReader reader;
        public File(String path)
        {
            reader = new StreamReader(path);
        }

        public String getLine()
        {
            return reader.ReadLine();
        }

        public String getFullText()
        {
            String text = "";
            String line = getLine();
            while (line != null)
            {
                text += line;
                text += Environment.NewLine;
                line = getLine();
            }
            return text;
        }
    }
}