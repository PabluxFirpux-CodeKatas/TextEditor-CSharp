using System.Text;

namespace TextEditor.Interface
{
    class CLIEditor
    {
        Tables.Table _table;
        CLIScreen _screen;
        StringBuilder _buffer;
        Boolean _movedCursor;
        int _lastPos;
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace, ConsoleKey.Enter };

        public CLIEditor(Tables.Table table)
        {
            _table = table;
            _buffer = new StringBuilder();
            _movedCursor = false;
            _lastPos = 0;
        }
        void exitProgram(int code = 0)
        {
            Console.Clear();
            Console.TreatControlCAsInput = false;
            Environment.Exit(code);
        }

        public void start(String docName = "PabloWord")
        {
            Console.Title = $"{docName}";
            Console.CursorVisible = true;
            Console.TreatControlCAsInput = true;
            Console.Clear();
            _screen = new CLIScreen(_table.parseTable());
            print(_screen);
            Console.SetCursorPosition(2, 0);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                handleKeyPress(key);
                ReDraw();
            }
        }

        void ReDraw()
        {
            (int, int) pos = Console.GetCursorPosition();
            StringBuilder textToDraw = new StringBuilder(_table.parseTable());
            textToDraw.Insert(getCursorIndex(), _buffer.ToString());
            _screen = new CLIScreen(textToDraw.ToString());

            Console.Clear();
            print(_screen);


            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        void print(CLIScreen screen)
        {
            List<CLILine> lines = screen.getLines();
            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                if (i < lines.Count)
                {
                    Console.Write(lines[i].getNum().ToString() + " " + lines[i].getText());
                }
                else
                {
                    Console.WriteLine("~");
                }
            }
        }

        Boolean handleKeyPress(ConsoleKeyInfo key)
        {
            Boolean specialChar = specialCharacters.Contains(key.Key);
            if (specialChar) handleSpecialCharacter(key.Key);
            else
            {
                _buffer.Append(key.KeyChar);
                ReDraw();
                handleCursor(ConsoleKey.RightArrow);
            }

            return !specialChar;
        }

        void handleSpecialCharacter(ConsoleKey key)
        {
            addText(_buffer.ToString());
            _buffer.Clear();
            _movedCursor = true;
            switch (key)
            {
                case ConsoleKey.Escape:
                    exitProgram();
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    handleCursor(key);
                    break;
                case ConsoleKey.Backspace:
                    deleteText();
                    break;
                case ConsoleKey.Enter:
                    addText(Environment.NewLine);
                    handleCursor(ConsoleKey.DownArrow);
                    break;
            }
        }

        void deleteText()
        {
            _table.deleteText(getCursorIndex());
            handleCursor(ConsoleKey.LeftArrow);
            ReDraw();
        }

        void addText(String text)
        {
            _table.addText(text, getCursorIndex());
            ReDraw();
        }

        int getCursorIndex()
        {
            if (!_movedCursor) return _lastPos;
            (int, int) pos = Console.GetCursorPosition();
            int index = 0;
            List<CLILine> lines = _screen.getLines();
            foreach (CLILine line in lines)
            {
                if (pos.Item2 == line.getNum() - 1) break;
                index += line.getLength();
            }
            index += pos.Item1 - 2;
            _lastPos = index;
            _movedCursor = false;
            return index;
        }

        void handleCursor(ConsoleKey key)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    top -= 1;
                    break;
                case ConsoleKey.LeftArrow:
                    left -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    top += 1;
                    break;
                case ConsoleKey.RightArrow:
                    left += 1;
                    break;
            }
            if (top < 0) top = 0;
            if (top > _screen.getLines().Count - 1) top = _screen.getLines().Count - 1;
            if (left < 2) left = 2;
            if (left > _screen.getLines().ToArray().ElementAt(top).getLength()) left = _screen.getLines().ToArray().ElementAt(top).getLength();
            Console.SetCursorPosition(left, top);
        }
    }
}