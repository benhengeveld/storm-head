/*  Program: HowToPlayScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Shows the user the controls and how to play
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
    public class HowToPlayScene : Scene
    {
        //Exit text
        private string exitText = "Press Esc to return";
        private SpriteFont exitFont = FontHolder.infoFont;
        private Vector2 exitOffset = new Vector2(-15, 15);
        private Color exitColor = Color.Black;

        public HowToPlayScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            //Set the background to the image of how to play
            Sprite background = new Sprite(this, TextureHolder.howToPlayTexture, Vector2.Zero, ScenesGame.screenSize);
            this.Components.Add(background);
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

            //Draw the exit text
            Vector2 exitPos = new Vector2(ScenesGame.screenSize.X - FontHolder.infoFont.MeasureString(exitText).X, 0) + exitOffset;
            SpriteBatch.DrawString(exitFont, exitText, exitPos, exitColor);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
