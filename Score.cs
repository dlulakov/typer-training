using System;

namespace Typer_Training
{
    [Serializable]
    public class Score
    {
        public string Name { get; set; }
        public int WPM { get; set; }
        public Type ScoreType { get; set; } // new enum property

        public Score(string name, int wpm, Type scoreType)
        {
            Name = name;
            WPM = wpm;
            ScoreType = scoreType;
        }
    }

    public enum Type
    {
        Words,
        Time,
        Quote,
        Zen,
    }
}
