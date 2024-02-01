namespace TextEditor.Interface
{
    class CLIScreen
    {
        List<CLILine> _lines;
        int _num;
        public CLIScreen(String text)
        {
            _lines = new List<CLILine>();
            _num = 0;
            generate(text);
        }

        public List<CLILine> getLines() { return _lines; }

        void generate(String text)
        {
            _num = 0;
            String[] strings = text.Split(Environment.NewLine);
            foreach (String s in strings)
            {
                if (s.Length >= Console.WindowWidth - 2)
                {
                    recursiveLines(s);
                }
                else
                {
                    String line = s;
                    line += Environment.NewLine;
                    _lines.Add(new CLILine(_num + 1, line));
                }
                _num++;
            }
        }

        void recursiveLines(String line)
        {
            if (line.Length >= Console.WindowWidth - 2)
            {
                _lines.Add(new CLILine(_num, line.Substring(0, Console.WindowWidth - 2)));
                _num++;
                recursiveLines(line.Substring(Console.WindowWidth - 2));
            }
            else
            {
                _lines.Add(new CLILine(_num, line));
                _num++;
            }
        }
    }
}