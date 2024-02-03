using TextEditor.Dev;
using TextEditor.GUI.CLI;
using TextEditor.Interfaces;

namespace TextEditor.Factory
{
    public enum FileDataStructure
    {
        TABLE
    }

    public enum FileGUI
    {
        CLI
    }

    public static class EditorFactory
    {
        public static void OpenEditor(String path, FileGUI fileGUI, FileDataStructure dataStructure)
        {
            Files.File file = new Files.File(path);
            IEditable editable = getDataStructure(dataStructure, file);
            switch (fileGUI)
            {
                case FileGUI.CLI:
                    CLIEditor editor = new CLIEditor(editable, file);
                    editor.start();
                    break;
                default:
                    CLIEditor edit = new CLIEditor(editable, file);
                    edit.start();
                    break;
            }
        }

        private static IEditable getDataStructure(FileDataStructure dataStructure, Files.File file)
        {
            switch (dataStructure)
            {
                case FileDataStructure.TABLE:
                    return new Tables.Table(file.getFullText());
                default:
                    return new MockTable(file.getFullText());
            }
        }
    }
}