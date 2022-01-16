/*  Program: TextureAnimated.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A texture that is animated for use with a player or enemy
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StormHead.Models
{
    public class TextureAnimated
    {
        private Texture2D texture;
        private SoundEffect texturesSound;
        private int soundFrame = 0;
        private List<Rectangle> frames;
        private int col;
        private int row;
        private Vector2 dimension;
        private Vector2 animationPosOffset;

        private bool isDone = false;
        protected int frameIndex = 0;
        private int startIndex;
        private int endIndex;

        private int delay;
        protected int delayCounter = 0;
        private bool loop;
        private bool playSound = true;
        private bool canBeInterrupted;
        private int cooldown = 0;

        public Texture2D Texture { get => texture; set => texture = value; }
        public bool IsDone { get => isDone; set => isDone = value; }
        public bool CanBeInterrupted { get => canBeInterrupted; set => canBeInterrupted = value; }
        public Vector2 AnimationPosOffset { get => animationPosOffset; set => animationPosOffset = value; }
        public int Cooldown { get => cooldown; set => cooldown = value; }

        public TextureAnimated(Texture2D texture, int row, int col, int delay, Vector2 animationPosOffset, int cooldown, SoundEffect texturesSound,
            int soundFrame = 0, bool loop = true, bool canBeInterrupted = true, int startIndex = -1, int endIndex = -1)
        {
            this.texture = texture;
            this.col = col;
            this.row = row;
            this.delay = delay;
            this.loop = loop;
            this.canBeInterrupted = canBeInterrupted;
            this.animationPosOffset = animationPosOffset;
            this.cooldown = cooldown;
            this.texturesSound = texturesSound;
            this.soundFrame = soundFrame;

            //Create the dimensions of the textures frames
            this.dimension = new Vector2(texture.Width / col, texture.Height / row);
            CreateFrames();
            
            //If there is no given start index then set it to 0
            if (startIndex == -1)
                startIndex = 0;

            //If there is no given end index then set it to the last frame
            if (endIndex == -1)
                endIndex = frames.Count - 1;

            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.frameIndex = startIndex;
        }

        /// <summary>
        /// Creates the frames rectangles
        /// </summary>
        private void CreateFrames()
        {
            //Makes a new list for the frames
            frames = new List<Rectangle>();

            //Loop through the rows and columns
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    //Gets the position of the frame
                    Point framesPos = new Point((int)dimension.X * x, (int)dimension.Y * y);
                    //Makes the rectangle of the frame
                    Rectangle rec = new Rectangle(framesPos, dimension.ToPoint());
                    //Adds the rectangle to the frame list
                    frames.Add(rec);
                }
            }
        }

        //Stops and restarts the animation
        public void Stop()
        {
            //Set the frame index to the start
            frameIndex = startIndex;
            //Restart the delay counter
            delayCounter = 0;
            //Set the is done to false
            isDone = false;
        }

        /// <summary>
        /// Goes to the next frame of the animation
        /// </summary>
        public void UpdateFrame()
        {
            //If the animation is not done
            if (!isDone)
            {
                //If the animation is on the sound frame and there is a sound
                if (frameIndex == soundFrame && delayCounter == 0 && texturesSound != null && playSound)
                {
                    //Play the sound
                    texturesSound.Play();
                }

                //Add one to the delay counter
                delayCounter++;
                //If the counter is past the delay
                if (delayCounter > delay)
                {
                    //reset the counter
                    delayCounter = 0;

                    //Advance the frame index
                    frameIndex++;
                    //If the frame index is past the end
                    if (frameIndex >= endIndex || frameIndex >= frames.Count)
                    {
                        //Put the frame index to the start
                        frameIndex = startIndex;
                        //If the animation does not loop
                        if (!loop)
                        {
                            //Set the frameIndex to the end
                            frameIndex = endIndex;
                            //Set done to true
                            isDone = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the current frame of the animation
        /// </summary>
        /// <returns>The current frames rectangle</returns>
        public Rectangle GetCurrentFrame()
        {
            //Gets the current frames rectangle
            Rectangle currentFrame = frames[frameIndex];
            return currentFrame;
        }
    }
}
