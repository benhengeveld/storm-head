/*  Program: GameScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A more advance scene that is used for levels
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
using System.Diagnostics;
using System.Text;

namespace StormHead.Models
{
    public abstract class GameScene : Scene
    {
        protected Random random;

        //Camera
        protected Vector2 cameraPos;
        protected Vector2 moveCameraTowards;
        protected float maxCamSpeed = 200;
        protected float camSpeedMulti = 15;
        
        protected List<CollidableObject> collidableObjects;
        protected CollisionHandler collisionHandler;
        protected List<Enemy> enemies;
        protected MapLoader mapLoader;
        protected bool gameOver = false;

        //Player
        protected int playersScore = 0;
        protected Player player;
        protected Vector2 playerSize = new Vector2(128 * 4, 64 * 4);
        protected Vector2 playerHitboxPos = new Vector2(204, 140);
        protected Vector2 playerHitboxSize = new Vector2(24, 52);
        protected float playerSpeed = 7;
        protected float playerJumpHeight = 2;
        protected float playerMaxHealth = 100;
        protected Vector2 playerGravity = new Vector2(0, 5);

        //Ghost
        protected Vector2 ghostSize = new Vector2(43, 60);
        protected float ghostMinScale = 0.7f;
        protected float ghostMaxScale = 1.3f;
        protected float ghostMinSpeed = 2;
        protected float ghostMaxSpeed = 6;
        protected float ghostMinJumpHeight = 2f;
        protected float ghostMaxJumpHeight = 3f;
        protected float ghostMaxHealth = 50;
        protected float ghostDamage = 5;
        protected int ghostDamageCooldown = 50;
        protected Vector2 ghostGravity = new Vector2(0, 5);
        protected int ghostPointWorth = 100;

        //Laser ghost
        protected Vector2 laserEnemySize = new Vector2(43, 60);
        protected float laserEnemyMinScale = 0.7f;
        protected float laserEnemyMaxScale = 1.3f;
        protected float laserEnemyMinSpeed = 1;
        protected float laserEnemyMaxSpeed = 4;
        protected float laserEnemyMinJumpHeight = 2f;
        protected float laserEnemyMaxJumpHeight = 3f;
        protected float laserEnemyMaxHealth = 75;
        protected float laserEnemyDamage = 15;
        protected int laserEnemyDamageCooldown = 300;
        protected Vector2 laserEnemyGravity = new Vector2(0, 5);
        protected int laserEnemyPointWorth = 200;
        protected Color laserColor = Color.Red;
        protected Vector2 laserSize = new Vector2(2000, 5);
        protected int laserCooldown = 10;

        //Players score
        protected SpriteFont scoreFont = FontHolder.pointsFont;
        protected Color scoreColor = Color.Black;
        protected Vector2 scorePos = new Vector2(15, 15);

        //Game over text
        protected String gameOverText = "Game Over!";
        protected String leaveInfoText = "Press Enter to go to the menu";
        protected Vector2 gameOverBackgroundOffside = new Vector2(25, 25);
        protected SpriteFont gameOverFont = FontHolder.gameOverFont;
        protected SpriteFont finalScoreFont = FontHolder.finalScoreFont;
        protected SpriteFont exitFont = FontHolder.infoFont;

        public List<CollidableObject> CollidableObjects { get => collidableObjects; set => collidableObjects = value; }
        public CollisionHandler CollisionHandler { get => collisionHandler; set => collisionHandler = value; }
        public Player Player { get => player; set => player = value; }
        public List<Enemy> Enemies { get => enemies; set => enemies = value; }
        public Vector2 CameraPos { get => cameraPos; set => cameraPos = value; }
        public Vector2 MoveCameraTowards { get => moveCameraTowards; set => moveCameraTowards = value; }
        public int PlayersScore { get => playersScore; set => playersScore = value; }
        public bool GameOver { get => gameOver; set => gameOver = value; }

        public GameScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            this.collidableObjects = new List<CollidableObject>();
            this.collisionHandler = new CollisionHandler(this);
            this.enemies = new List<Enemy>();
            this.cameraPos = ScenesGame.screenSize / 2;
            this.moveCameraTowards = cameraPos;

            random = new Random();
        }

        /// <summary>
        /// Sets up the level and makes it visable
        /// </summary>
        public override void Show()
        {
            //Reset the lists and make the game over false
            CollidableObjects = new List<CollidableObject>();
            Enemies = new List<Enemy>();
            Components = new List<GameComponent>();
            GameOver = false;

            //Get all the walls from the map loader
            List<Wall> walls = mapLoader.GetWalls();
            //Loop through all the walls and add them to the components list
            foreach (Wall wall in walls)
            {
                this.Components.Add(wall);
            }

            //Spawn the first set of enemies
            SpawnGhosts();
            SpawnLaserGhosts();

            //Get the players position from the map loader
            Vector2 playerPosition = mapLoader.GetPlayerPosition() - playerHitboxPos - playerHitboxSize / 2;
            //Make the player
            Player = new Player(this, playerPosition, playerSize, playerHitboxPos,
                playerHitboxSize, playerSpeed, playerJumpHeight, playerMaxHealth, playerGravity,
                TextureHolder.idleTexture, TextureHolder.runTexture, TextureHolder.jumpTexture,
                TextureHolder.dashTeleportTexture, TextureHolder.attackOneTexture, TextureHolder.superchargedAttackTexture);
            //Add the player to the components list
            this.Components.Add(Player);
            //Set the camera position to the players position
            CameraPos = Player.Position + Player.HitboxPos + Player.HitboxSize / 2;

            base.Show();
        }

        /// <summary>
        /// Spawns all the ghosts in the level
        /// </summary>
        public void SpawnGhosts()
        {
            //Get a list of the ghost spawn positions from the map loader
            List<Vector2> ghostPositions = mapLoader.GetGhostPositions();
            //Loop through all the positions
            foreach (Vector2 ghostPosition in ghostPositions)
            {
                //Get the position the new ghost is spawning at
                Vector2 ghostPos = ghostPosition - ghostSize / 2;
                //Get a random speed between the range set
                float ghostSpeed = (float)random.NextDouble() * (ghostMaxSpeed - ghostMinSpeed) + ghostMinSpeed;
                //Get a random jump height between the range set
                float ghostJumpHeight = (float)random.NextDouble() * (ghostMaxJumpHeight - ghostMinJumpHeight) + ghostMinJumpHeight;
                //Get a random scale between the range set
                float ghostScale = (float)random.NextDouble() * (ghostMaxScale - ghostMinScale) + ghostMinScale;

                //Make the new ghost
                BasicEnemy newGhost = new BasicEnemy(this, TextureHolder.ghostTexture, ghostPos, ghostSize * ghostScale, SoundHolder.ghostAttack,
                    ghostSpeed, ghostJumpHeight, ghostMaxHealth, ghostDamage, ghostDamageCooldown, ghostGravity, ghostPointWorth);
                //Add the ghost to the components list
                this.Components.Add(newGhost);
            }
        }

        /// <summary>
        /// Spawns all the laser ghosts in the level
        /// </summary>
        public void SpawnLaserGhosts()
        {
            //Get a list of the laser ghost spawn positions from the map loader
            List<Vector2> laserEnemyPositions = mapLoader.GetLaserEnemiesPositions();
            //Loop through all the positions
            foreach (Vector2 laserEnemyPosition in laserEnemyPositions)
            {
                //Get the position the new laser ghost is spawning at
                Vector2 laserEnemyPos = laserEnemyPosition - laserEnemySize / 2;
                //Get a random speed between the range set
                float laserEnemySpeed = (float)random.NextDouble() * (laserEnemyMaxSpeed - laserEnemyMinSpeed) + laserEnemyMinSpeed;
                //Get a random jump height between the range set
                float laserEnemyJumpHeight = (float)random.NextDouble() * (laserEnemyMaxJumpHeight - laserEnemyMinJumpHeight) + laserEnemyMinJumpHeight;
                //Get a random scale between the range set
                float laserEnemyScale = (float)random.NextDouble() * (laserEnemyMaxScale - laserEnemyMinScale) + laserEnemyMinScale;

                //Make the new laser ghost
                LaserEnemy newLaserEnemy = new LaserEnemy(this, TextureHolder.whiteGhostTexture, laserEnemyPos,
                    laserEnemySize * laserEnemyScale, SoundHolder.laserAttack, laserEnemySpeed, laserEnemyJumpHeight,
                    laserEnemyMaxHealth, laserEnemyDamage, laserEnemyDamageCooldown, laserEnemyGravity, laserEnemyPointWorth,
                    laserColor, laserSize, laserCooldown);
                //Add the laser ghost to the components list
                this.Components.Add(newLaserEnemy);
            }
        }

        /// <summary>
        /// Removes all the dead enemies
        /// </summary>
        public void RemoveDeadEnemies()
        {
            List<Enemy> deadEnemies = new List<Enemy>();

            //Loop through all the scenes enemies
            foreach (Enemy enemy in enemies)
            {
                //If the enemy is dead add them to the dead enemies list
                if (enemy.Health == 0)
                {
                    deadEnemies.Add(enemy);
                }
            }

            //Loop through all dead enemies
            foreach (Enemy deadEnemy in deadEnemies)
            {
                //Remove the dead enemy from the enemies list
                enemies.Remove(deadEnemy);
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //If there is no more enemies and the game is not over
            if (Enemies.Count == 0 && !GameOver)
            {
                //Spawn more enemies
                SpawnGhosts();
                SpawnLaserGhosts();
            }

            //If the player press the esc key return to the menu
            if (ks.IsKeyDown(Keys.Escape))
            {
                ScenesGame.HideAllScenes();
                ScenesGame.menuScene.Show();
            }

            //Get the distance from the camera position to the move camera towards position
            float camXDistance =  moveCameraTowards.X - cameraPos.X;
            float camYDistance = moveCameraTowards.Y - cameraPos.Y;
            float camDistance = (float)Math.Sqrt(Math.Abs(camXDistance * camXDistance) + Math.Abs(camYDistance * camYDistance));

            //If the distance is less then 1, set the camera position to the move camera towards position
            if (camDistance < 1)
            {
                cameraPos = moveCameraTowards;
            }
            else
            {
                //Move the camera towards the camera towards position with the speed based on the distance to the camera towards position
                cameraPos += new Vector2(Math.Clamp(camXDistance, -maxCamSpeed, maxCamSpeed),
                    Math.Clamp(camYDistance, -maxCamSpeed, maxCamSpeed)) / camSpeedMulti;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();

            //Draw players score
            string scoreText = $"Score: {playersScore}";
            SpriteBatch.DrawString(scoreFont, scoreText, scorePos, scoreColor);

            //If the game is over
            if (GameOver)
            {
                //Get the game over pos
                Vector2 gameOverPos = new Vector2(ScenesGame.screenSize.X / 2 - gameOverFont.MeasureString(gameOverText).X / 2,
                    ScenesGame.screenSize.Y / 2 - gameOverFont.LineSpacing);

                //Get the final score text and pos
                String finalScoreText = $"Final Score: {PlayersScore}";
                Vector2 finalScorePos = new Vector2(ScenesGame.screenSize.X / 2 - finalScoreFont.MeasureString(finalScoreText).X / 2,
                    ScenesGame.screenSize.Y / 2);

                //Get the leave info pos
                Vector2 leaveInfoPos = new Vector2(ScenesGame.screenSize.X / 2 - exitFont.MeasureString(leaveInfoText).X / 2,
                    finalScorePos.Y + finalScoreFont.LineSpacing);

                //Get the background pos and size
                Vector2 gameOverBackgroundPos = gameOverPos - gameOverBackgroundOffside;
                Vector2 gameOverBackgroundSize = new Vector2(gameOverFont.MeasureString(gameOverText).X,
                    leaveInfoPos.Y + finalScoreFont.LineSpacing - gameOverPos.Y) + gameOverBackgroundOffside * 2;

                //Draw the game over, final score, leave info, and background
                SpriteBatch.Draw(TextureHolder.pixelTexture, new Rectangle(gameOverBackgroundPos.ToPoint(), gameOverBackgroundSize.ToPoint()), Color.Black);
                SpriteBatch.DrawString(gameOverFont, gameOverText, gameOverPos, Color.Red);
                SpriteBatch.DrawString(finalScoreFont, finalScoreText, finalScorePos, Color.White);
                SpriteBatch.DrawString(exitFont, leaveInfoText, leaveInfoPos, Color.White);
            }

            SpriteBatch.End();
        }
    }
}
