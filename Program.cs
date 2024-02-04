using TextEditor.GUI.CLI;
using TextEditor.Tables;
using TextEditor.Dev;
using TextEditor.Factory;

namespace TextEditor.app
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                printHelp();
                return;
            }
            String path = args[0];
            EditorFactory.OpenEditor(path, FileGUI.CLI, FileDataStructure.TABLE);
        }

        static void printHelp()
        {
            Console.WriteLine("First Argument should be the name of the file to be edited or created");
        }
    }
}