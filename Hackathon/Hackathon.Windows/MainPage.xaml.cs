// Adrian Sypos

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.UI;
using Windows.System.Threading;

namespace Hackathon
{
    public sealed partial class MainPage : Page
    {
        #region Initialize localSettings and create timers
        public ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer moveTimer = new DispatcherTimer();
        #endregion

        #region initialize variables
        int gameTimerTick;
        int moveTimerTick;
        public int savedScore;
        public int blueClicked = 0;
        public int redClicked = 0;
        public int greenClicked = 0;
        public int greenCorrect = 0;
        public int redCorrect = 0;
        public int blueCorrect = 0;
        public int correctCnt = 0;
        public int wrongCnt = 0;
        public int score = 0;
        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            this.randomOption();

            redBlock.IsEnabled = false;
            blueBlock.IsEnabled = false;
            greenBlock.IsEnabled = false;

            #region create gameTimer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            gameTimer.Tick += gameTimer_Tick;
            #endregion

            #region create moveTimer
            moveTimer = new DispatcherTimer();
            moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            moveTimer.Tick += moveTimer_Tick;
            #endregion

            #region localSettings
            //check if localSetting variable called bestScore already exist or not
            if (localSettings.Values.Keys.Contains("bestScore") == false)
            {
                //if doesnt exist create one and initialize it to 0
                localSettings.Values["bestScore"] = 0;
            }
            else
            {
                //if exists read it from localSettings
                localSettings.Values["bestScore"] = localSettings.Values["bestScore"];
            }
            
            bestScoreTxtBlock.Text = "Best score: " + localSettings.Values["bestScore"].ToString();
            #endregion
        }

        private void gameTimer_Tick(object sender, object e)
        {
            if (gameTimerTick <= 0)
            {
                lost();
            }
            else
            {
                gameTimerTxtBlock.Text = gameTimerTick--.ToString() + " ms";
            }
        }

        private void moveTimer_Tick(object sender, object e)
        {
            moveTimerTick++;
        }

        #region BlueBlock, GreenBlock and RedBlock CLICK EVENT
        private void blueBlock_Click(object sender, RoutedEventArgs e)
        {
            blueClicked++;
            if (validate() == true)
            {
                score++;
                correctCnt++;
                scoreTxtBlock.Text = "Score: " + score;
            }
            else
            {
                wrongCnt++;
                wrongTxtBlock.Text = "Mistakes: " + wrongCnt;
                if (wrongCnt == 3)
                {
                    lost();
                }
            }
            randomOption();
            blueClicked = 0;
        }

        private void greenBlock_Click(object sender, RoutedEventArgs e)
        {
            greenClicked++;
            if (validate() == true)
            {
                score++;
                correctCnt++;
                scoreTxtBlock.Text = "Score: " + score;
            }
            else
            {
                wrongCnt++;
                wrongTxtBlock.Text = "Mistakes: " + wrongCnt;
                if (wrongCnt == 3)
                {
                    lost();
                }

            }
            randomOption();
            greenClicked = 0;
        }

        private void redBlock_Click(object sender, RoutedEventArgs e)
        {
            redClicked++;
            if (validate().Equals(true))
            {
                score++;
                correctCnt++;
                scoreTxtBlock.Text = "Score: " + score;
            }
            else
            {
                wrongCnt++;
                wrongTxtBlock.Text = "Mistakes: " + wrongCnt;
                if (wrongCnt == 3)
                {
                    lost();
                }

            }
            randomOption();
            redClicked = 0;
        }
        #endregion

        public void randomOption()
        {
            moveTimerTick = 0;
            moveTimer.Start();

            redCorrect = 0;
            blueCorrect = 0;
            greenCorrect = 0;
            Random rnd = new Random();
            int backgroundColor, foregroundColor, answer;
            backgroundColor = rnd.Next(1, 4);
            foregroundColor = rnd.Next(1, 4);
            answer = rnd.Next(1, 4);

            if (backgroundColor == foregroundColor)
            {
                do
                {
                    backgroundColor = rnd.Next(1, 4);
                } while (backgroundColor == foregroundColor);
            }

            switch (backgroundColor)
            {
                case 1:
                    clickOnButton.Background = new SolidColorBrush(Colors.Red);
                break;

                case 2:
                    clickOnButton.Background = new SolidColorBrush(Colors.Blue);
                break;

                case 3:
                    clickOnButton.Background = new SolidColorBrush(Colors.Green);  
                break;
            }

            switch (foregroundColor)
            {
                case 1:
                    clickOnButton.Foreground = new SolidColorBrush(Colors.Red);
                    break;

                case 2:
                    clickOnButton.Foreground = new SolidColorBrush(Colors.Blue);
                    break;

                case 3:
                    clickOnButton.Foreground = new SolidColorBrush(Colors.Green);
                    break;
            }

            switch (answer)
            {
                case 1:
                    clickOnButton.Content = "GREEN";
                    greenCorrect = 1;
                    break;
                case 2:
                    clickOnButton.Content = "BLUE";
                    blueCorrect = 1;
                    break;
                case 3:
                    clickOnButton.Content = "RED";
                    redCorrect = 1;
                    break;
            }
        }

        public Boolean validate()
        {
            if (redClicked == 1 && redCorrect == 1 || blueClicked == 1 && blueCorrect == 1 || greenClicked == 1 && greenCorrect == 1)
            {
                if (moveTimerTick <= 5){
                    moveTimerTickTxtBlock.Text = "+50ms!";
                    gameTimerTick += 50;
                }
                else if (moveTimerTick <= 15){
                    moveTimerTickTxtBlock.Text = "+15ms!";
                    gameTimerTick += 15;
                }
                else {
                    moveTimerTickTxtBlock.Text = "";
                }
                moveTimer.Stop();
                return true;
            }
            else
            {
                moveTimer.Stop();
                return false;
            }
        }

        public void restart()
        {
            checkBestScore();
            redBlock.IsEnabled = false;
            blueBlock.IsEnabled = false;
            greenBlock.IsEnabled = false;

            blueClicked = 0;
            redClicked = 0;
            greenClicked = 0;
            greenCorrect = 0;
            redCorrect = 0;
            blueCorrect = 0;
            correctCnt = 0;
            wrongCnt = 0;
            score = 0;

            scoreTxtBlock.Text = "Score: " + score;
            wrongTxtBlock.Text = "Mistakes: " + wrongCnt;
            gameTimer.Stop();
            gameTimerTxtBlock.Text = "--- ms";
            startButton.Visibility = Visibility.Visible;

            randomOption();
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            restart();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            gameTimerTick = 300;
            restart();
            redBlock.IsEnabled = true;
            blueBlock.IsEnabled = true;
            greenBlock.IsEnabled = true;
            startButton.Visibility = Visibility.Collapsed;
            gameTimer.Start();
        }

        private void lost()
        {
            checkBestScore();
            redBlock.IsEnabled = false;
            greenBlock.IsEnabled = false;
            blueBlock.IsEnabled = false;
            startButton.Content = "Play again";
            startButton.Visibility = Visibility.Visible;
            gameTimerTxtBlock.Text = "--- ms";
            gameTimer.Stop();
            moveTimer.Stop();
            moveTimerTickTxtBlock.Text = "";
        }

        private void checkBestScore()
        {
            savedScore = int.Parse(localSettings.Values["bestScore"].ToString());
            if (score > savedScore)
            {
                localSettings.Values["bestScore"] = score;
                bestScoreTxtBlock.Text = "Best score: " + localSettings.Values["bestScore"].ToString();
            }
        }
    }
}