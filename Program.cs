using TextEditor.GUI.CLI;
using TextEditor.Tables;
using TextEditor.Dev;
using TextEditor.Factory;

namespace TextEditor.app
{
    class Program
    {
        static void Main()
        {
            String path = "D:/AAUni/Personal/C#/TextEditor-CSharp/test.txt";
            EditorFactory.OpenEditor(path, FileGUI.CLI, FileDataStructure.TABLE);
        }
    }
}