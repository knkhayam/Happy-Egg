using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Egg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }
        SoundPlayer bg = new SoundPlayer();
        SoundPlayer die = new SoundPlayer();

        int fallSpeed = 2; // character fall speed //
        int liftUpSpeed = 2; // lift up speed //
        int moveSpeed = 6; // character left/right move speed //
        int direction = 0; // direction to use left/right for character use only to move left/right//
        int liftWidth = 80; // lift widht // default is 80 //
        int liftHeight = 20; //lift depth // default is 20 //
        int liftFasla = 120; /// aik lift say dusri lift tak fasla //
        int score = 0; /// score //

        Panel[] lift = new Panel[10];
        Random random = new Random();
        private void init()
        {
            bg.SoundLocation = "Resources/background.wav";
            bg.PlayLooping();
            die.SoundLocation = "Resources/die.wav";
            for(int i=0;i<10;i++)
            {
                lift[i] = new Panel();
                lift[i].Width = liftWidth;
                lift[i].Height = liftHeight;
                lift[i].Left = random.Next(0, this.Width - liftWidth);
                lift[i].Top = this.Height / 2 + (i * liftFasla);
                lift[i].BackColor = Color.Black;
                lift[i].Tag = "noScore"; // to use scoring purpose only //
                this.Controls.Add(lift[i]);
            }
            highScoreLabel.Text = "High Score: " + Properties.Settings.Default.highScore.ToString();
        }

        private void checker()
        {
            for (int i = 0; i < 10; i++)
            {
                if (character.Left + character.Width > lift[i].Left && character.Left < lift[i].Left + lift[i].Width)
                {
                    if (character.Top + character.Height > lift[i].Top && character.Top + character.Height < lift[i].Top + lift[i].Height)
                    {
                        character.Top = lift[i].Top - character.Height;
                    }
                }
                if (character.Top + character.Height > lift[i].Top && character.Top + character.Height < lift[i].Top + lift[i].Height)
                {
                    if (lift[i].Tag == "noScore")
                    {
                        score++;
                        int hs = Properties.Settings.Default.highScore;
                        if (score > hs)
                        {
                            highScoreLabel.Text = "High Score: " + score;
                            Properties.Settings.Default.highScore = score;
                            Properties.Settings.Default.Save();
                        }
                        if (score % 10 == 0)
                        {
                            if (score < 30)
                            {
                                liftUpSpeed++;
                                moveSpeed++;
                                fallSpeed++;
                            }
                        }
                        scoreLabel.Text = "Score: " + score;
                        lift[i].Tag = "Score";
                    }
                }
            }
            if (character.Top > this.Height - character.Height + 2 || character.Top < panel1.Height)
            {
                fallTimer.Stop();
                liftTimer.Stop();
                die.Play();

                if (MessageBox.Show("Game Over!\nPlay Again?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ///////////////
                    liftUpSpeed = 2;
                    fallSpeed = 2;
                    moveSpeed = 6;
                    direction = 0;
                    //////////////
                    bg.PlayLooping();
                    score = 0;
                    scoreLabel.Text = "Score: 0";
                    character.Top = character.Height + 10;
                    character.Left = Width / 2;
                    for (int i = 0; i < 10; i++)
                    {
                        lift[i].Top = this.Height / 2 + (i * liftFasla);
                        lift[i].Tag = "noScore";
                    }
                    fallTimer.Start();
                    liftTimer.Start();

                }
                else
                    Close();
            }
            
        }

        private void fallTimer_Tick(object sender, EventArgs e)
        {
            character.Top += fallSpeed;
            if (character.Left < -1)
                character.Left = Width - character.Width;
            if (character.Left + character.Width > Width + 1)
                character.Left = 1;
            if (direction == 1)
                character.Left += moveSpeed;
            else if (direction == -1)
                character.Left -= moveSpeed;
            checker();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
                direction = -1;
            if (e.KeyCode == Keys.Right)
                direction = 1;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                direction = 0;
        }

        private void liftTimer_Tick(object sender, EventArgs e)
        {
            for(int i=0;i<10;i++)
            {
                lift[i].Top -= liftUpSpeed;
                if(lift[i].Top < -liftHeight)
                {
                    lift[i].Top = (liftFasla + liftHeight) * 9;
                    lift[i].Left = random.Next(0, this.Width - liftWidth);
                    lift[i].Tag = "noScore";
                }
            }
        }

     
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startPanel.Hide();
            fallTimer.Start();
            liftTimer.Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string instructions = "Use Arrow Keys Left/Right to move the Egg.\n" +
               "Don't let the egg dropped.\n" +
               "Reach the maximum Score.";
            MessageBox.Show(instructions);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            playToolStripMenuItem_Click(sender, e);
        }
    }
}
