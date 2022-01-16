/*  Program: Player.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: The user character that the user can move and use attacks with
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
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
    public class Player : DrawableGameComponent
    {
        private GameScene scene;
        private SpriteBatch spriteBatch;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private Point horizontalFlipOffset = new Point(-80, 0);

        //All the players textures
        private TextureAnimated currentTexture;
        private TextureAnimated idleTexture;
        private TextureAnimated runTexture;
        private TextureAnimated jumpTexture;
        private TextureAnimated dashTeleportTexture;
        private AttackTexture attackOneTexture;
        private AttackTexture superchargedAttackTexture;

        //Ability cooldowns
        private int dashCooldownCounter = 0;
        private int attackOneCooldownCounter = 0;
        private int superAttackCooldownCounter = 0;

        //Players position and size
        private Vector2 position;
        private Vector2 size;

        //Health
        private bool canBeDamaged = true;
        private float maxHealth;
        private float health;
        private HealthBar healthBar;
        private Vector2 healthBarSize = new Vector2(400, 50);

        //Players hitbox
        private Vector2 hitboxPos;
        private Vector2 hitboxSize;

        //Players movement
        private Vector2 velocity = Vector2.Zero;
        private Vector2 gravity;
        private bool useGravity = true;
        private bool stopMovement = false;
        private float speed;
        private float jumpHeight = 2;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 HitboxPos { get => hitboxPos; set => hitboxPos = value; }
        public Vector2 HitboxSize { get => hitboxSize; set => hitboxSize = value; }
        public bool CanBeDamaged { get => canBeDamaged; set => canBeDamaged = value; }

        public Player(GameScene scene, Vector2 position, Vector2 size, Vector2 hitboxPos, Vector2 hitboxSize,
            float speed, float jumpHeight, float maxHealth, Vector2 gravity,
            TextureAnimated idleTexture, TextureAnimated runTexture,
            TextureAnimated jumpTexture, TextureAnimated dashTeleportTexture, AttackTexture attackOneTexture,
            AttackTexture superchargedAttackTexture) : base(scene.ScenesGame)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;

            this.position = position;
            this.size = size;
            this.hitboxPos = hitboxPos;
            this.hitboxSize = hitboxSize;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.gravity = gravity;

            this.idleTexture = idleTexture;
            this.runTexture = runTexture;
            this.jumpTexture = jumpTexture;
            this.dashTeleportTexture = dashTeleportTexture;
            this.attackOneTexture = attackOneTexture;
            this.superchargedAttackTexture = superchargedAttackTexture;

            //Make a health bar for the player in the top center of the screen
            Vector2 healthBarPos = new Vector2(scene.ScenesGame.screenSize.X / 2 - healthBarSize.X / 2, 25);
            healthBar = new HealthBar(scene, healthBarPos, healthBarSize, TextureHolder.pixelTexture);
        }

        /// <summary>
        /// Damage a player by a given amount, and if the player dies end the game
        /// </summary>
        /// <param name="damage">The amount of damage</param>
        public void DamagePlayer(float damage)
        {
            //If the damage wont kill the player
            if (health - damage > 0)
            {
                //Remove the damage amount from the players health
                health -= damage;
            }
            else //The player dies
            {
                //Set the player health to 0
                health = 0;
                //Stop the player
                Visible = false;
                Enabled = false;
                canBeDamaged = false;
                stopMovement = true;
                //Make the level game over
                scene.GameOver = true;
            }
        }

        /// <summary>
        /// Switchs the current texture to a new texture and resets the old texture
        /// </summary>
        /// <param name="newTexture">The new texture to change to</param>
        public void SetCurrentTexture(TextureAnimated newTexture)
        {
            if (currentTexture == newTexture)
                return;

            if(currentTexture != null)
                currentTexture.Stop();
            currentTexture = newTexture;
        }

        /// <summary>
        /// Lets the user dash teleport in the derection of the player
        /// </summary>
        public void UseDashTeleport()
        {
            //Reset the cooldown for this ability
            dashCooldownCounter = dashTeleportTexture.Cooldown;

            //Turn of gravity and stop the player from moving
            useGravity = false;
            velocity.Y = 0;
            //Set the current texture to dash teleport
            SetCurrentTexture(dashTeleportTexture);
        }

        /// <summary>
        /// Lets the user use attack one in the derection of the mouse
        /// </summary>
        public void UseAttackOne()
        {
            //Reset the cooldown for this ability
            attackOneCooldownCounter = attackOneTexture.Cooldown;

            //Find what side of the screen the mouse is on
            MouseState ms = Mouse.GetState();
            float xMidPos = scene.ScenesGame.screenSize.X / 2;
            //Set what way the mouse is facing
            if (ms.X > xMidPos)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipHorizontally;

            //Set the current texture to attack one
            SetCurrentTexture(attackOneTexture);
        }

        /// <summary>
        /// Lets the user super charge attack in the derection of the player
        /// </summary>
        public void UseSuperchargedAttack()
        {
            //Reset the cooldown for this ability
            superAttackCooldownCounter = superchargedAttackTexture.Cooldown;

            //Turn of gravity and stop the player from moving
            useGravity = false;
            velocity.Y = 0;
            //Set the current texture to super charged attack
            SetCurrentTexture(superchargedAttackTexture);
        }

        /// <summary>
        /// Checks if the player is on the floor
        /// </summary>
        /// <returns>Returns true if the player is on the floor</returns>
        public bool IsPlayerOnFloor()
        {
            //Get the hitbox of the player but moved 1 pixel down
            CollidableObject collisionObject = 
                scene.CollisionHandler.CheckForCollision(new Rectangle(hitboxPos.ToPoint() + position.ToPoint() + new Point(0, 1),
                hitboxSize.ToPoint()));
            //Check if the hitbox hit something
            if (collisionObject != null)
                return true;

            return false;
        }

        /// <summary>
        /// Moves the player based on the players velocity and gravity
        /// </summary>
        /// <param name="gameTime">The games time used for gravity</param>
        public void MovePlayer(GameTime gameTime)
        {
            //Save the players old position
            Vector2 oldPos = position;
            //Move the players x with the players velocity and speed
            position.X += velocity.X * speed;
            //Get the hitbox in the players new position
            CollidableObject collisionObject = 
                scene.CollisionHandler.CheckForCollision(new Rectangle(hitboxPos.ToPoint() + position.ToPoint(),
                hitboxSize.ToPoint()));
            //Check if the new position collides with something
            if (collisionObject != null)
            {
                //If the player if closer to the left side of the object that was hit, put the player to the left of the object hit
                if (position.X + hitboxPos.X + hitboxSize.X / 2 < collisionObject.GetPosition().X + collisionObject.GetSize().X / 2)
                {
                    position.X = collisionObject.GetPosition().X - hitboxPos.X - hitboxSize.X;
                }
                //If the player if closer to the right side of the object that was hit, put the player to the right of the object hit
                else
                {
                    position.X = collisionObject.GetPosition().X - hitboxPos.X + collisionObject.GetSize().X;
                }

                //If the player is still colliding, then set hit x pos back to what it was at the start
                collisionObject = 
                    scene.CollisionHandler.CheckForCollision(new Rectangle(hitboxPos.ToPoint() + position.ToPoint(),
                    hitboxSize.ToPoint()));
                if (collisionObject != null)
                {
                    position.X = oldPos.X;
                }
            }

            //If the players gravity is on
            if (useGravity)
            {
                //Save the players old position
                oldPos = position;
                //Move the players y with the players velocity and speed
                position.Y += velocity.Y * speed;
                //Get the hitbox in the players new position
                collisionObject = 
                    scene.CollisionHandler.CheckForCollision(new Rectangle(hitboxPos.ToPoint() + position.ToPoint(),
                    hitboxSize.ToPoint()));
                //Check if the new position collides with something
                if (collisionObject != null)
                {
                    //If the player is moving down put the player on the top of the object hit
                    if (velocity.Y > 0)
                    {
                        position.Y = collisionObject.GetPosition().Y - hitboxPos.Y - hitboxSize.Y;
                    }
                    //If the player is moving up put the player on the bottom of the object hit
                    else
                    {
                        position.Y = collisionObject.GetPosition().Y - hitboxPos.Y + collisionObject.GetSize().Y;
                    }

                    //If the player is still colliding, then set hit x pos back to what it was at the start
                    collisionObject = 
                        scene.CollisionHandler.CheckForCollision(new Rectangle(hitboxPos.ToPoint() + position.ToPoint(),
                        hitboxSize.ToPoint()));
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

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            //Reset the x velocity
            velocity.X = 0;

            //Update ability cooldowns
            if (dashCooldownCounter > 0)
                dashCooldownCounter--;

            if (attackOneCooldownCounter > 0)
                attackOneCooldownCounter--;

            if (superAttackCooldownCounter > 0)
                superAttackCooldownCounter--;

            //If the current texture is done playing
            if (currentTexture != null && currentTexture.IsDone)
            {
                //Move the play based on the textures offset and the direction the player is facing
                if (spriteEffect == SpriteEffects.None)
                    position += currentTexture.AnimationPosOffset;
                else
                    position -= currentTexture.AnimationPosOffset;
            }

            //If the current texture is an attack texture
            if (currentTexture is AttackTexture)
            {
                //Get the attack texture
                AttackTexture attackTexture = (AttackTexture)currentTexture;
                //If it is the attack textures time to attack
                if (attackTexture.IsTimeToAttack())
                {
                    Rectangle attackHitbox = new Rectangle();
                    //If the player is facing right
                    if (spriteEffect == SpriteEffects.None)
                    {
                        //Get the position and size of the attacks hitbox to the right of the player
                        Vector2 hitboxWorldPos = position + hitboxPos + hitboxSize / 2 + attackTexture.HitboxPos;
                        attackHitbox = new Rectangle(hitboxWorldPos.ToPoint(), attackTexture.HitboxSize.ToPoint());
                    }
                    //If the player is facing left
                    else
                    {
                        //Get the position and size of the attacks hitbox to the left of the player
                        Vector2 hitboxWorldPos = position + hitboxPos + hitboxSize / 2 + attackTexture.HitboxPos - new Vector2(attackTexture.HitboxSize.X, 0);
                        attackHitbox = new Rectangle(hitboxWorldPos.ToPoint(), attackTexture.HitboxSize.ToPoint());
                    }

                    //Loop through every enemy
                    foreach (Enemy enemy in scene.Enemies)
                    {
                        //Get the enemies rectangle
                        Rectangle enemyRec = new Rectangle(enemy.Position.ToPoint(), enemy.Size.ToPoint());
                        //If the enemy can be damaged and the attacks hitbox intersects with the enemy
                        if (enemy.CanBeDamaged && attackHitbox.Intersects(enemyRec))
                        {
                            //Damage the enemy the attacks amount
                            enemy.DamageEnemy(attackTexture.AttackDamage);
                        }
                    }
                }
            }
            //Remove all dead enemies in the scene
            scene.RemoveDeadEnemies();

            //If there is no current texture, of the current texture is done or can be interrupted
            if (currentTexture == null || currentTexture.CanBeInterrupted || currentTexture.IsDone)
            {
                //Allow the player to move
                stopMovement = false;
                //If the player presses e, the current texture is not already the super charged attack, and the attacks cooldown is 0
                if (ks.IsKeyDown(Keys.E) && currentTexture != superchargedAttackTexture && superAttackCooldownCounter == 0)
                {
                    //Stop the players movement
                    stopMovement = true;
                    //Use the super charged attack
                    UseSuperchargedAttack();
                }
                //If the player presses left mouse, the current texture is not already the attack one, and the attacks cooldown is 0
                else if (ms.LeftButton == ButtonState.Pressed && currentTexture != attackOneTexture && attackOneCooldownCounter == 0)
                {
                    //Use attack one
                    UseAttackOne();
                }
                //If the player presses f, the current texture is not already the dash teleport, and the abilitys cooldown is 0
                else if (ks.IsKeyDown(Keys.F) && currentTexture != dashTeleportTexture && dashCooldownCounter == 0)
                {
                    //Stop the players movement
                    stopMovement = true;
                    //Use dash teleport
                    UseDashTeleport();
                }
                //If d is press and a is not
                else if (ks.IsKeyDown(Keys.D) && ks.IsKeyUp(Keys.A))
                {
                    //Allow gravity
                    useGravity = true;
                    //Set the player to face to the right
                    spriteEffect = SpriteEffects.None;
                    //If the player is on the floor set the texture to run, otherwise set it to jump
                    if (IsPlayerOnFloor())
                        SetCurrentTexture(runTexture);
                    else
                        SetCurrentTexture(jumpTexture);
                }
                //If a is press and d is not
                else if (ks.IsKeyDown(Keys.A) && ks.IsKeyUp(Keys.D))
                {
                    //Allow gravity
                    useGravity = true;
                    //Set the player to face to the left
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    //If the player is on the floor set the texture to run, otherwise set it to jump
                    if (IsPlayerOnFloor())
                        SetCurrentTexture(runTexture);
                    else
                        SetCurrentTexture(jumpTexture);
                }
                //No other key is used
                else
                {
                    //Allow gravity
                    useGravity = true;
                    //If the player is on the floor set the texture to idle, otherwise set it to jump
                    if(IsPlayerOnFloor())
                        SetCurrentTexture(idleTexture);
                    else
                        SetCurrentTexture(jumpTexture);
                }
            }

            //If movement is allowed
            if (!stopMovement)
            {
                //If d is press and a is not
                if (ks.IsKeyDown(Keys.D) && ks.IsKeyUp(Keys.A))
                {
                    //Set the velocity to the right
                    velocity.X = 1;
                }
                //If a is press and d is not
                else if (ks.IsKeyDown(Keys.A) && ks.IsKeyUp(Keys.D))
                {
                    //Set the velocity to the left
                    velocity.X = -1;
                }

                //If the space bar is press and the player is on the floor
                if (ks.IsKeyDown(Keys.Space) && IsPlayerOnFloor())
                {
                    //Set the velocity to make the player jump
                    velocity.Y = -jumpHeight;
                }
            }
            
            //Move the player
            MovePlayer(gameTime);
            
            //update the current textures frame
            if (currentTexture != null)
                currentTexture.UpdateFrame();

            //Make the scenes camera move towards the center of the players hitbox
            scene.MoveCameraTowards = position + hitboxPos + hitboxSize / 2;

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

            //If there is a current texture
            if (currentTexture != null)
            {
                Point offset = Point.Zero;
                //If the player is facing Left get the flip offset
                if (spriteEffect == SpriteEffects.FlipHorizontally)
                    offset = horizontalFlipOffset;

                //Get a rectangle for the players position and size
                Rectangle rec = new Rectangle(position.ToPoint() + offset - cameraOffset.ToPoint(), size.ToPoint());
                //Draw the player
                spriteBatch.Draw(currentTexture.Texture, rec, currentTexture.GetCurrentFrame(), Color.White, 0, Vector2.Zero,
                    spriteEffect, 1f);
            }
            
            spriteBatch.End();

            //Get the health percent and draw the health bar
            healthBar.BarPercent = Math.Clamp((health / maxHealth) * 100, 0, 100);
            healthBar.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
