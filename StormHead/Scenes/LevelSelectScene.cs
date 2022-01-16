/*  Program: LevelSelectScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Lets the user pick from two levels to play
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
using StormHead.Handlers;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Scenes
{
    public class LevelSelectScene : Scene
    {
        //Menu component
        private Vector2 menuPos = new Vector2(200, 200);
        private MenuComponent levelMenu;
        private string[] menuItems = { "Level One", "Level Two" };
        private KeyboardState oldKeyState;
        private SpriteFont menuRegularFont = FontHolder.regularFont;
        private Color menuRegularColor = Color.Black;
        private SpriteFont menuHilightFont = FontHolder.hilightFont;
        private Color menuHilightColor = Color.Red;

        //Title
        private string titleText = "Select a Level";
        private SpriteFont titleFont = FontHolder.gameOverFont;
        private Vector2 titleOffset = new Vector2(0, 15);
        private Color titleColor = Color.Black;

        //Exit text
        private string exitText = "Press Esc to return";
        private SpriteFont exitFont = FontHolder.infoFont;
        private Vector2 exitOffset = new Vector2(-15, 15);
        private Color exitColor = Color.Black;

        //Levels scores
        private string scoreString;
        private SpriteFont scoreFont = FontHolder.pointsFont;
        private Vector2 scorePos = new Vector2(1000, 250);
        private Color scoreColor = Color.Black;

        public LevelSelectScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            //Make the menu component
            levelMenu = new MenuComponent(this, menuPos, menuRegularFont, menuRegularColor, menuHilightFont, menuHilightColor, menuItems);
            this.Components.Add(levelMenu);
        }

        public override void Show()
        {
            //Load all the levels scores
            ScenesGame.levelScoreHandler.LoadLevelScores();

            //Reset the menu index
            levelMenu.SelectedIndex = 0;
            string[] newMenuItems = new string[2];

            //If there is a score on level one
            if (ScenesGame.levelScoreHandler.LevelOneScore >= 0)
                //Set the new menu item to locked
                newMenuItems[0] = $"{menuItems[0]} - Locked";
            else
                //Set as normal
                newMenuItems[0] = $"{menuItems[0]}";

            //If there is a score on level two
            if (ScenesGame.levelScoreHandler.LevelTwoScore >= 0)
                //Set the new menu item to locked
                newMenuItems[1] = $"{menuItems[1]} - Locked";
            else
                //Set as normal
                newMenuItems[1] = $"{menuItems[1]}";

            //Set the new menu items
            levelMenu.MenuItems = newMenuItems;

            string levelOneScore = "";
            //If the score is not -1 set level one score
            if (ScenesGame.levelScoreHandler.LevelOneScore >= 0)
                levelOneScore = ScenesGame.levelScoreHandler.LevelOneScore.ToString();

            string levelTwoScore = "";
            //If the score is not -1 set level two score
            if (ScenesGame.levelScoreHandler.LevelTwoScore >= 0)
                levelTwoScore = ScenesGame.levelScoreHandler.LevelTwoScore.ToString();

            //Set the scores string
            scoreString = $"--| Scores |--\n" +
                $"Level One: {levelOneScore}\n" +
                $"Level Two: {levelTwoScore}";

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

            //Get the selected item from the menu component
            int selectedIndex = levelMenu.SelectedIndex;
            string selectedItem = levelMenu.GetSelectedItem();
            //If the user can select an menu item and the user is pressing enter
            if (canSelect && ks.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
            {
                //If the menu is enabled
                if (levelMenu.Enabled)
                {
                    //Get what menu item was selected
                    //Level one
                    if (selectedItem == menuItems[0] && ScenesGame.levelScoreHandler.LevelOneScore == -1)
                    {
                        //Hide all the scenes then show level one
                        ScenesGame.HideAllScenes();
                        ScenesGame.levelOneScene.Show();
                    }
                    //Level two
                    else if (selectedItem == menuItems[1] && ScenesGame.levelScoreHandler.LevelTwoScore == -1)
                    {
                        //Hide all the scenes then show level two
                        ScenesGame.HideAllScenes();
                        ScenesGame.levelTwoScene.Show();
                    }
                }
            }

            //Save the keystate as the old keystate
            oldKeyState = ks;
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

            //Draw levels scores
            SpriteBatch.DrawString(scoreFont, scoreString, scorePos, scoreColor);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
