using System.Collections;

namespace TextEditor.Tables
{
    class Table
    {
        private String _originalText;
        private String _modification;
        private ArrayList _cells;

        public Table()
        {
            _cells = new ArrayList();
        }

        public Table(String originalText)
        {
            _cells = new ArrayList();
            _originalText = originalText;
            _modification = "";
            _cells.Add(new Cell(FileType.ORIGINAL, 0, _originalText.Length));
        }

        public ArrayList getCells() { return _cells; }

        public String parseTable()
        {
            String text = "";
            foreach (Cell c in _cells)
            {
                text += (c.getFileType() == FileType.ORIGINAL ? _originalText : _modification).Substring(c.getIndex(), c.getLength());
            }
            return text;
        }

        public void addText(String text, int index)
        {
            Cell c = getCellAtIndex(index);
            Cell toAdd = new Cell(FileType.MODIFICATION, _modification.Length, text.Length);
            _modification += text;
            int cellIndex = _cells.IndexOf(c);
            int beginIndex = getGeneralIndex(c);
            _cells.Remove(c);
            int diff = index - beginIndex;
            Cell prev = new Cell(FileType.ORIGINAL, c.getIndex(), diff);
            Cell post = new Cell(FileType.ORIGINAL, c.getIndex() + diff, c.getLength() - diff);
            _cells.Insert(cellIndex++, prev);
            _cells.Insert(cellIndex++, toAdd);
            _cells.Insert(cellIndex++, post);
        }

        public void deleteText(int index)
        {
            Cell c = getCellAtIndex(index);
            if (index == c.getIndex() + c.getLength())
            {
                c.setLenght(c.getLength() - 1);
            }
            else
            {
                int cellIndex = _cells.IndexOf(c);
                int beginIndex = getGeneralIndex(c);
                int diff = index - beginIndex;
                Cell prev = new Cell(FileType.ORIGINAL, c.getIndex(), diff);
                Cell post = new Cell(FileType.ORIGINAL, c.getIndex() + diff, c.getLength() - diff);
                prev.setLenght(c.getLength() - 1);
                _cells.Remove(c);
                _cells.Insert(cellIndex++, prev);
                _cells.Insert(cellIndex++, post);

            }


        }

        private Cell getCellAtIndex(int i)
        {
            int x = 0;
            foreach (Cell c in _cells)
            {
                x += c.getLength();
                if (x >= i) return c;
            }
            return null;
        }

        private int getGeneralIndex(Cell c)
        {
            int x = 0;
            foreach (Cell cell in _cells)
            {
                if (cell.Equals(c)) return x;
                x += cell.getLength();
            }
            return -1;
        }
    }
}