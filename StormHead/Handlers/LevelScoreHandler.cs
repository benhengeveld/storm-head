/*  Program: LevelScoreHandler.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to hold each levels score and to save and load the levels score from a file
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using StormHead.Forms;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace StormHead.Handlers
{
    public class LevelScoreHandler
    {
        private string fileName = "level-scores.txt";

        private int levelOneScore = -1;
        private int levelTwoScore = -1;

        public int LevelOneScore { get => levelOneScore; set => levelOneScore = value; }
        public int LevelTwoScore { get => levelTwoScore; set => levelTwoScore = value; }

        /// <summary>
        /// Loads the levels scores from the file
        /// </summary>
        public void LoadLevelScores()
        {
            //Load the file
            string fileData = FileHandler.LoadFile(fileName);
            //Split the files data with commas
            string[] scoreStrings = fileData.Split(',');

            //Try and parse the numbers
            try
            {
                levelOneScore = int.Parse(scoreStrings[0]);
                levelTwoScore = int.Parse(scoreStrings[1]);
            }
            catch (Exception)
            {
                levelOneScore = -1;
                levelTwoScore = -1;
            }

            //If both levels dont have a score of -1
            if (levelOneScore != -1 && levelTwoScore != -1)
            {
                //Get the total score and try to save it as a high-score
                int score = levelOneScore + levelTwoScore;
                //Check if the score is a high-score
                if (HighScoreHandler.CheckIfHighScore(score))
                {
                    //Show the form to save the new high-score
                    HighScoreForm highScoreForm = new HighScoreForm(score);
                    highScoreForm.Show();
                }

                //Set both scores to -1
                levelOneScore = -1;
                levelTwoScore = -1;
                //Save the new scores
                SaveLevelScores();
            }
        }
        
        /// <summary>
        /// Saves the levels scores to a file
        /// </summary>
        public void SaveLevelScores()
        {
            if (levelOneScore != -1 && levelTwoScore != -1)
            {
                //Get the total score and try to save it as a high-score
                int score = levelOneScore + levelTwoScore;
                //Check if the score is a high-score
                if (HighScoreHandler.CheckIfHighScore(score))
                {
                    //Show the form to save the new high-score
                    HighScoreForm highScoreForm = new HighScoreForm(score);
                    highScoreForm.Show();
                }

                //Set both scores to -1
                levelOneScore = -1;
                levelTwoScore = -1;
            }

            //Save the scores as a string to the file
            string saveString = $"{levelOneScore},{levelTwoScore}";
            FileHandler.SaveFile(fileName, saveString);
        }
    }
}
