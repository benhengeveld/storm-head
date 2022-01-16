using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Models
{
    public class HighScore
    {
        private string name;
        private int score;

        public string Name { get => name; set => name = value; }
        public int Score { get => score; set => score = value; }

        public HighScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
