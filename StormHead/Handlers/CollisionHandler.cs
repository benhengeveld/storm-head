/*  Program: TextureHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to check for collition between objects
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

namespace StormHead.Handlers
{
    public class CollisionHandler
    {
        private GameScene gameScene;

        public CollisionHandler(GameScene gameScene)
        {
            this.gameScene = gameScene;
        }

        /// <summary>
        /// Checks if the given rectangle collides with any collidable object in the scene
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public CollidableObject CheckForCollision(Rectangle rectangle)
        {
            //Loop through all the collidable objects in the scene
            foreach (CollidableObject collidableObject in gameScene.CollidableObjects)
            {
                //If the collidable objects hitbox intersects the given rectangle
                if (collidableObject.GetHitbox().Intersects(rectangle))
                {
                    //Return the collidable object the given rectangle hit
                    return collidableObject;
                }
            }

            return null;
        }
    }
}
