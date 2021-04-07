using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score;
        private int _highScore;

        public int GetHighScore() => _highScore;

        public void SetHighScore(int highScore) => _highScore = highScore;

        public bool IsNewHighScore() => _score > _highScore;

        public int GetScore() => _score;

        public void SetScore(int score) => _score = score;

        public void AddScore(int score) => _score += score;

        public void ScoreClearedRowsAndColumns(int noOfClearedRowsAndColumns)
        {
            switch (noOfClearedRowsAndColumns)
            {
                case 1:
                    _score += 10;
                    break;
                case 2:
                    _score += 30;
                    break;
                case 3:
                    _score += 60;
                    break;
                case 4:
                    _score += 100;
                    break;
                case 5:
                    _score += 150;
                    break;
                case 6:
                    _score += 210;
                    break;
                case 7:
                    _score += 280;
                    break;
                case 8:
                    _score += 360;
                    break;
                case 9:
                    _score += 450;
                    break;
                case 10:
                    _score += 550;
                    break;
                default:
                    _score += 0;
                    break;
            }
        }
    }
}