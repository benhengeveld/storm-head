/*  Program: Sprite.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A simple sprite you draw at a position
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Models
{
    public class Sprite : DrawableGameComponent
    {
        private Scene scene;
        private SpriteBatch spriteBatch;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 size;
        private Color color = Color.White;

        public Vector2 Position { get => position; set => position = value; }

        public Sprite(Scene scene, Texture2D texture, Vector2 position, Vector2 size) : base(scene.ScenesGame)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;
            this.texture = texture;
            this.Position = position;
            this.size = size;
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            //Draws the sprite
            Rectangle rec = new Rectangle(Position.ToPoint(), size.ToPoint());
            spriteBatch.Draw(texture, rec, color);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
