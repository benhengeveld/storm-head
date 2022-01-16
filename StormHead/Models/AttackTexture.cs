/*  Program: AttackTexture.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A more advance animated texture used for players attacks
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Models
{
    public class AttackTexture : TextureAnimated
    {
        private float attackDamage;
        private Vector2 hitboxPos;
        private Vector2 hitboxSize;
        private int attackFrameNumber;

        public float AttackDamage { get => attackDamage; set => attackDamage = value; }
        public Vector2 HitboxPos { get => hitboxPos; set => hitboxPos = value; }
        public Vector2 HitboxSize { get => hitboxSize; set => hitboxSize = value; }

        public AttackTexture(Texture2D texture, int row, int col, int delay, Vector2 animationPosOffset, int cooldown,
            float attackDamage, Vector2 hitboxPos, Vector2 hitboxSize, int attackFrameNumber, SoundEffect texturesSound,
            int soundFrame = 0, bool loop = true, bool canBeInterrupted = true, int startIndex = -1, int endIndex = -1)
            : base(texture, row, col, delay, animationPosOffset, cooldown, texturesSound, soundFrame, loop, canBeInterrupted, startIndex, endIndex)
        {
            this.attackDamage = attackDamage;
            this.hitboxPos = hitboxPos;
            this.hitboxSize = hitboxSize;
            this.attackFrameNumber = attackFrameNumber;
        }

        /// <summary>
        /// Returns if it is time from the player to spawn the attacks hitbox
        /// </summary>
        /// <returns></returns>
        public bool IsTimeToAttack()
        {
            //Check if the frame index is at the attack frame
            if (frameIndex == attackFrameNumber && delayCounter == 0)
            {
                //It is time to attack
                return true;
            }
            return false;
        }
    }
}
