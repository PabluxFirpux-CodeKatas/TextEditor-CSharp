using TextEditor.Files;
using TextEditor.Tables;

namespace TextEditor.app
{
    class Program
    {
        static void Main()
        {
            Console.Clear();
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
            }
        }
    }






}