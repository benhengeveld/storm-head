/*  Program: MenuScene.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Shows a menu for the user
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
using StormHead.Forms;
using StormHead.Handlers;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace StormHead.Scenes
{
    public class MenuScene : Scene
    {
        //The menu component
        private MenuComponent menu;
        private string[] menuItems = { "Start game", "Help", "High Score", "Credits", "Quit" };
        private KeyboardState oldKeyState;
        private Vector2 menuPos = new Vector2(200, 200);
        private SpriteFont menuRegularFont = FontHolder.regularFont;
        private Color menuRegularColor = Color.Black;
        private SpriteFont menuHilightFont = FontHolder.hilightFont;
        private Color menuHilightColor = Color.Red;

        //The title
        private string titleText = "Storm Head";
        private SpriteFont titleFont = FontHolder.gameOverFont;
        private Vector2 titleOffset = new Vector2(0, 15);
        private Color titleColor = Color.Black;

        public MenuScene(Game1 game, Song scenesMusic) : base(game, scenesMusic)
        {
            //Make the menu component
            menu = new MenuComponent(this, menuPos, menuRegularFont, menuRegularColor, menuHilightFont, menuHilightColor, menuItems);
            this.Components.Add(menu);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            //Get the selected item from the menu component
            int selectedIndex = menu.SelectedIndex;
            string selectedItem = menu.GetSelectedItem();
            //If the user can select an menu item and the user is pressing enter
            if (canSelect && ks.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
            {
                //If the menu is enabled
                if (menu.Enabled)
                {
                    //Get what menu item was selected
                    //Start game
                    if (selectedItem == menuItems[0])
                    {
                        //Hide all the scenes then show the level select scene
                        ScenesGame.HideAllScenes();
                        ScenesGame.levelSelectScene.Show();
                    }
                    //Help
                    else if (selectedItem == menuItems[1])
                    {
                        //Hide all the scenes then show the how to play scene
                        ScenesGame.HideAllScenes();
                        ScenesGame.howToPlayScene.Show();
                    }
                    //High score
                    else if (selectedItem == menuItems[2])
                    {
                        //Hide all the scenes then show the high-score scene
                        ScenesGame.HideAllScenes();
                        ScenesGame.highScoresScene.Show();
                    }
                    //Credits
                    else if (selectedItem == menuItems[3])
                    {
                        //Hide all the scenes then show the about scene
                        ScenesGame.HideAllScenes();
                        ScenesGame.aboutScene.Show();
                    }
                    //Quit
                    else if (selectedItem == menuItems[4])
                    {
                        //Exit the program
                        ScenesGame.Exit();
                    }
                }
            }

            //Save the keystate as the old keystate
            oldKeyState = ks;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            //Draw the title
            Vector2 titlePos = new Vector2(ScenesGame.screenSize.X / 2 - titleFont.MeasureString(titleText).X / 2, 0) + titleOffset;
            SpriteBatch.DrawString(titleFont, titleText, titlePos, titleColor);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
