/*  Program: TextureHolder.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: Used to save and load text from a file
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StormHead.Handlers
{
    public class FileHandler
    {
        /// <summary>
        /// Saves text to a new file
        /// </summary>
        /// <param name="fileName">File name and path of the file to save</param>
        /// <param name="textToSave">The text to save to the file</param>
        public static void SaveFile(string fileName, string textToSave)
        {
            //Check if the file already exist
            if (!File.Exists(fileName))
            {
                //If the file does not exist then make the file
                using (File.CreateText(fileName)) { }
            }

            //Write the text to the file
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine(textToSave);
            }
        }

        /// <summary>
        /// Loads the text from a file
        /// </summary>
        /// <param name="fileName">File name and path of the file to load</param>
        /// <returns>The text from the file</returns>
        public static string LoadFile(string fileName)
        {
            string returnString = "";

            if (File.Exists(fileName))
            {
                //Read the file
                using (StreamReader reader = new StreamReader(fileName))
                {
                    //Read each line and add it to the return string
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        returnString += $"{line}\n";
                    }
                }
            }

            return returnString.Trim('\n');
        }
    }
}
