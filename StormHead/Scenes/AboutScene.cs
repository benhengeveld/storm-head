/*  Program: AboutScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Shows who made the game
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
    public class AboutScene : Scene
    {
        //About text
        private string aboutText = "Game by: Ben Hengeveld";
        private SpriteFont aboutFont = FontHolder.gameOverFont;
        private Color aboutColor = Color.Black;

        //Exit text
        private string exitText = "Press Esc to return";
        private SpriteFont exitFont = FontHolder.infoFont;
        private Vector2 exitOffset = new Vector2(-15, 15);
        private Color exitColor = Color.Black;

        public AboutScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
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

            //Draw the about text
            Vector2 aboutPos = new Vector2(ScenesGame.screenSize.X / 2 - aboutFont.MeasureString(aboutText).X / 2, ScenesGame.screenSize.Y / 2 - aboutFont.LineSpacing / 2);
            SpriteBatch.DrawString(aboutFont, aboutText, aboutPos, aboutColor);

            //Draw the exit text
            Vector2 exitPos = new Vector2(ScenesGame.screenSize.X - FontHolder.infoFont.MeasureString(exitText).X, 0) + exitOffset;
            SpriteBatch.DrawString(exitFont, exitText, exitPos, exitColor);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
