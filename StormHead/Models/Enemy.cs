/*  Program: Game1.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: The base model used for enemys
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
using StormHead.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Models
{
    public class Enemy : DrawableGameComponent
    {
        private GameScene scene;
        protected SpriteBatch spriteBatch;
        protected SpriteEffects spriteEffect = SpriteEffects.None;

        private TextureAnimated texture;
        private SoundEffect attackSound;

        private Vector2 position;
        private Vector2 size;
        private float speed;
        private float jumpHeight;
        private int jumpDistance = 10;

        private float maxHealth;
        private float health;
        private HealthBar healthBar;
        private bool canBeDamaged = true;
        private float damage;
        private int damageCooldown;
        private int cooldownCounter = 0;
        private int pointWorth = 100;

        private Vector2 velocity = Vector2.Zero;
        private Vector2 gravity;
        private bool useGravity = true;
        private bool stopMovement = false;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public float Health { get => health; set => health = value; }
        public bool CanBeDamaged { get => canBeDamaged; set => canBeDamaged = value; }
        public SoundEffect AttackSound { get => attackSound; set => attackSound = value; }
        public int CooldownCounter { get => cooldownCounter; set => cooldownCounter = value; }
        public float Damage { get => damage; set => damage = value; }

        public Enemy(GameScene scene, TextureAnimated texture, Vector2 position, Vector2 size, SoundEffect attackSound,
            float speed, float jumpHeight, float maxHealth, float damage, int damageCooldown, Vector2 gravity, int pointWorth) 
            : base(scene.ScenesGame)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;

            this.texture = texture;
            this.position = position;
            this.size = size;
            this.attackSound = attackSound;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.damage = damage;
            this.damageCooldown = damageCooldown;
            this.gravity = gravity;
            this.pointWorth = pointWorth;

            //Make the health bar for the enemy
            Vector2 healthBarPos = position;
            Vector2 healthBarSize = new Vector2(size.X, size.X / 4.5f);
            healthBar = new HealthBar(scene, healthBarPos, healthBarSize, TextureHolder.pixelTexture);
            healthBar.BarOffset = new Vector2(2, 2);

            //Add the enemy to the scenes enemies list
            scene.Enemies.Add(this);
        }

        /// <summary>
        /// Remove the enemy from view
        /// </summary>
        public void RemoveEnemy()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        /// <summary>
        /// Check if the enemy is on the floor
        /// </summary>
        /// <returns>True if the enemy is on the floor</returns>
        public bool IsEnemyOnFloor()
        {
            //Get the hitbox of the enemy moved down one
            CollidableObject collisionObject = 
                scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint() + new Point(0, 1),
                size.ToPoint()));
            //If the hitbox hit something then the enemy is on the floor
            if (collisionObject != null)
                return true;

            return false;
        }

        /// <summary>
        /// Check if there is a wall in front of then enemies direction
        /// </summary>
        /// <returns>True if there is a wall in front of the enemy</returns>
        public bool IsWallInFront()
        {
            CollidableObject collisionObject = null;
            //If the enemy is facing the left
            if (spriteEffect == SpriteEffects.None)
            {
                //Get the hitbox of the enemy moved one to the left
                collisionObject =
                    scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint() + new Point(-jumpDistance, 0),
                    size.ToPoint()));
            }
            //If the enemy is facing the right
            else
            {
                //Get the hitbox of the enemy moved one to the right
                collisionObject =
                    scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint() + new Point(jumpDistance, 0),
                    size.ToPoint()));
            }

            //If the hitbox hit something then the enemy has a wall in front of it
            if (collisionObject != null)
                return true;

            return false;
        }

        /// <summary>
        /// Damage a enemy by a given amount, and if the enemy remove it and add the point amount to the players score
        /// </summary>
        /// <param name="damage">The amount of damage</param>
        public void DamageEnemy(float damage)
        {
            //If the enemy not be damaged return
            if (!canBeDamaged)
                return;

            //If the damage wont kill the enemy
            if (health - damage > 0)
            {
                //Remove the damage amount from the enemies health
                health -= damage;
            }
            //The enemy will die
            else
            {
                //Set the enemies health to 0 and make it so it cant be damaged
                health = 0;
                canBeDamaged = false;
                //Add the points to the players score
                scene.PlayersScore += pointWorth;
                //Remove the enemy from view
                RemoveEnemy();
            }
        }

        /// <summary>
        /// Moves the enemy based on the enemies velocity and gravity
        /// </summary>
        /// <param name="gameTime">The games time used for gravity</param>
        public void MoveEnemy(GameTime gameTime)
        {
            //Save the enemies old position
            Vector2 oldPos = position;
            //Move the enemies x with the enemies velocity and speed
            position.X += velocity.X * speed;
            //Get the hitbox in the enemies new position
            CollidableObject collisionObject = 
                scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint(),
                size.ToPoint()));
            //Check if the new position collides with something
            if (collisionObject != null)
            {
                //If the enemy if closer to the left side of the object that was hit, put the enemy to the left of the object hit
                if (position.X + size.X / 2 < collisionObject.GetPosition().X + collisionObject.GetSize().X / 2)
                {
                    position.X = collisionObject.GetPosition().X - size.X;
                }
                //If the enemy if closer to the right side of the object that was hit, put the enemy to the right of the object hit
                else
                {
                    position.X = collisionObject.GetPosition().X + collisionObject.GetSize().X;
                }

                //If the enemy is still colliding, then set hit x pos back to what it was at the start
                collisionObject = scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint(), size.ToPoint()));
                if (collisionObject != null)
                {
                    position.X = oldPos.X;
                }
            }

            //If the enemies gravity is on
            if (useGravity)
            {
                //Save the enemies old position
                oldPos = position;
                //Move the enemies y with the enemies velocity and speed
                position.Y += velocity.Y * speed;
                //Get the hitbox in the enemies new position
                collisionObject = scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint(), size.ToPoint()));
                //Check if the new position collides with something
                if (collisionObject != null)
                {
                    //If the enemy is moving down put the player on the top of the object hit
                    if (velocity.Y > 0)
                    {
                        position.Y = collisionObject.GetPosition().Y - size.Y;
                    }
                    //If the enemy is moving up put the player on the bottom of the object hit
                    else
                    {
                        position.Y = collisionObject.GetPosition().Y + collisionObject.GetSize().Y;
                    }

                    //If the enemy is still colliding, then set hit x pos back to what it was at the start
                    collisionObject = scene.CollisionHandler.CheckForCollision(new Rectangle(position.ToPoint(), size.ToPoint()));
                    if (collisionObject != null)
                    {
                        position.Y = oldPos.Y;
                    }

                    //Reset the y velocity
                    velocity.Y = 0;
                }
                //Add the gravity to the velocity
                velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Lets the enemies have diffrent attacks
        /// </summary>
        public virtual void TryToAttack() { }

        public override void Update(GameTime gameTime)
        {
            //If the cooldown counter is not dont add one to the counter
            if (cooldownCounter < damageCooldown)
                cooldownCounter++;

            //If the cooldown counter is done and the player can be damaged
            if (cooldownCounter >= damageCooldown && scene.Player.CanBeDamaged)
            {
                //Try to attack the player
                TryToAttack();
            }

            //Get the players position and the enemies position
            Vector2 playerPos = scene.Player.Position + scene.Player.HitboxPos + scene.Player.HitboxSize / 2;
            Vector2 enemyPos = position + size / 2;

            //Reset the x velocity
            velocity.X = 0;

            //Get the x distance between the enemy and the player
            float xDistance = Math.Abs(playerPos.X - enemyPos.X);

            //if the enemy can move and the x distance is less then the enemy would move
            if (!stopMovement && xDistance > speed)
            {
                //If the player is to the left of the enemy
                if (playerPos.X < enemyPos.X)
                {
                    //Set the velocity and effect to the left
                    velocity.X = -1;
                    spriteEffect = SpriteEffects.None;
                }
                //If the player is to the right of the enemy
                else
                {
                    //Set the velocity and effect to the right
                    velocity.X = 1;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }

                //If the enemy is on the floor and there is a wall in front of it
                if (IsEnemyOnFloor() && IsWallInFront())
                {
                    //Make the enemy jump
                    velocity.Y = -jumpHeight;
                }
            }

            //Move the enemy
            MoveEnemy(gameTime);

            //Update the enemys frame
            texture.UpdateFrame();

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

            //Get the cameras offset for drawing
            Vector2 cameraOffset = scene.CameraPos - scene.ScenesGame.screenSize / 2;

            //Draw the enemy
            Rectangle rec = new Rectangle(position.ToPoint() - cameraOffset.ToPoint(), size.ToPoint());
            spriteBatch.Draw(texture.Texture, rec, texture.GetCurrentFrame(), Color.White, 0, Vector2.Zero, spriteEffect, 1f);

            spriteBatch.End();

            //Draw the health bar on the enemies head
            healthBar.Position = new Vector2(position.X - cameraOffset.X, (float)Math.Floor(position.Y) - cameraOffset.Y) + new Vector2(0, -10);
            healthBar.BarPercent = Math.Clamp((health / maxHealth) * 100, 0, 100);
            healthBar.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
