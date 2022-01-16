/*  Program: Wall.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A wall that is user on levels for the player and enemys to collide with
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Objects
{
    public class Wall : CollidableObject
    {
        private GameScene scene;
        private SpriteBatch spriteBatch;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 size;

        public Wall(GameScene scene, Texture2D texture, Vector2 position, Vector2 size, bool canCollide = true) : base(scene.ScenesGame, canCollide)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;
            this.texture = texture;
            this.position = position;
            this.size = size;

            //Add this wall to the scenes collidable object list
            this.scene.CollidableObjects.Add(this);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            //Get the cameras offset from the scene
            Vector2 cameraOffset = scene.CameraPos - scene.ScenesGame.screenSize / 2;
            //Draw the wall on the screen with the cameras offset
            Rectangle rec = new Rectangle(position.ToPoint() - cameraOffset.ToPoint(), size.ToPoint());
            spriteBatch.Draw(texture, rec, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the rectangle hitbox of a wall
        /// </summary>
        /// <returns>A rectangle with the position and size of the current wall</returns>
        public override Rectangle GetHitbox()
        {
            Rectangle rec = new Rectangle(position.ToPoint(), size.ToPoint());
            return rec;
        }

        /// <summary>
        /// Gets the current position of the wall
        /// </summary>
        /// <returns>The position of the wall</returns>
        public override Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Gets the current size of the wall
        /// </summary>
        /// <returns>The size of the wall</returns>
        public override Vector2 GetSize()
        {
            return size;
        }
    }
}
