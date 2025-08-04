namespace Milestone
{
    internal class Program
    {
        static void PrintAnswers(Board board)
        {
            for (int r = 0; r < board.Size; r++)
            {
                for (int c = 0; c < board.Size; c++)
                {
                    var cell = board.Cells[r, c];
                    if (cell.IsBomb)
                    {
                        Console.Write("* ");
                    }
                    else
                    {
                        Console.Write(cell.NumberOfBombNeighbors + " ");
                    }
                }
                Console.WriteLine(); // new line after each row
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(" Hello, welcome to Minesweeper");
            // size 10 and difficulty 0.1
            Board board = new Board(10, 0.1f);
            Console.WriteLine("Here is sthe answer key for the first board");
            PrintAnswers(board);

            // size 15 and difficulty 0.15
            board = new Board(15, 0.15f);
            Console.WriteLine("Here is the answer key for the second board");
            PrintAnswers(board);
        }
    }
}

