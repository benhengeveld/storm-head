/*  Program: MapLoader.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to load maps, to get walls from a map, to get the players pos in a map, and to get enemys pos in a map
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
using StormHead.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StormHead.Handlers
{
    public class MapLoader
    {
        private GameScene scene;
        private int[,] map;
        private float cubeSize;
        private Vector2 startPos;
        private Texture2D wallTexture;

        public MapLoader(GameScene scene, int[,] map, float cubeSize, Vector2 startPos, Texture2D wallTexture)
        {
            this.scene = scene;
            this.map = map;
            this.cubeSize = cubeSize;
            this.startPos = startPos;
            this.wallTexture = wallTexture;
        }

        /// <summary>
        /// Get all the walls from the map
        /// </summary>
        /// <returns>A list of all the wall</returns>
        public List<Wall> GetWalls()
        {
            List<Wall> walls = new List<Wall>();

            //Loop through everything in the map
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //If the item in the map is 1 for a wall
                    if (map[y,x] == 1)
                    {
                        //Make a new wall at the position with the cube size and add it to the walls list
                        Vector2 newWallPos = new Vector2(x * cubeSize, y * cubeSize) + startPos;
                        Vector2 newWallSize = new Vector2(cubeSize, cubeSize);
                        Wall newWall = new Wall(scene, wallTexture, newWallPos, newWallSize, true);
                        walls.Add(newWall);
                    }
                }
            }

            return walls;
        }

        /// <summary>
        /// Gets the players start position in a map
        /// </summary>
        /// <returns>The start position of the player</returns>
        public Vector2 GetPlayerPosition()
        {
            //Loop through everything in the map
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //If the item in the map is 2 for the player
                    if (map[y, x] == 2)
                    {
                        //Gets the starting position of the player and returns it
                        Vector2 playerPos = new Vector2(x * cubeSize, y * cubeSize) + startPos + new Vector2(cubeSize / 2,
                            cubeSize / 2);
                        return playerPos;
                    }
                }
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Gets the positions of the ghosts in a map
        /// </summary>
        /// <returns>A list of position the ghost are in the map</returns>
        public List<Vector2> GetGhostPositions()
        {
            List<Vector2> enemyPositions = new List<Vector2>();

            //Loop through everything in the map
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //If the item in the map is 3 for a ghost
                    if (map[y, x] == 3)
                    {
                        //Get the position of the ghost and then add it to the list
                        Vector2 newEnemyPos = new Vector2(x * cubeSize, y * cubeSize) + startPos 
                            + new Vector2(cubeSize / 2, cubeSize / 2);
                        enemyPositions.Add(newEnemyPos);
                    }
                }
            }

            return enemyPositions;
        }

        /// <summary>
        /// Gets the positions of the laser ghosts in a map
        /// </summary>
        /// <returns>A list of position the laser ghost are in the map</returns>
        public List<Vector2> GetLaserEnemiesPositions()
        {
            List<Vector2> enemyPositions = new List<Vector2>();

            //Loop through everything in the map
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //If the item in the map is 4 for a laser ghost
                    if (map[y, x] == 4)
                    {
                        //Get the position of the laser ghost and then add it to the list
                        Vector2 newEnemyPos = new Vector2(x * cubeSize, y * cubeSize) + startPos 
                            + new Vector2(cubeSize / 2, cubeSize / 2);
                        enemyPositions.Add(newEnemyPos);
                    }
                }
            }

            return enemyPositions;
        }
    }
}
