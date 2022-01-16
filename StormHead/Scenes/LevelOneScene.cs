/*  Program: LevelOneScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: The first level of the game
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
using StormHead.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StormHead.Scenes
{
    public class LevelOneScene : GameScene
    {
        public LevelOneScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            //Get the map and make a new map loader with the map
            int[,] map = MapHolder.levelOneMap;
            mapLoader = new MapLoader(this, map, 64, Vector2.Zero, TextureHolder.wallTexture);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (GameOver && canSelect && ks.IsKeyDown(Keys.Enter))
            {
                //Save the players score to the first level in the games level score handler
                ScenesGame.levelScoreHandler.LevelOneScore = playersScore;
                //Save the scores to the file
                ScenesGame.levelScoreHandler.SaveLevelScores();

                //Hide all the scenes and then show the menu scene
                ScenesGame.HideAllScenes();
                ScenesGame.menuScene.Show();
            }

            base.Update(gameTime);
        }
    }
}
