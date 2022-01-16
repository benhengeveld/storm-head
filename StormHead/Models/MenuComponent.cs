/*  Program: MenuComponent.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: A menu that the user can scroll through with there arrow keys
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

namespace StormHead.Models
{
    public class MenuComponent : DrawableGameComponent
    {
        private Scene scene;
        private SpriteBatch spriteBatch;

        private SpriteFont regularFont;
        private SpriteFont hilightFont;
        private Color regularColor = Color.Black;
        private Color hilightColor = Color.Red;
        private string[] menuItems;

        private int selectedIndex;
        private Vector2 position;
        private KeyboardState oldKeyState;

        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        public string[] MenuItems { get => menuItems; set => menuItems = value; }

        public MenuComponent(Scene scene, Vector2 position, SpriteFont regularFont, Color regularColor, SpriteFont hilightFont, Color hilightColor, string[] menuItems) : base(scene.ScenesGame)
        {
            this.scene = scene;
            this.spriteBatch = scene.SpriteBatch;
            this.position = position;
            this.regularFont = regularFont;
            this.regularColor = regularColor;
            this.hilightFont = hilightFont;
            this.hilightColor = hilightColor;
            this.menuItems = menuItems;
        }

        public string GetSelectedItem()
        {
            //Return the selected item
            return menuItems[selectedIndex];
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //If the down key is down and was up before
            if (ks.IsKeyDown(Keys.Down) && oldKeyState.IsKeyUp(Keys.Down))
            {
                //Add one to the selected index
                selectedIndex++;
                //If the selected index is past the end, put it at the start
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = 0;
            }

            //If the up key is down and was up before
            if (ks.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
            {
                //Remove one from the selected index
                selectedIndex--;
                //If the selected index is past the start, put it at the end
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Length - 1;
            }

            //Save the old key state
            oldKeyState = ks;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Position to darw the menu item at
            Vector2 tempPos = position;

            spriteBatch.Begin();

            //Loop through all the menu items
            for (int i = 0; i < menuItems.Length; i++)
            {
                //If the current item is selected
                if (selectedIndex == i)
                {
                    //Draw it as hilighted
                    spriteBatch.DrawString(hilightFont, menuItems[i], tempPos, hilightColor);
                    //Add the height of the string to the temp pos
                    tempPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    //Draw the item as normal
                    spriteBatch.DrawString(regularFont, menuItems[i], tempPos, regularColor);
                    //Add the height of the string to the temp pos
                    tempPos.Y += regularFont.LineSpacing;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
