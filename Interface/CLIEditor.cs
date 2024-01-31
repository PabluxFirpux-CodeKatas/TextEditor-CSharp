using System.Security.Cryptography;
using Microsoft.VisualBasic;

namespace TextEditor.Interface
{
    class CLIEditor
    {
        Tables.Table _table;
        CLIScreen _screen;
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace, ConsoleKey.Enter };

        public CLIEditor(Tables.Table table)
        {
            _table = table;
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
            ReDraw();
            Console.SetCursorPosition(2, 0);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (handleKeyPress(key)) ReDraw();
            }
        }

        void ReDraw()
        {
            (int, int) pos = Console.GetCursorPosition();
            Console.Clear();
            _screen = new CLIScreen(_table.parseTable());
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
            if (specialCharacters.Contains(key.Key)) handleSpecialCharacter(key.Key);
            else addText(key.KeyChar.ToString());

            return !specialCharacters.Contains(key.Key);
        }

        void handleSpecialCharacter(ConsoleKey key)
        {
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
            handleCursor(ConsoleKey.RightArrow);
            ReDraw();
        }

        int getCursorIndex()
        {
            (int, int) pos = Console.GetCursorPosition();
            int index = 0;
            List<CLILine> lines = _screen.getLines();
            foreach (CLILine line in lines)
            {
                if (pos.Item2 == line.getNum() - 1) break;
                index += line.getLength();
            }
            index += pos.Item1 - 2;
            return index;
        }

        void handleCursor(ConsoleKey key)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            switch (key)
            {
                //Operadores ternarios para evitar OutOfRangeExceptions del cursor
                case ConsoleKey.UpArrow:
                    top -= (top < 1) ? 0 : 1;
                    break;
                case ConsoleKey.LeftArrow:
                    left -= (left < 3) ? 0 : 1;
                    break;
                case ConsoleKey.DownArrow:
                    top += (top >= Console.WindowHeight - 1) ? 0 : 1;
                    break;
                case ConsoleKey.RightArrow:
                    left += (left >= Console.WindowWidth - 1) ? 0 : 1;
                    break;
            }
            Console.SetCursorPosition(left, top);
        }
    }
}