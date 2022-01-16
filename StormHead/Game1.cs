/*  Program: Game1.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: The main game file
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
using StormHead.Scenes;

namespace StormHead
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public Vector2 screenSize = new Vector2(1500, 900);

        public SpriteBatch _spriteBatch;
        public LevelScoreHandler levelScoreHandler;

        //All the scenes
        public MenuScene menuScene;
        public HowToPlayScene howToPlayScene;
        public AboutScene aboutScene;
        public HighScoresScene highScoresScene;
        public LevelSelectScene levelSelectScene;
        public LevelOneScene levelOneScene;
        public LevelTwoScene levelTwoScene;

        /// <summary>
        /// Hides all the scenes from view
        /// </summary>
        public void HideAllScenes()
        {
            //Loop through every component
            for (int i = 0; i < this.Components.Count; i++)
            {
                //Get the current component
                IGameComponent item = this.Components[i];
                //Check if the current component is a scene
                if (item is Scene)
                {
                    //Hide the scene
                    Scene scene = (Scene)item;
                    scene.Hide();
                }
            }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Set the screen size
            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            _graphics.ApplyChanges();

            //Load all the sounds, textures, and fonts
            SoundHolder.LoadSounds(this);
            TextureHolder.LoadTextures(this);
            FontHolder.LoadFonts(this);

            //Make a new score handler
            levelScoreHandler = new LevelScoreHandler();

            //Make all the scenes

            //Menu scene
            menuScene = new MenuScene(this, null);
            this.Components.Add(menuScene);

            //How to play scene
            howToPlayScene = new HowToPlayScene(this, null);
            this.Components.Add(howToPlayScene);

            //About scene
            aboutScene = new AboutScene(this, null);
            this.Components.Add(aboutScene);

            //High-score scene
            highScoresScene = new HighScoresScene(this, null);
            this.Components.Add(highScoresScene);

            //Level select scene
            levelSelectScene = new LevelSelectScene(this, null);
            this.Components.Add(levelSelectScene);

            //Level one scene
            levelOneScene = new LevelOneScene(this, SoundHolder.gameMusic);
            this.Components.Add(levelOneScene);

            //Level two scene
            levelTwoScene = new LevelTwoScene(this, SoundHolder.gameMusic);
            this.Components.Add(levelTwoScene);

            //Show the menu scene
            menuScene.Show();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
