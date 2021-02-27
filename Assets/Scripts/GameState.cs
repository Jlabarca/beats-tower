using System;

namespace Jlabarca.BeatsTower
{
    [Serializable]
    public sealed class GameState : IGameState
    {
        public event Action<int> OnScoreChange;
        public event Action<int> OnCountChange;

        public float time;
        public float timeOffset = 1.66f;
        public int[,] towerHeightWarm = new int[3,3];
        public int[,] towerHeight = new int[3,3];

        private int _score = -1;

        public int Score
        {
            get => _score;
            set
            {
                if (_score == value)
                {
                    return;
                }

                _score = value;
                OnScoreChange?.Invoke(_score);
            }
        }

        private int _count = -1;

        public int Count
        {
            get => _count;
            set
            {
                if (_count == value)
                {
                    return;
                }

                _count = value;
                OnCountChange?.Invoke(_count);
            }
        }
    }
}
