using System.Text;
using TextEditor.Interfaces;

namespace TextEditor.GUI.CLI
{
    class CLIEditor
    {
        IEditable _table;
        CLIScreen _screen;
        StringBuilder _buffer;
        Boolean _movedCursor;
        int _lastPos;
        int _heightOffset;
        int _separation;
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace, ConsoleKey.Enter };

        public CLIEditor(IEditable table)
        {
            _table = table;
            _buffer = new StringBuilder();
            _movedCursor = false;
            _lastPos = 0;
            _heightOffset = 0;
            _separation = 2;
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
            _screen = new CLIScreen(_table.getText());
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
            StringBuilder textToDraw = new StringBuilder(_table.getText());
            int index = getCursorIndex();
            if (index > textToDraw.Length)
            {
                textToDraw.Append(_buffer.ToString());
            }
            else
            {
                textToDraw.Insert(getCursorIndex(), _buffer.ToString());
            }
            _screen = new CLIScreen(textToDraw.ToString());

            Console.Clear();
            print(_screen);


            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        void print(CLIScreen screen)
        {
            List<CLILine> lines = screen.getLines();
            int maxInd = lines.Count() + 1;
            _separation = maxInd.ToString().Length + 1;
            for (int i = _heightOffset; i < _heightOffset + Console.BufferHeight - 1; i++)
            {
                if (i < lines.Count)
                {
                    String ind = lines[i].getNum().ToString();
                    Console.Write(ind);
                    for (int j = 0; j < _separation - ind.Length; j++) Console.Write(" ");
                    Console.Write(lines[i].getText());
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
                    handleCursor(ConsoleKey.LeftArrow);
                    handleCursor(ConsoleKey.LeftArrow);
                    _movedCursor = true;
                    break;
            }
        }

        void deleteText(int count = 1)
        {
            if (Console.GetCursorPosition().Left == _separation)
            {
                int ind = getCursorIndex();
                _table.deleteText(ind);
                _table.deleteText(ind - 1);
                handleCursor(ConsoleKey.LeftArrow);
                handleCursor(ConsoleKey.UpArrow);
            }
            else
            {
                _table.deleteText(getCursorIndex());
                handleCursor(ConsoleKey.LeftArrow);
            }
            ReDraw();
        }

        void addText(String text)
        {
            _table.addText(getCursorIndex(), text);
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
                if (pos.Item2 + _heightOffset == line.getNum() - 1)
                {
                    index += pos.Item1 - _separation;
                    _lastPos = index;
                    _movedCursor = false;
                    return index;
                }
                index += line.getLength();
            }
            index += pos.Item1 - _separation;
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
            if (top < 0) { top = 0; scrollUp(); }
            if (top > _screen.getLines().Count - _heightOffset - 1 || top >= Console.BufferHeight - 1) { top = Math.Min(Console.BufferHeight - 1, _screen.getLines().Count - _heightOffset - 1); }
            if (top > Console.BufferHeight - 4) { top = Console.BufferHeight - 4; scrollDown(); }
            if (left < _separation) left = _separation;
            if (left > _screen.getLines().ToArray().ElementAt(top + _heightOffset).getLength() + _separation - 2) left = _screen.getLines().ToArray().ElementAt(top + _heightOffset).getLength() + _separation - 2;

            Console.SetCursorPosition(left, top);
        }

        void scrollUp()
        {
            if (_heightOffset < 1) return;
            _heightOffset -= 1;
            ReDraw();
        }

        void scrollDown()
        {
            _heightOffset += 1;
            ReDraw();
        }
    }
}