namespace TextEditor.Interface
{
    class CLILine
    {
        int _num;
        int _length;
        String _text;
        public CLILine(int num, String text)
        {
            _num = num;
            _text = text;
            _length = _text.Length;
        }

        public int getNum() { return _num; }
        public int getLength() { return _length; }
        public String getText() { return _text; }
    }
}