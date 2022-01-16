/*  Program: HighScoreForm.cs
 *  
 *  Assignment: Final Project
 *  
 *  Description: 
 *  
 *  Name: Ben Hengeveld
 *  
 *  Revision History:
 *      Ben Hengeveld, 2021.12.08: Created
 */

using StormHead.Handlers;
using StormHead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StormHead.Forms
{
    public partial class HighScoreForm : Form
    {
        private string name;
        private int score;

        public HighScoreForm(int score)
        {
            this.score = score;
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Submits the high-score
            name = txtName.Text;
            HighScore highScore = new HighScore(name, score);
            HighScoreHandler.TryToSaveScore(highScore);
            this.Close();
        }
    }
}
