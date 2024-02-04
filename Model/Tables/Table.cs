using System.Collections;
using System.Linq;
using TextEditor.Interfaces;

namespace TextEditor.Tables
{
    class Table : IEditable
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

        public String getText()
        {
            String text = "";
            foreach (Cell c in _cells)
            {
                text += (c.getFileType() == FileType.ORIGINAL ? _originalText : _modification).Substring(c.getIndex(), c.getLength());
            }
            return text;
        }

        public void addText(int index, String text)
        {
            Cell c = getCellAtIndex(index);
            Cell toAdd = new Cell(FileType.MODIFICATION, _modification.Length, text.Length);
            _modification += text;
            if (c == null)
            {
                _cells.Insert(_cells.Count, toAdd);
                return;
            }
            int cellIndex = _cells.IndexOf(c);
            int beginIndex = getGeneralIndex(c);
            _cells.Remove(c);
            int diff = index - beginIndex;
            Cell prev = new Cell(c.getFileType(), c.getIndex(), diff);
            Cell post = new Cell(c.getFileType(), c.getIndex() + diff, c.getLength() - diff);
            _cells.Insert(cellIndex++, prev);
            _cells.Insert(cellIndex++, toAdd);
            _cells.Insert(cellIndex++, post);
            updateTable();
        }

        public void deleteText(int index)
        {
            Cell c = getCellAtIndex(index);
            if (c == null)
            {
                c = (Cell)_cells[_cells.Count - 1];
                c.setLenght(c.getLength() - 1);
            }
            else if (index == c.getIndex() + c.getLength())
            {
                c.setLenght(c.getLength() - 1);
            }
            else
            {
                int cellIndex = _cells.IndexOf(c);
                int beginIndex = getGeneralIndex(c);
                int diff = index - beginIndex;
                Cell prev = new Cell(c.getFileType(), c.getIndex(), diff - 1);
                Cell post = new Cell(c.getFileType(), c.getIndex() + diff, c.getLength() - diff);
                // prev.setLenght(prev.getLength() - 1);
                _cells.Remove(c);
                _cells.Insert(cellIndex++, prev);
                _cells.Insert(cellIndex++, post);

            }
            updateTable();
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

        private void updateTable()
        {
            ArrayList cells = (ArrayList)getCells().Clone();
            foreach (Cell c in cells)
            {
                if (c.getLength() <= 0) _cells.Remove(c);
            }
        }
    }
}