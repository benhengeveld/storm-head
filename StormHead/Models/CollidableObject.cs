/*  Program: Game1.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: The base model for collidable objects like walls
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
    public abstract class CollidableObject : DrawableGameComponent
    {
        private bool canCollide;

        public bool CanCollide { get => canCollide; set => canCollide = value; }
        

        public CollidableObject(Game1 game, bool canCollide) : base(game)
        {
            this.canCollide = canCollide;
        }

        public abstract Vector2 GetPosition();
        public abstract Vector2 GetSize();
        public abstract Rectangle GetHitbox();
    }
}
