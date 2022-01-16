/*  Program: FontHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to load and hold fonts for other objects to use
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

namespace StormHead.Handlers
{
    public class FontHolder
    {
        public static SpriteFont regularFont;
        public static SpriteFont hilightFont;
        public static SpriteFont pointsFont;
        public static SpriteFont gameOverFont;
        public static SpriteFont finalScoreFont;
        public static SpriteFont infoFont;

        /// <summary>
        /// Loads all the fonts used for the game
        /// </summary>
        /// <param name="game">The game to load the textures from</param>
        public static void LoadFonts(Game1 game)
        {
            regularFont = game.Content.Load<SpriteFont>("fonts/menuFont");
            hilightFont = game.Content.Load<SpriteFont>("fonts/selectedMenuFont");
            pointsFont = game.Content.Load<SpriteFont>("fonts/pointsFont");
            gameOverFont = game.Content.Load<SpriteFont>("fonts/gameOverFont");
            finalScoreFont = game.Content.Load<SpriteFont>("fonts/finalScoreFont");
            infoFont = game.Content.Load<SpriteFont>("fonts/infoFont");
        }
    }
}
