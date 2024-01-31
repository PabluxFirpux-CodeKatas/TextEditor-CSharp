namespace TextEditor.Interface
{
    class CLIScreen
    {
        List<CLILine> _lines;
        public CLIScreen(String text)
        {
            _lines = new List<CLILine>();
            generate(text);
        }

        public List<CLILine> getLines() { return _lines; }

        void generate(String text)
        {
            String[] strings = text.Split(Environment.NewLine);
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].Length >= Console.WindowWidth - 2)
                {

                }
                else
                {
                    strings[i] += Environment.NewLine;
                    _lines.Add(new CLILine(i + 1, strings[i]));
                }

            }
        }
    }
}