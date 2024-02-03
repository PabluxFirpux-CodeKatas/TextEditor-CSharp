using System.Text;
using TextEditor.Interfaces;

namespace TextEditor.Dev
{
    public class MockTable : IEditable
    {
        private StringBuilder _text;
        public MockTable()
        {
            _text = new StringBuilder();
        }

        public MockTable(String text)
        {
            _text = new StringBuilder(text);
        }

        public String getText()
        {
            return _text.ToString();
        }
        public void addText(int index, String text)
        {
            try
            {
                _text.Insert(index, text);
            }
            catch
            {
                _text.Append(text);
            }
        }
        public void deleteText(int index)
        {
            try
            {
                _text.Remove(index - 1, 1);
            }
            catch
            {
                _text.Remove(_text.Length - 1, 1);
            }
        }
    }
}