/*  Program: HealthBar.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Shows a health bar based on a given percent
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Objects
{
    public class HealthBar : DrawableGameComponent
    {
        private GameScene scene;
        private SpriteBatch spriteBatch;

        private Vector2 position;
        private Vector2 size;
        private Vector2 barOffset = new Vector2(5, 5);
        private Texture2D pixelTexture;

        private Color backgroundColor = Color.Black;
        private Color barColor = Color.Green;
        private float barPercent = 100;
        private float yellowPercent = 65;
        private float redPercent = 35;

        public Vector2 Position { get => position; set => position = value; }
        public float BarPercent { get => barPercent; set => barPercent = value; }
        public Vector2 BarOffset { get => barOffset; set => barOffset = value; }

        public HealthBar(GameScene scene, Vector2 position, Vector2 size, Texture2D pixelTexture) : base(scene.ScenesGame)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;
            this.Position = position;
            this.size = size;
            this.pixelTexture = pixelTexture;
        }

        public override void Draw(GameTime gameTime)
        {
            //If the bar percent is or bellow the red percent, set the bars color to red
            if (barPercent <= redPercent)
                barColor = Color.Red;
            //If the bar percent is or bellow the yellow percent, set the bars color to yellow
            else if (barPercent <= yellowPercent)
                barColor = Color.Yellow;
            //Otherwise set the bars color to green
            else
                barColor = Color.Green;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            //Draw the health bars background
            spriteBatch.Draw(pixelTexture, new Rectangle(Position.ToPoint(), size.ToPoint()), backgroundColor);

            //Draw the health bar based on the percentage
            Vector2 barPos = Position + barOffset;
            Vector2 barSize = size - barOffset * 2;
            barSize.X *= barPercent / 100;
            spriteBatch.Draw(pixelTexture, new Rectangle(barPos.ToPoint(), barSize.ToPoint()), barColor);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
