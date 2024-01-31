using TextEditor.Files;
using TextEditor.Tables;
using System;
using System.Runtime.InteropServices;

namespace TextEditor.app
{
    class Program
    {
        static ConsoleKey[] specialCharacters = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Escape, ConsoleKey.Backspace };
        static void Main()
        {
            /* Console.Clear();
            String path = "./test.txt";
            Files.File file = new Files.File(path);
            Table table = new Table(file.getFullText());
            table.addText("MECMECMEMCM", 2);
            table.addText("MECMECMEMCM", 53);
            table.deleteText(2);
            foreach (Cell c in table.getCells())
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("=============");
            Console.WriteLine(table.parseTable());
            while (true)
            {
                Console.Clear();
                Console.WriteLine(table.parseTable());
                Thread.Sleep(1000 / 60);
            } */
            start();

        }

        static void exitProgram(int code = 0)
        {
            Console.Clear();
            Console.TreatControlCAsInput = false;
            Environment.Exit(code);
        }

        static void start(String docName = "PabloWord")
        {
            Console.Title = $"{docName}";
            Console.Clear();
            Console.CursorVisible = true;
            Console.TreatControlCAsInput = true;
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                handleKeyPress(key);
            }
        }

        static void handleKeyPress(ConsoleKey key)
        {
            if (specialCharacters.Contains(key)) handleSpecialCharacter(key);
            else Console.Write(key);
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