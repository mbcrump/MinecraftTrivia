using System;
using MinecraftTrivia.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Storage;
using MinecraftTrivia.Models;
using System.Collections.Generic;
using Windows.ApplicationModel;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace MinecraftTrivia.Views
{
    public sealed partial class MainPage : Page
    {
        int score = 0;
        private List<Question> questions;
        public Random randomNumber;
        public int storeNumber;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 600));
            questions = new List<Question>();
            CreateQuestions();
            UpdateScore();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                if (localSettings.Values["lastScore"] != null)
                {
                    tbHighScore.Text = localSettings.Values["lastScore"].ToString();
                }

                if ((int)localSettings.Values["lastScore"] == 0)
                {
                    localSettings.Values["lastScore"] = 0;
                    score = 0;
                    tbScore.Text = "0";
                    tbHighScore.Text = "0";
                }
            }
            catch
            {
                localSettings.Values["lastScore"] = 0;
                score = 0;
                tbScore.Text = "0";
                tbHighScore.Text = "0";
            }
        }


        private void CreateQuestions()
        {

            string XMLFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "questions.xml");

            XDocument xDoc = XDocument.Load(XMLFilePath);

            var query = (from x in xDoc.Descendants("quiz").Elements("problem")
                         select new Question
                         {
                             question = x.Element("question").Value,
                             a = x.Element("answerA").Value,
                             b = x.Element("answerB").Value,
                             c = x.Element("answerC").Value,
                             d = x.Element("answerD").Value,
                             correct = x.Element("correct").Value
                         }).ToList();

            questions = query;

        }

        private void btn_start(object sender, RoutedEventArgs e)
        {
            tbStatus.Text = "";

            rdAnswer1.Visibility = Visibility.Visible;
            rdAnswer2.Visibility = Visibility.Visible;
            rdAnswer3.Visibility = Visibility.Visible;
            rdAnswer4.Visibility = Visibility.Visible;

            //reset radiobuttons
            rdAnswer1.IsChecked = false;
            rdAnswer2.IsChecked = false;
            rdAnswer3.IsChecked = false;
            rdAnswer4.IsChecked = false;
            try
            {
                Random randomNumber = new Random();
                int randomNum = randomNumber.Next(0, questions.Count);
                storeNumber = randomNum;
                tbQuestion.Text = questions[randomNum].question;
                rdAnswer1.Content = questions[randomNum].a;
                rdAnswer2.Content = questions[randomNum].b;
                rdAnswer3.Content = questions[randomNum].c;
                rdAnswer4.Content = questions[randomNum].d;

                btnStart.IsEnabled = false;
                btnSubmit.IsEnabled = true;


            }
            catch (Exception)
            {
                tbStatus.Text = "Game Over";
                btnStart.IsEnabled = false;
                btnSubmit.IsEnabled = false;
            }
        }

        private void btn_submit(object sender, RoutedEventArgs e)
        {

            if (getSelectedAnswer().Equals(questions[storeNumber].correct.ToString()))
            {
                tbStatus.Text = "Correct";
                tbStatus.Foreground = new SolidColorBrush(Colors.Green);
                score++;
                tbScore.Text = Convert.ToString(score);
                btnSubmit.IsEnabled = false;
                btnStart.IsEnabled = true;
                btnStart.Content = "Next";
            }

            else
            {
                tbStatus.Text = "Incorrect";
                tbStatus.Foreground = new SolidColorBrush(Colors.Red);
                score--;
                tbScore.Text = Convert.ToString(score);
                btnSubmit.IsEnabled = false;
                btnStart.IsEnabled = true;
                btnStart.Content = "Next";
            }

           
                //only save score if it's the users highest
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
          

                if (score > (int)localSettings.Values["lastScore"])
                {
                    localSettings.Values["lastScore"] = score;
                    tbHighScore.Text = score.ToString();
                }
          
           

        }

        string getSelectedAnswer()
        {
            if (rdAnswer1.IsChecked == true)
                return rdAnswer1.Content.ToString();
            if (rdAnswer2.IsChecked == true)
                return rdAnswer2.Content.ToString();
            if (rdAnswer3.IsChecked == true)
                return rdAnswer3.Content.ToString();
            if (rdAnswer4.IsChecked == true)
                return rdAnswer4.Content.ToString();
            return "";
        }

    }
}
