/*  Program: SoundHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to load and hold sounds for other objects to use
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
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Handlers
{
    public class SoundHolder
    {
        public static Song gameMusic;
        public static SoundEffect swordSlash;
        public static SoundEffect superAttack;
        public static SoundEffect teleportSound;
        public static SoundEffect ghostAttack;
        public static SoundEffect laserAttack;

        /// <summary>
        /// Loads all the sound effects and songs used for the game
        /// </summary>
        /// <param name="game">The game to load the sounds from</param>
        public static void LoadSounds(Game1 game)
        {
            gameMusic = game.Content.Load<Song>("sounds/game-music");
            swordSlash = game.Content.Load<SoundEffect>("sounds/sword-slash");
            superAttack = game.Content.Load<SoundEffect>("sounds/super-attack-sound");
            teleportSound = game.Content.Load<SoundEffect>("sounds/teleport-sound");
            ghostAttack = game.Content.Load<SoundEffect>("sounds/ghost-attack-sound");
            laserAttack = game.Content.Load<SoundEffect>("sounds/laser-sound");
        }
    }
}
