using static Milestone.Board;

namespace Milestone
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a 8x8 board with 10% bombs 
            Board board = new Board(8, 0.1f);

            Console.WriteLine("Here is the answer key (cheating for now):");
            PrintAnswers(board);

            bool victory = false;
            bool death = false;
            bool rewardAvailable = false;

            while (!victory && !death)
            {
                Console.WriteLine("\nCurrent board:");
                PrintBoard(board);

                Console.Write("Enter row: ");
                if (!int.TryParse(Console.ReadLine(), out int row) || row < 0 || row >= board.Size)
                {
                    Console.WriteLine("Invalid row. Try again.");
                    continue;
                }

                Console.Write("Enter column: ");
                if (!int.TryParse(Console.ReadLine(), out int col) || col < 0 || col >= board.Size)
                {
                    Console.WriteLine("Invalid column. Try again.");
                    continue;
                }

                Console.Write("Choose action (Visit / Flag / Use Reward): ");
                string action = Console.ReadLine().Trim().ToLower();

                var cell = board.GetCell(row, col);

                if (action == "flag")
                {
                    cell.IsFlagged = !cell.IsFlagged;
                    Console.WriteLine(cell.IsFlagged ? "Flag placed." : "Flag removed.");
                }
                else if (action == "visit")
                {
                    if (cell.IsFlagged)
                    {
                        Console.WriteLine("Cell is flagged. Unflag it before visiting.");
                        continue;
                    }
                    cell.IsVisited = true;

                    if (cell.IsBomb)
                    {
                        death = true;
                        break;
                    }
                    else if (cell.HasSpecialReward != Cell.SpecialRewardType.None)
                    {
                        rewardAvailable = true;
                        Console.WriteLine("🎁 You found a reward! Use it wisely.");
                    }
                }
                else if (action == "use reward")
                {
                    if (!rewardAvailable)
                    {
                        Console.WriteLine("No reward available to use.");
                        continue;
                    }

                    Console.Write("Enter row to peek: ");
                    if (!int.TryParse(Console.ReadLine(), out int peekRow) || peekRow < 0 || peekRow >= board.Size)
                    {
                        Console.WriteLine("Invalid row.");
                        continue;
                    }

                    Console.Write("Enter column to peek: ");
                    if (!int.TryParse(Console.ReadLine(), out int peekCol) || peekCol < 0 || peekCol >= board.Size)
                    {
                        Console.WriteLine("Invalid column.");
                        continue;
                    }

                    var peekCell = board.GetCell(peekRow, peekCol);
                    Console.WriteLine(peekCell.IsBomb ? "💣 That cell is a bomb!" : "✅ That cell is safe.");
                    rewardAvailable = false;
                }
                else
                {
                    Console.WriteLine("Invalid action.");
                    continue;
                }

                var gameState = board.DetermineGameState();
                if (gameState == Board.GameStatus.Won)
                    victory = true;
                else if (gameState == Board.GameStatus.Lost)
                    death = true;
            }

            Console.WriteLine(victory ? "🎉 Congratulations! You won!" : "💥 Boom! You lost.");
        }

        static void PrintAnswers(Board board)
        {
            for (int r = 0; r < board.Size; r++)
            {
                for (int c = 0; c < board.Size; c++)
                {
                    var cell = board.GetCell(r, c);
                    if (cell.IsBomb)
                        Console.Write("* ");
                    else
                        Console.Write(cell.NumberOfBombNeighbors + " ");
                }
                Console.WriteLine();
            }
        }

        static void PrintBoard(Board board)
        {
            for (int r = 0; r < board.Size; r++)
            {
                for (int c = 0; c < board.Size; c++)
                {
                    var cell = board.GetCell(r, c);
                    if (cell.IsFlagged)
                        Console.Write("F ");
                    else if (!cell.IsVisited)
                        Console.Write("? ");
                    else
                        Console.Write(cell.NumberOfBombNeighbors + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
