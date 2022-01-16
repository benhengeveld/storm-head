/*  Program: LaserEnemy.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: An enemy that attacks the player with a laser
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

namespace StormHead.Objects
{
    public class LaserEnemy : Enemy
    {
        private GameScene scene;

        //Laser
        private Color laserColor;
        private Vector2 laserSize;
        private int laserCooldown;
        private int laserCounter = 0;
        private bool showLaser = false;

        public LaserEnemy(GameScene scene, TextureAnimated texture, Vector2 position, Vector2 size, SoundEffect attackSound,
            float speed, float jumpHeight, float maxHealth, float damage, int damageCooldown, Vector2 gravity, int pointWorth,
            Color laserColor, Vector2 laserSize, int laserCooldown)
            : base(scene, texture, position, size, attackSound, speed, jumpHeight, maxHealth, damage, damageCooldown, gravity, pointWorth)
        {
            this.scene = scene;
            this.laserColor = laserColor;
            this.laserSize = laserSize;
            this.laserCooldown = laserCooldown;
        }

        /// <summary>
        /// Trys to attack the player
        /// </summary>
        public override void TryToAttack()
        {
            //Get the players position and hitbox rectangle
            Vector2 playerPos = scene.Player.Position + scene.Player.HitboxPos;
            Rectangle playerRec = new Rectangle(playerPos.ToPoint(), scene.Player.HitboxSize.ToPoint());

            //If the cooldown is done
            if (laserCounter >= laserCooldown)
            {
                //reset the cooldown
                CooldownCounter = 0;
                laserCounter = 0;
                //Set the laser to be shown
                showLaser = true;

                //Get the position of the laser
                Vector2 laserPos = Position + Size / 2;
                if (spriteEffect == SpriteEffects.None)
                    laserPos.X -= laserSize.X;

                //Get the rectangle of the laser
                Rectangle laserRec = new Rectangle(laserPos.ToPoint(), laserSize.ToPoint());
                //Check if the laser hits the player
                if (laserRec.Intersects(playerRec))
                {
                    //Damage the player
                    scene.Player.DamagePlayer(Damage);
                }

                //If there is an attack sound, play the sound
                if (AttackSound != null)
                    AttackSound.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //If the cooldown is not done yet, add one to the cooldown
            if (laserCounter < laserCooldown)
                laserCounter++;

            //If the cooldown is done dont show the laser
            if (laserCounter >= laserCooldown)
                showLaser = false;

                base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            //Get the scenes camera draw offset
            Vector2 cameraOffset = scene.CameraPos - scene.ScenesGame.screenSize / 2;

            //If the laser is visable
            if (showLaser)
            {
                //Get the lasers position
                Vector2 laserPos = Position + Size / 2;
                //If the enemy is facing left, offset the lasers position
                if (spriteEffect == SpriteEffects.None)
                    laserPos.X -= laserSize.X;

                //Draw the laser
                Rectangle laserDrawRec = new Rectangle(laserPos.ToPoint() - cameraOffset.ToPoint(), laserSize.ToPoint());
                spriteBatch.Draw(TextureHolder.pixelTexture, laserDrawRec, laserColor);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
