using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryWPF
{
    class Stage
    {

        public int _stageNumber;
        public int _boardSize;
        public int _winsInARow;
        public int _firstLevel;
        public int _lastLevel;
        public string _robotImage;


        public Stage(int stageNumber, int boardSize, int winsInARow, int firstLevel, int lastLevel,string robotImage)
        {

            _stageNumber = stageNumber;
            _boardSize = boardSize;
            _winsInARow = winsInARow;
            _firstLevel = firstLevel;
            _lastLevel = lastLevel;
            _robotImage = robotImage;
        }

        public int getStageNumber()
        {
            return _stageNumber;

        }
    }
}
