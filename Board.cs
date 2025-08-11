using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Milestone.Board;

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

        public Cell GetCell(int row, int col)
        {
            return Cells[row, col];
        }

        private bool IsCellOnBoard(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size;
        }

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

        private int GetNumberOfBombNeighbors(int row, int col)
        {
            if (Cells[row, col].IsBomb)
            {
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

        private void SetupRewards()
        {
            int totalRewards = Size; // Example: number of rewards
            int rewardsPlaced = 0;

            while (rewardsPlaced < totalRewards)
            {
                int r = random.Next(Size);
                int c = random.Next(Size);

                if (Cells[r, c].HasSpecialReward == Cell.SpecialRewardType.None && !Cells[r, c].IsBomb)
                {
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

        public GameStatus DetermineGameState()
        {
            foreach (var cell in Cells)
            {
                if (cell.IsBomb && cell.IsVisited)
                    return GameStatus.Lost;
            }

            bool allClearCellsVisitedOrFlagged = true;
            foreach (var cell in Cells)
            {
                if (!cell.IsBomb && !cell.IsVisited && !cell.IsFlagged)
                    allClearCellsVisitedOrFlagged = false;
            }

            return allClearCellsVisitedOrFlagged ? GameStatus.Won : GameStatus.InProgress;
        }
    }
}

