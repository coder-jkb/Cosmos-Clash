using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Cosmos_Clash
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer launchSound = new WindowsMediaPlayer();
        WindowsMediaPlayer nextlevelSound = new WindowsMediaPlayer();
        WindowsMediaPlayer gameSound = new WindowsMediaPlayer();
        WindowsMediaPlayer playerShootSound = new WindowsMediaPlayer();
        WindowsMediaPlayer enemyShootSound = new WindowsMediaPlayer();
        WindowsMediaPlayer enemyDiesSound = new WindowsMediaPlayer();
        WindowsMediaPlayer playerDiesSound = new WindowsMediaPlayer();

        PictureBox[] stars;
        PictureBox[] munitions;
        PictureBox[] enemyMunitions;
        PictureBox[] enemies;

        int bgspeed;
        int playerspeed;
        int enemyspeed;
        int munitionspeed;
        int enemymunitionspeed;
        int enemyNum;
        int score;
        int level;
        int difficulty; // number of enemies being able to shoot
        string enemyURL;
        string gameState;
        
        Random random;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e){
            ScoreLabel.Text = "Score:\n0";
            LevelLabel.Text = "Level:\n0";
            ScoreLabel.Visible = true;
            gameState = "play";
            score = 0;
            level = 1;
            difficulty = 9;
            bgspeed = 4;
            playerspeed = 15;
            enemyspeed = 4;
            munitionspeed = 30;
            enemymunitionspeed = 15;
            stars = new PictureBox[12];
            munitions = new PictureBox[3];
            enemyMunitions = new PictureBox[10];
            enemies = new PictureBox[10];
            random = new Random();

            Image munition = Image.FromFile("assets\\munitions.png");
            Image enemymunition = Image.FromFile("assets\\munitions2.png");
            launchSound.URL = "sounds\\launch.mp3";
            gameSound.URL = "sounds\\mainloop.mp3";
            playerShootSound.URL = "sounds\\player-shot.wav"; 
            enemyShootSound.URL = "sounds\\enemy-shot.wav";
            nextlevelSound.URL = "sounds\\next-level.wav";
            enemyDiesSound.URL = "sounds\\explosion.mp3";
            playerDiesSound.URL = "sounds\\player-died.wav";

            gameSound.settings.setMode("loop", true);

            gameSound.settings.volume = 20;

            playerShootSound.settings.volume = 10;
            enemyShootSound.settings.volume = 2;
            launchSound.settings.volume = 15;
            nextlevelSound.settings.volume = 25;
            enemyDiesSound.settings.volume = 5;
            playerDiesSound.settings.volume = 15;

            gameSound.controls.play();
            launchSound.controls.play();

            for (int i=0; i < enemies.Length; i++) {
                enemyNum = random.Next(1, 9); // enemies1.png to enemies8.png
                enemyURL = $"assets\\enemy{enemyNum}.png";

                enemies[i] = new PictureBox();
                int rsize = random.Next(60, 80);
                enemies[i].Size = new Size(rsize, rsize);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);

                enemies[i].Location = new Point(((i + 1) * 95) - 40, -50);
                enemies[i].Image = Image.FromFile(enemyURL);
            }

            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 40);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            for (int i = 0; i < enemyMunitions.Length; i++)
            {
                enemyMunitions[i] = new PictureBox();
                enemyMunitions[i].Size = new Size(6, 30);
                enemyMunitions[i].Image = enemymunition;
                enemyMunitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemyMunitions[i].BorderStyle = BorderStyle.None;
                enemyMunitions[i].Visible = false;
                int ran_enemy = random.Next(0, 10);
                enemyMunitions[i].Location = new Point(enemies[ran_enemy].Location.X,
                                                       enemies[ran_enemy].Location.Y - 20);
                this.Controls.Add(enemyMunitions[i]);
            }

            for (int i = 0; i < stars.Length; i++){
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(random.Next(20, 1000), random.Next(-10, 600));
                if (i % 2 == 1){ 
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.Wheat;
                } else{
                    stars[i].Size = new Size(4,4);
                    stars[i].BackColor = Color.DarkGray;
                }
                this.Controls.Add(stars[i]);
            }
        }
        private void MoveBGTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++){
                stars[i].Top += bgspeed;
                if (stars[i].Top >= this.Height) {
                    stars[i].Top = -stars[i].Height;
                }
            }
            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += bgspeed-2;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }
        private void LeftMoveTimer_Tick(object sender, EventArgs e){
            if (Player.Left > 10){Player.Left -= 2*playerspeed;}
            else{
                Player.Left = 980;
            }
        }
        private void RightMoveTimer_Tick(object sender, EventArgs e){
            if (Player.Left < 960){ Player.Left += 2*playerspeed; }
            else {
                Player.Left = 10;
            }
        }
        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top >= 250) { Player.Top -= playerspeed; }
        }
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top >= 200 && Player.Top <= 520) { Player.Top += playerspeed; }
        }
        private void pictureBox1_Click(object sender, EventArgs e){}
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState == "play"){
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                {
                    UpMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                {
                    DownMoveTimer.Start();
                }
            }

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Escape)
            {
                PlayPause();
            }

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }
        private void MoveMunitionsTimer_Tick(object sender, EventArgs e)
        {
            playerShootSound.controls.play();
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 50)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= munitionspeed;
                }else {
                    //this.Controls.Remove(munitions[i]);
                    munitions[i].Visible=false;
                    munitions[i].Location = new Point(Player.Location.X+33, Player.Location.Y-i*30);
                }
            }
        }
        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemyspeed);
        }
        private void MoveEnemies(PictureBox[] arr, int speed)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Visible = true;
                arr[i].Top += speed + random.Next(0,5) ;
                if (arr[i].Top > Height) { arr[i].Location = new Point(((i + 1) * 95) - 40, -100); }
            }
            Collision();
        }
        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                for(int m = 0; m < munitions.Length; m++)
                {
                    if (munitions[m].Bounds.IntersectsWith(enemies[i].Bounds)){
                        // working with score, level and difficulty
                        score++;
                        LevelLabel.ForeColor = Color.Wheat;
                        ScoreLabel.Text = "Score:\n"+Convert.ToString(score);
                        
                        if (score%15 == 0){
                            nextlevelSound.controls.play();
                            level++;
                            LevelLabel.ForeColor = Color.Gold;
                            LevelLabel.Text = "Level:\n" + Convert.ToString(level);
                            if (enemyspeed <= 10 && enemymunitionspeed <= 25 && difficulty >= 0) {
                                difficulty--;
                                enemyspeed++;
                                enemymunitionspeed++;
                            }
                            if (level == 10)
                            {
                                GameOver("Cosmos Clash\nWell Played!");
                            }

                        }



                        enemyDiesSound.controls.play();
                        int ran_enemy = random.Next(1, 9); // enemies1.png to enemies8.png
                        enemyURL = $"assets\\enemy{ran_enemy}.png";
                        enemies[i].Image = Image.FromFile(enemyURL);
                        enemies[i].Location = new Point(((i + 1) * 95) - 40, -100);
                    }
                    if (Player.Bounds.IntersectsWith(enemies[i].Bounds)){
                        playerDiesSound.controls.play();
                        Player.Visible = false;
                        GameOver("Cosmos Clash\nGAME OVER");
                    }
                    munitions[m].Visible = false;
                }

                for (int m = 0; m < enemyMunitions.Length; m++)
                {
                    if (enemyMunitions[m].Bounds.IntersectsWith(Player.Bounds))
                    {
                        enemyMunitions[m].Visible = false;
                        playerDiesSound.controls.play();
                        Player.Visible = false;
                        GameOver("Cosmos Clash\nGAME OVER");
                    }
                }
            }
        }
        private void Timers(string state)
        {
            if (state.ToLower() == "stop")
            {
                MoveBGTimer.Stop();
                MoveEnemiesTimer.Stop();
                MoveMunitionsTimer.Stop();
                EnemiesMunitionTimer.Stop();
                LeftMoveTimer.Stop();
                RightMoveTimer.Stop();
                UpMoveTimer.Stop();
                DownMoveTimer.Stop();
            }
            if (state.ToLower() == "start")
            {
                MoveBGTimer.Start();
                MoveEnemiesTimer.Start();
                MoveMunitionsTimer.Start();
                EnemiesMunitionTimer.Start();
                LeftMoveTimer.Start();
                RightMoveTimer.Start();
                UpMoveTimer.Start();
                DownMoveTimer.Start();
            }
        }
        private void PlayPause() {

            if (gameState == "play")
            {
                ScoreLabel.Visible = false;
                LevelLabel.Visible = false;
                label.Visible = true;
                label.Text = "Cosmos Clash\nPAUSED";
                pauseplayinfo.Visible = true;
                pauseplayinfo.Text = "PLAY [SPACE / ESC]";
                pauseplayinfo.Location = new Point(370, 350);
                pauseplayinfo.Font = new Font("Audiowide", 20);
                gameSound.controls.stop();
                gameState = "pause";
                Timers("Stop");
            }
            else if (gameState == "pause")
            {
                ScoreLabel.Visible = true;
                LevelLabel.Visible = true;
                label.Visible = false;
                pauseplayinfo.Location = new Point(10, 10);
                pauseplayinfo.Font = new Font("Audiowide", 12);
                pauseplayinfo.Text = "PAUSE [SPACE / ESC]";
                gameSound.controls.play();
                gameState = "play";
                Timers("Start");
            }
        }
        private void GameOver(string msg)
        {
            label.Visible = true;
            replayBtn.Visible = true;
            exitBtn.Visible = true;
            pauseplayinfo.Visible = false;
            label.Text = msg;
            gameSound.controls.stop();
            gameState = "GameOver";
            Timers("Stop");
        }
        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < enemyMunitions.Length-difficulty; i++)
            {
                if (enemyMunitions[i].Top < this.Height)
                {
                    enemyMunitions[i].Visible = true;
                    enemyMunitions[i].Top += enemymunitionspeed;
                }
                else
                {
                    enemyMunitions[i].Visible = false;
                    int ran_enemy = random.Next(0, 10);
                    enemyMunitions[i].Location = new Point(enemies[ran_enemy].Location.X + 25,
                                                           enemies[ran_enemy].Location.Y + 30);
                }
            }
        }
        private void replayBtn_Click(object sender, EventArgs e){
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
            Timers("Stop");
            this.Controls.Clear(); // play pause worked properly only after 
            InitializeComponent(); // repearting this restart code twice
            Form1_Load(e, e);
        }
        private void exitBtn_Click(object sender, EventArgs e){Environment.Exit(1);}

    }
}