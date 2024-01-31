namespace TextEditor.Interface
{
    class CLIEditor
    {
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace };
        static void exitProgram(int code = 0)
        {
            Console.Clear();
            Console.TreatControlCAsInput = false;
            Environment.Exit(code);
        }

        public static void start(String docName = "PabloWord")
        {
            Console.Title = $"{docName}";
            Console.Clear();
            Console.CursorVisible = true;
            Console.TreatControlCAsInput = true;
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                handleKeyPress(key);
            }
        }

        static void handleKeyPress(ConsoleKeyInfo key)
        {
            if (specialCharacters.Contains(key.Key)) handleSpecialCharacter(key.Key);
            else Console.Write(key.KeyChar);
        }

        static void handleSpecialCharacter(ConsoleKey key)
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
            }
        }

        static void deleteText()
        {
            //TODO: Optimize delete
            handleCursor(ConsoleKey.LeftArrow);
            Console.Write(" ");
            handleCursor(ConsoleKey.LeftArrow);
        }

        static void handleCursor(ConsoleKey key)
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
                    left -= (left < 1) ? 0 : 1;
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