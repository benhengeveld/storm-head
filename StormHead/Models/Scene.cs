/*  Program: Scene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A scene model that holds diffrent game scenes
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
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Models
{
    public abstract class Scene : DrawableGameComponent
    {
        private Game1 scenesGame;
        private SpriteBatch spriteBatch;
        private Song scenesMusic;
        protected bool canSelect = false;

        //A list of components to update and draw
        public List<GameComponent> Components { get; set; }
        
        public Game1 ScenesGame { get => scenesGame; set => scenesGame = value; }
        public SpriteBatch SpriteBatch { get => spriteBatch; set => spriteBatch = value; }

        public Scene(Game1 game, Song scenesMusic) : base(game)
        {
            this.scenesGame = game;
            this.spriteBatch = game._spriteBatch;
            this.Components = new List<GameComponent>();
            this.scenesMusic = scenesMusic;

            //Hide the scene when it is made
            Hide();
        }

        /// <summary>
        /// Makes the scene visable and plays the scenes music
        /// </summary>
        public virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;
            this.canSelect = false;

            //If the scene has music play the music on repeat
            if (scenesMusic != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(scenesMusic);
            }
        }

        /// <summary>
        /// Makes the scene invisable and stops the scenes music
        /// </summary>
        public virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
            this.canSelect = false;

            //If the scene has music stop it
            if (scenesMusic != null)
                MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //Loop through all the components
            foreach (GameComponent item in Components)
            {
                //If the item is enabled, update it
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            //Once the enter hey is lifted the user can select things
            if (ks.IsKeyUp(Keys.Enter))
                canSelect = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Loop through all the components
            foreach (GameComponent item in Components)
            {
                //If the item is drawable
                if (item is DrawableGameComponent)
                {
                    //Get the drawable item
                    DrawableGameComponent comp = (DrawableGameComponent)item;
                    //If the item is visable draw the item
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }

            base.Draw(gameTime);
        }
    }
}
