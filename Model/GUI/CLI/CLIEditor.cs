using System.Text;
using System;
using TextEditor.Interfaces;
using TextEditor.Files;

namespace TextEditor.GUI.CLI
{
    class CLIEditor
    {
        IEditable _table;
        String _filename;
        Files.File _file;
        CLIScreen _screen;
        StringBuilder _buffer;
        Boolean _movedCursor;
        int _lastPos;
        int _heightOffset;
        int _separation;
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace, ConsoleKey.Enter };

        public CLIEditor(IEditable table, Files.File file, String filename)
        {
            _table = table;
            _file = file;
            _filename = filename;
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

        public void start()
        {
            Console.Title = $"{_filename}";
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
                textToDraw.Insert(index, _buffer.ToString());
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

        void handleKeyPress(ConsoleKeyInfo key)
        {
            Boolean specialChar = specialCharacters.Contains(key.Key);
            if (key.Modifiers.HasFlag(ConsoleModifiers.Control) && key.Key.Equals(ConsoleKey.S)) { saveFile(); return; }
            if (Char.IsControl(key.KeyChar) && !specialChar) return;
            if (specialChar) handleSpecialCharacter(key.Key);
            else
            {
                _buffer.Append(key.KeyChar);
                ReDraw();
                handleCursor(ConsoleKey.RightArrow);
                Console.Title = $"{_filename} (unsaved)";
            }
        }

        void pushBuffer()
        {
            addText(_buffer.ToString());
            _buffer.Clear();
            ReDraw();
            _movedCursor = true;
        }

        void handleSpecialCharacter(ConsoleKey key)
        {
            pushBuffer();
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
                    EnterCursor();
                    _movedCursor = true;
                    break;
            }
        }

        void deleteText(int count = 1)
        {
            if (Console.GetCursorPosition().Left == _separation)
            {
                int ind = getCursorIndex();
                handleCursor(ConsoleKey.UpArrow);
                cursorMaxLeft();
                _table.deleteText(ind);
                if (Environment.NewLine.Length > 1)
                    _table.deleteText(ind - 1);
            }
            else
            {
                _table.deleteText(getCursorIndex());
                handleCursor(ConsoleKey.LeftArrow);
            }
            _movedCursor = true;
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
            if (top < 0) { cursorMinTop(); scrollUp(); return; }
            if (top > _screen.getLines().Count - _heightOffset - 1 || top > Console.BufferHeight - 1) { cursorMaxTop(); return; }
            if (top > Console.BufferHeight - 4) { top = Console.BufferHeight - 4; scrollDown(); }
            if (left < _separation) { cursorMinLeft(); return; }
            if (left > _screen.getLines().ToArray().ElementAt(top + _heightOffset).getLength() + _separation - Environment.NewLine.Length) { cursorMaxLeft(); return; }

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

        void EnterCursor()
        {
            cursorMinLeft();
            handleCursor(ConsoleKey.DownArrow);
        }

        void cursorMinTop()
        {
            Console.SetCursorPosition(Console.CursorLeft, 0);
        }

        void cursorMaxTop()
        {
            int maxTop = Math.Min(Console.BufferHeight - 1, _screen.getLines().Count - _heightOffset - 1);
            Console.SetCursorPosition(Console.CursorLeft, maxTop);
        }

        void cursorMinLeft()
        {
            Console.SetCursorPosition(_separation, Console.CursorTop);
        }

        void cursorMaxLeft()
        {
            int top = Console.CursorTop;
            int maxLeft = _screen.getLines().ToArray().ElementAt(top + _heightOffset).getLength() + _separation - Environment.NewLine.Length;
            Console.SetCursorPosition(maxLeft, top);
        }

        void saveFile()
        {
            Console.Title = $"{_filename}";
            pushBuffer();
            _file.saveFile(_table.getText());
        }
    }
}