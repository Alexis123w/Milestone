using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone
{

    public class Board
    {
        public int Size { get; private set; }
        public float Difficulty { get; set; }
        public Cell[,] Cells { get; private set; }
        public int RewardsRemaining { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public enum GameStatus { InProgress, Won, Lost }

        private Random random = new Random();

        public Board(int size, float difficulty)
        {
            Size = size;
            Difficulty = difficulty;
            Cells = new Cell[size, size];
            RewardsRemaining = 0;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Cells[r, c] = new Cell(r, c);
                }
            }

            SetupBombs();
            SetupRewards();
            CalculateNumberOfBombNeighbors();

            StartTime = DateTime.Now;
        }
        // used when player selects a cell and chooses to play the reward
        public void UseSpecialBonus()
        {
            // Not implemented now
        }
        // use after game is over to calculate final score
        public int DetermineFinalScore() { return 0; }

        // helper function to determine if a cell is out of bounds
        private bool IsCellOnBoard(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size;
        }

        // Use during setup to calculate the number of bomb neighbors for each cell
        private void CalculateNumberOfBombNeighbors()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Cells[r, c].NumberOfBombNeighbors = GetNumberOfBombNeighbors(r, c);
                }
            }
        }

        // helper function to determine the number of bumb neighbors for a cell
        private int GetNumberOfBombNeighbors(int row, int col)
        {
            if (Cells[row, col].IsBomb)
            {
                // Convention: bomb cells have neighbor count = 9
                return 9;
            }

            int bombCount = 0;
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (IsCellOnBoard(r, c) && !(r == row && c == col) && Cells[r, c].IsBomb)
                    {
                        bombCount++;
                    }
                }
            }
            return bombCount;
        }

        // use during setup to place bombs on the board
        private void SetupBombs()
        {
            int totalCells = Size * Size;
            int bombsToPlace = (int)(totalCells * Difficulty);

            int bombsPlaced = 0;
            while (bombsPlaced < bombsToPlace)
            {
                int r = random.Next(Size);
                int c = random.Next(Size);

                if (!Cells[r, c].IsBomb)
                {
                    Cells[r, c].IsBomb = true;
                    bombsPlaced++;
                }
            }
        }

        // use during setup to place rewards on the board
        private void SetupRewards()
        {
            // Example: randomly assign some special rewards to a few cells
            int totalRewards = Size; // just an example number

            int rewardsPlaced = 0;
            while (rewardsPlaced < totalRewards)
            {
                int r = random.Next(Size);
                int c = random.Next(Size);

                if (Cells[r, c].HasSpecialReward == Cell.SpecialRewardType.None && !Cells[r, c].IsBomb)
                {
                    // Assign a random reward type except None
                    Array rewardValues = Enum.GetValues(typeof(Cell.SpecialRewardType));
                    Cell.SpecialRewardType randomReward = Cell.SpecialRewardType.None;

                    while (randomReward == Cell.SpecialRewardType.None)
                    {
                        randomReward = (Cell.SpecialRewardType)rewardValues.GetValue(random.Next(rewardValues.Length));
                    }

                    Cells[r, c].HasSpecialReward = randomReward;
                    rewardsPlaced++;
                }
            }
        }

        // use every turn to determine the current game state
        public GameStatus DetermineGameState()
        {
            return GameStatus.InProgress;
        }

    }
}

