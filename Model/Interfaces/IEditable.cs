namespace TextEditor.Interfaces
{
    public interface IEditable
    {
        public String getText();
        public void addText(int index, String text);
        public void deleteText(int index);
    }
}