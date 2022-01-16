/*  Program: HighScoresScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Shows the top 5 high-scores
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StormHead.Models;
using StormHead.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Scenes
{
    public class HighScoresScene : Scene
    {
        //High-scores
        private List<HighScore> highScores;
        private string highScoreText = "";
        private SpriteFont highScoreFont = FontHolder.regularFont;
        private Color highScoreColor = Color.Black;

        //Title
        private string titleText = "High-Scores";
        private SpriteFont titleFont = FontHolder.gameOverFont;
        private Vector2 titleOffset = new Vector2(0, 15);
        private Color titleColor = Color.Black;

        //Exit text
        private string exitText = "Press Esc to return";
        private SpriteFont exitFont = FontHolder.infoFont;
        private Vector2 exitOffset = new Vector2(-15, 15);
        private Color exitColor = Color.Black;

        public HighScoresScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            //Set the high-scores list to an empty list
            highScores = new List<HighScore>();
        }

        public override void Show()
        {
            //Set the high-score text to empty
            highScoreText = "";

            //Load the high-scores from a file
            highScores = HighScoreHandler.GetHighScores();
            //Loop through all the high-scores
            foreach (HighScore highScore in highScores)
            {
                //Add the current high-score to the high-score text
                highScoreText += $"{highScore.Name}: {highScore.Score}\n";
            }
            //Remove the extra new line at the end of the high-score text
            highScoreText = highScoreText.Trim('\n');

            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //If the user pressed the esc key
            if (ks.IsKeyDown(Keys.Escape))
            {
                //Hide all the scenes and then show the menu
                ScenesGame.HideAllScenes();
                ScenesGame.menuScene.Show();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            //Draw title
            Vector2 titlePos = new Vector2(ScenesGame.screenSize.X / 2 - titleFont.MeasureString(titleText).X / 2, 0) + titleOffset;
            SpriteBatch.DrawString(titleFont, titleText, titlePos, titleColor);

            //Draw esc to return to menu
            Vector2 exitPos = new Vector2(ScenesGame.screenSize.X - FontHolder.infoFont.MeasureString(exitText).X, 0) + exitOffset;
            SpriteBatch.DrawString(exitFont, exitText, exitPos, exitColor);

            //Draw High-scores
            Vector2 highScorePos = new Vector2(ScenesGame.screenSize.X / 2 - highScoreFont.MeasureString(highScoreText).X / 2,
                ScenesGame.screenSize.Y / 2 - highScoreFont.MeasureString(highScoreText).Y / 2);
            SpriteBatch.DrawString(highScoreFont, highScoreText, highScorePos, highScoreColor);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
