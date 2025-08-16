using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typer_Training
{
    [Serializable]
    public class HighScores
    {
        public List<Score> scores { get; set; }

        public HighScores()
        {
            scores = new List<Score>();
        }

        public void addScore(Score score) {
            scores.Add(score);
        }

        public List<Score> getSorted() {
            return scores.OrderByDescending(score => score.WPM).ToList();
        }
    }
}
