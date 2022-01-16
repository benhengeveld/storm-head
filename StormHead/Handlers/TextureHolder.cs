/*  Program: TextureHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to load and hold textures for other objects to use
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
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Handlers
{
    public class TextureHolder
    {
        public static Texture2D howToPlayTexture;
        public static Texture2D pixelTexture;
        public static Texture2D darkGhost;
        public static Texture2D whiteGhost;
        public static Texture2D chainAttack;
        public static Texture2D damaged;
        public static Texture2D dashTeleport;
        public static Texture2D idle;
        public static Texture2D runHop;
        public static Texture2D superchargedAttack;
        public static Texture2D run;
        public static Texture2D wallTexture;

        public static TextureAnimated ghostTexture;
        public static TextureAnimated whiteGhostTexture;
        public static TextureAnimated idleTexture;
        public static TextureAnimated runTexture;
        public static TextureAnimated jumpTexture;
        public static TextureAnimated dashTeleportTexture;

        public static AttackTexture attackOneTexture;
        public static AttackTexture superchargedAttackTexture;

        /// <summary>
        /// Loads all the textures used for the game
        /// </summary>
        /// <param name="game">The game to load the textures from</param>
        public static void LoadTextures(Game1 game)
        {
            //Loads all the texture 2d sprites
            howToPlayTexture = game.Content.Load<Texture2D>("images/HowToPlay");
            pixelTexture = game.Content.Load<Texture2D>("images/pixel");
            darkGhost = game.Content.Load<Texture2D>("images/enemys/dark-ghost");
            whiteGhost = game.Content.Load<Texture2D>("images/enemys/white-ghost");
            chainAttack = game.Content.Load<Texture2D>("images/stormhead/Chain-Attack");
            damaged = game.Content.Load<Texture2D>("images/stormhead/damaged");
            dashTeleport = game.Content.Load<Texture2D>("images/stormhead/dash-teleport");
            idle = game.Content.Load<Texture2D>("images/stormhead/idle");
            runHop = game.Content.Load<Texture2D>("images/stormhead/Run-Hop");
            superchargedAttack = game.Content.Load<Texture2D>("images/stormhead/Supercharged-attack");
            run = game.Content.Load<Texture2D>("images/stormhead/imorpved-run");
            wallTexture = game.Content.Load<Texture2D>("images/wall");

            //Load the animated textures
            LoadTextureAnimated();
            //Load the attack textures
            LoadAttackTexture();
        }

        /// <summary>
        /// Makes all the animated textures
        /// </summary>
        private static void LoadTextureAnimated()
        {
            //Ghost texture
            Texture2D ghostTexture2D = TextureHolder.darkGhost;
            int ghostRow = 1;
            int ghostCol = 6;
            int ghostDelay = 3;
            Vector2 ghostAnimationPosOffset = Vector2.Zero;
            int ghostCooldown = 0;
            SoundEffect ghostTexturesSound = null;
            int ghostSoundFrame = 0;
            bool ghostLoop = true;
            bool ghostCanBeInterrupted = true;

            ghostTexture = new TextureAnimated(ghostTexture2D, ghostRow, ghostCol, ghostDelay, ghostAnimationPosOffset,
                ghostCooldown, ghostTexturesSound, ghostSoundFrame, ghostLoop, ghostCanBeInterrupted);

            //White Ghost Texture
            Texture2D whiteGhostTexture2D = TextureHolder.whiteGhost;
            int whiteGhostRow = 1;
            int whiteGhostCol = 6;
            int whiteGhostDelay = 3;
            Vector2 whiteGhostAnimationPosOffset = Vector2.Zero;
            int whiteGhostCooldown = 0;
            SoundEffect whiteGhostTexturesSound = null;
            int whiteGhostSoundFrame = 0;
            bool whiteGhostLoop = true;
            bool whiteGhostCanBeInterrupted = true;

            whiteGhostTexture = new TextureAnimated(whiteGhostTexture2D, whiteGhostRow, whiteGhostCol, whiteGhostDelay, whiteGhostAnimationPosOffset,
                whiteGhostCooldown, whiteGhostTexturesSound, whiteGhostSoundFrame, whiteGhostLoop, whiteGhostCanBeInterrupted);

            //Idle texture
            Texture2D idleTexture2D = TextureHolder.idle;
            int idleRow = 1;
            int idleCol = 1;
            int idleDelay = 1;
            Vector2 idleAnimationPosOffset = Vector2.Zero;
            int idleCooldown = 0;
            SoundEffect idleTexturesSound = null;

            idleTexture = new TextureAnimated(idleTexture2D, idleRow, idleCol, idleDelay,
                idleAnimationPosOffset, idleCooldown, idleTexturesSound);

            //Run texture
            Texture2D runTexture2D = TextureHolder.run;
            int runRow = 6;
            int runCol = 1;
            int runDelay = 3;
            Vector2 runAnimationPosOffset = Vector2.Zero;
            int runCooldown = 0;
            SoundEffect runTexturesSound = null;

            runTexture = new TextureAnimated(runTexture2D, runRow, runCol, runDelay,
                runAnimationPosOffset, runCooldown, runTexturesSound);

            //Jump texture
            Texture2D jumpTexture2D = TextureHolder.runHop;
            int jumpRow = 11;
            int jumpCol = 1;
            int jumpDelay = 3;
            Vector2 jumpAnimationPosOffset = Vector2.Zero;
            int jumpCooldown = 0;
            SoundEffect jumpTexturesSound = null;
            int jumpSoundFrame = 0;
            bool jumpLoop = true;
            bool jumpCanBeInterrupted = true;
            int jumpStartIndex = 6;
            int jumpEndIndex = 6;

            jumpTexture = new TextureAnimated(jumpTexture2D, jumpRow, jumpCol, jumpDelay, jumpAnimationPosOffset,
                jumpCooldown, jumpTexturesSound, jumpSoundFrame, jumpLoop, jumpCanBeInterrupted, jumpStartIndex, jumpEndIndex);

            //Dash teleport texture
            Texture2D teleportTexture2D = TextureHolder.dashTeleport;
            int teleportRow = 7;
            int teleportCol = 1;
            int teleportDelay = 3;
            Vector2 teleportAnimationPosOffset = new Vector2(200, 0);
            int teleportCooldown = 70;
            SoundEffect teleportTexturesSound = SoundHolder.teleportSound;
            int teleportSoundFrame = 0;
            bool teleportLoop = false;
            bool teleportCanBeInterrupted = false;

            dashTeleportTexture = new TextureAnimated(teleportTexture2D, teleportRow, teleportCol, teleportDelay,
                teleportAnimationPosOffset, teleportCooldown, teleportTexturesSound, teleportSoundFrame, teleportLoop, teleportCanBeInterrupted);
        }

        /// <summary>
        /// Makes all the animated attack textures
        /// </summary>
        private static void LoadAttackTexture()
        {
            //Attack One texture
            Texture2D attackOneTexture2D = TextureHolder.chainAttack;
            int attackOneRow = 17;
            int attackOneCol = 1;
            int attackOneDelay = 1;
            Vector2 attackOneAnimationPosOffset = Vector2.Zero;
            int attackOneCooldown = 0;
            float attackOneDamage = 10;
            Vector2 attackOneHitboxPos = new Vector2(0, -60);
            Vector2 attackOneHitboxSize = new Vector2(70, 90);
            int attackOneFrameNumber = 4;
            SoundEffect attackOneTexturesSound = SoundHolder.swordSlash;
            int attackOneSoundFrame = 0;
            bool attackOneLoop = false;
            bool attackOneCanBeInterrupted = false;
            int attackOneStartIndex = 0;
            int attackOneEndIndex = 6;

            attackOneTexture = new AttackTexture(attackOneTexture2D, attackOneRow, attackOneCol, attackOneDelay,
                attackOneAnimationPosOffset, attackOneCooldown, attackOneDamage, attackOneHitboxPos, attackOneHitboxSize,
                attackOneFrameNumber, attackOneTexturesSound, attackOneSoundFrame, attackOneLoop, attackOneCanBeInterrupted,
                attackOneStartIndex, attackOneEndIndex);

            //Super Charged Attack texture
            Texture2D superAttackTexture2D = TextureHolder.superchargedAttack;
            int superAttackRow = 14;
            int superAttackCol = 1;
            int superAttackDelay = 3;
            Vector2 superAttackAnimationPosOffset = new Vector2(-40, 0);
            int superAttackCooldown = 350;
            float superAttackDamage = 25;
            Vector2 superAttackHitboxPos = new Vector2(0, -145);
            Vector2 superAttackHitboxSize = new Vector2(260, 172);
            int superAttackFrameNumber = 8;
            SoundEffect superAttackTexturesSound = SoundHolder.superAttack;
            int superAttackSoundFrame = 6;
            bool superAttackLoop = false;
            bool superAttackCanBeInterrupted = false;

            superchargedAttackTexture = new AttackTexture(superAttackTexture2D, superAttackRow, superAttackCol, superAttackDelay,
                superAttackAnimationPosOffset, superAttackCooldown, superAttackDamage, superAttackHitboxPos, superAttackHitboxSize,
                superAttackFrameNumber, superAttackTexturesSound, superAttackSoundFrame, superAttackLoop, superAttackCanBeInterrupted);
        }
    }
}