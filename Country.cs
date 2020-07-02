using System;
using System.Collections.Generic;
using System.Linq;

namespace AssessmentTask
{
    public class Country
    {
        public string CountryName { get; }
        public double AverageScore {
            get
            {
                return Math.Round(Scores.Average(), 2);
            }
        }
        private double MedianScore
        {
            get
            {
                this.Scores.Sort();
                int count = Scores.Count();
                if(count % 2 == 0)
                {
                    return (Scores[count / 2] + Scores[count / 2 - 1]) / 2;
                }
                else
                {
                    return Scores[count / 2];
                }
            }
        }
        private int MaxScore { get; set; }
        private string MaxScorePerson { get; set; }
        private int MinScore { get; set; }
        private string MinScorePerson { get; set; }
        private int RecordCount { get; set; }

        private List<int> Scores = new List<int>();

        public Country(string countryName, string firstName, string lastName, int score)
        {
            this.CountryName = countryName;
            this.MaxScore = score;
            this.MaxScorePerson = $"{firstName} {lastName}";
            this.MinScore = score;
            this.MinScorePerson = $"{firstName} {lastName}";
            this.RecordCount = 1;
            this.Scores.Add(score);
        }

        public void AddScore(string firstName, string lastName, int score)
        {
            this.Scores.Add(score);
            if(this.MaxScore < score)
            {
                this.MaxScore = score;
                this.MaxScorePerson = $"{firstName} {lastName}";
            }
            if(this.MinScore > score)
            {
                this.MinScore = score;
                this.MinScorePerson = $"{firstName} {lastName}";
            }
            this.RecordCount++;
        }

        public string GetCountryInCsv()
        {
            return $"{CountryName};{AverageScore.ToString()};{MedianScore.ToString()};{MaxScore.ToString()};{MaxScorePerson};{MinScore.ToString()};{MinScorePerson};{RecordCount.ToString()}";
        }
    }
}
