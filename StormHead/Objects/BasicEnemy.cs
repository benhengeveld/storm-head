/*  Program: BasicEnemy.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: An enemy that attacks the player by touching them
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
using StormHead.Handlers;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StormHead.Objects
{
    public class BasicEnemy : Enemy
    {
        private GameScene scene;

        public BasicEnemy(GameScene scene, TextureAnimated texture, Vector2 position, Vector2 size, SoundEffect attackSound,
            float speed, float jumpHeight, float maxHealth, float damage, int damageCooldown, Vector2 gravity, int pointWorth)
            :base (scene, texture, position, size, attackSound, speed, jumpHeight, maxHealth, damage, damageCooldown, gravity, pointWorth)
        {
            this.scene = scene;
        }

        /// <summary>
        /// Trys to attack the player
        /// </summary>
        public override void TryToAttack()
        {
            //Gets the players position and hitbox rectangle
            Vector2 playerPos = scene.Player.Position + scene.Player.HitboxPos;
            Rectangle playerRec = new Rectangle(playerPos.ToPoint(), scene.Player.HitboxSize.ToPoint());
            //Get the enemies rectangle hitbox
            Rectangle enemyRec = new Rectangle(Position.ToPoint(), Size.ToPoint());

            //Check if the players hitbox hits the enemies
            if (enemyRec.Intersects(playerRec))
            {
                //Reset the attack cooldown
                CooldownCounter = 0;
                //Damage the player
                scene.Player.DamagePlayer(Damage);

                //If there is an attack sound, play the sound
                if (AttackSound != null)
                    AttackSound.Play();
            }
        }
    }
}
