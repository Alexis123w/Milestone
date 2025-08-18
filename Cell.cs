using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone
{
    public class Cell
    {
        public int Row { get; set; } = -1;
        public int Column { get; set; } = -1;
        public bool IsVisited { get; set; } = false;   // already exists
        public bool IsRevealed { get; set; } = false;  // add this for GUI/flood fill
        public bool IsBomb { get; set; } = false;
        public bool IsFlagged { get; set; } = false;
        public int NumberOfBombNeighbors { get; set; } = 0;

        // Example reward enum for type of special reward
        public enum SpecialRewardType
        {
            None,
            Hint,
            TimeFreeze,
            BombDefuseKit,
            BombSquad,
            Undo
        }

        public SpecialRewardType HasSpecialReward { get; set; } = SpecialRewardType.None;

        // Constructor
        public Cell(int row = -1, int column = -1)
        {
            Row = row;
            Column = column;
        }
    }
}

