namespace TextEditor.Tables
{
    public enum FileType
    {
        ORIGINAL,
        MODIFICATION
    }
    class Cell
    {
        private FileType _ftype;
        private int _beginIndex;
        private int _length;

        public Cell(FileType ftype, int beginIndex, int length)
        {
            _ftype = ftype;
            _beginIndex = beginIndex;
            _length = length;
        }

        public int getIndex() { return _beginIndex; }
        public void setIndex(int i) { _beginIndex = i; }
        public FileType getFileType() { return _ftype; }
        public void setFileType(FileType f) { _ftype = f; }
        public int getLength() { return _length; }
        public void setLenght(int l) { _length = l; }

        public override String ToString()
        {
            return ((_ftype == FileType.ORIGINAL) ? "Original, " : "New, ") + "Index: " + _beginIndex + ", Length: " + _length + ".";
        }










    }
}