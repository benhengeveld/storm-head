/*  Program: TextureHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to save and load high-scores to a file
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormHead.Handlers
{
    public class HighScoreHandler
    {
        private static int maxHighScores = 5;
        private static string fileName = "high-scores.txt";

        /// <summary>
        /// Gets the high-scores from the file
        /// </summary>
        /// <returns>A list of the high-scores sorted by descending</returns>
        public static List<HighScore> GetHighScores()
        {
            List<HighScore> highScores = new List<HighScore>();

            //Get the high-scores from the file
            string fileData = FileHandler.LoadFile(fileName);
            //Split the files data by the commas
            string[] highScoreStrings = fileData.Split('\n');
            //Loop through all the high-scores
            for (int i = 0; i < highScoreStrings.Length; i++)
            {
                string[] highScoreData = highScoreStrings[i].Split(',');
                try
                {
                    //Try and parse the high-score and add it to the high-score list
                    int score = int.Parse(highScoreData[0]);
                    string name = highScoreData[1];
                    highScores.Add(new HighScore(name, score));
                }
                catch (Exception) { }
            }

            //Sort the high-scores by descending
            highScores = highScores.OrderByDescending(i => i.Score).ToList();
            return highScores;
        }

        /// <summary>
        /// Saves a list of ints as high-scores to a file
        /// </summary>
        /// <param name="highScores">A list of high-scores</param>
        public static void SaveHighScores(List<HighScore> highScores)
        {
            //Sort the high-scores by descending
            highScores = highScores.OrderByDescending(i => i.Score).ToList();

            string saveString = "";
            //Loop through all the high-scores in the list
            foreach (HighScore highScore in highScores)
            {
                //Add the current high-score to the save string with a comma after it
                saveString += $"{highScore.Score.ToString()},{highScore.Name}\n";
            }
            //Remove the comma at the end of the save string
            saveString = saveString.Trim('\n');

            //Try and save the file
            try
            {
                FileHandler.SaveFile(fileName, saveString);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// See if a score is a new high-score, and add it and save it if so
        /// </summary>
        /// <param name="score">The score to try and add</param>
        public static void TryToSaveScore(HighScore highScore)
        {
            //Get the list of high-scores
            List<HighScore> highScores = GetHighScores();

            //Add the given score to the list
            highScores.Add(highScore);

            //Sort the high-scores by descending
            highScores = highScores.OrderByDescending(i => i.Score).ToList();

            //While there are to many high-scores in the list
            while (highScores.Count > maxHighScores)
            {
                //Remove the one at the end
                highScores.RemoveAt(highScores.Count - 1);
            }

            //Save the list of high-scores
            SaveHighScores(highScores);
        }

        /// <summary>
        /// Checks if a given high-score is a new high-score
        /// </summary>
        /// <param name="score">The score to check</param>
        /// <returns>If the score is a new high-score</returns>
        public static bool CheckIfHighScore(int score)
        {
            //Score must be above 0
            if (score <= 0)
                return false;

            //Get the list of high-scores
            List<HighScore> highScores = GetHighScores();

            //If there is any empty spots then its a new high-score
            if (highScores.Count < maxHighScores)
                return true;

            //Loop through all the high-scores
            foreach (HighScore highScore in highScores)
            {
                //If the given score is higher then one of the old high-scores then its a new high-score
                if (score > highScore.Score)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
