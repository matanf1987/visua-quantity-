using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TryWPF
{
    /// <summary>
    /// Interaction logic for Exam_menu.xaml
    /// </summary>
    public partial class Exam_menu : Window
    {

        //Global params
        string _day = "";
        string _username = "";
        BLL_ms bllMS = new BLL_ms();

        //Constructor not in use
        public Exam_menu()
        {
            InitializeComponent();
            _username = "yona";
            _day = bllMS.GetExamineeCurrentDay(_username);
            if (_day == "")//The examinee can't play in this day beacause the date is not prefformed in the database
            {
                invalidScreen();
                return;
            }
            WriteBodyMessage("first entry");
            Updatescreen();
        }


        //Constructor
        public Exam_menu(string username,string status)
        {
            InitializeComponent();
            _username = username;

            try
            {
                _day = bllMS.GetExamineeCurrentDay(_username);

                if (_day == "")//The examinee can't play in this day beacause the date is not prefformed in the database
                {
                    invalidScreen();
                    return;
                }
                WriteBodyMessage(status);
                Updatescreen();
            }
            catch (Exception)
            {
                MessageBox.Show("יש בעיה עם בניית התפריט, פנה למנהל המערכת", "שגיאה");
            }


        }

        //In case the day is not a day the user can play
        private void invalidScreen()
        {

            WriteBodyMessage("day is invalid");
            robotGameTime.Content = "זמן שנותר למשחק: " + "0" + " דקות";
            skyGameTime.Content = "זמן שנותר למשחק: " +  "0" + " דקות";
            tmGameTime.Content = "זמן שנותר למשחק: " + "0" + " דקות";
            tmrGameTime.Content = "זמן שנותר למשחק: " + "0" + " דקות";
            castleGameTime.Content = "זמן שנותר למשחק: " + "0" + " דקות";
            robotGameButton.IsEnabled = false;
            skyGameButton.IsEnabled = false;
            tmGameButton.IsEnabled = false;
            tmrGameButton.IsEnabled = false;
            castleGameButton.IsEnabled = false;
            villageGameLocationButton.IsEnabled = false;
            villageGameIdentityButton.IsEnabled = false;

        }

        //Update the screen information
        private void Updatescreen()
        {

            try
            {
                DailyGamesInformation dgi = bllMS.GetExamineeDailyGamesInformation(_username, _day);

                robotGameTime.Content = "זמן שנותר למשחק: " + dgi.robotGameTimeLeft + " דקות";
                skyGameTime.Content = "זמן שנותר למשחק: " + dgi.skyGameTimeLeft + " דקות";
                tmGameTime.Content = "זמן שנותר למשחק: " + dgi.treasureMapGameTimeLeft + " דקות";
                tmrGameTime.Content = "זמן שנותר למשחק: " + dgi.treasureMapReverseGameTimeLeft + " דקות";
                castleGameTime.Content = "זמן שנותר למשחק: " + dgi.castleGameTimeLeft + " דקות";
                villageGameLocationTime.Content = "זמן שנותר למשחק: " + dgi.villageGameLocationTimeLeft + " דקות";
                villageGameIdentityTime.Content = "זמן שנותר למשחק: " + dgi.villageGameIdentityTimeLeft + " דקות";

                int count = 0;
                if (dgi.robotGameTimeLeft == "0")
                {
                    robotGameButton.IsEnabled = false;
                    count++;
                }
                if (dgi.skyGameTimeLeft == "0")
                {
                    skyGameButton.IsEnabled = false;
                    count++;
                }
                if (dgi.treasureMapGameTimeLeft == "0")
                {
                    tmGameButton.IsEnabled = false;
                    count++;
                }
                if (dgi.treasureMapReverseGameTimeLeft == "0")
                {
                    tmrGameButton.IsEnabled = false;
                    count++;
                }
                if (dgi.castleGameTimeLeft == "0")
                {
                    castleGameButton.IsEnabled = false;
                    count++;
                }
                if (dgi.villageGameLocationTimeLeft == "0")
                {
                    villageGameLocationButton.IsEnabled = false;
                    count++;
                }
                if (dgi.villageGameIdentityTimeLeft == "0")
                {
                    villageGameIdentityButton.IsEnabled = false;
                    count++;
                }

                if (count == 7)
                {
                    WriteBodyMessage("finish the day");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("יש בעיה עם עדכון התפריט, פנה למנהל המערכת", "שגיאה");
            }
        }

        //Write the message to the screen
        private void WriteBodyMessage(string status)
        {


            if (status == "first entry")
            {
                int dayNumber = Convert.ToInt32(_day.Substring(3, _day.Length - 3));
                int daysLeft = 20 - dayNumber;
                body1.Content = _username + " שלום";
                body2.Content = "אתה נמצא ביום " + dayNumber + " נותרו לך רק עוד " + daysLeft + " ימים";
                body3.Content = "יש לנו יום עמוס היום";
                body4.Content = "אלו המשחקים שאותם תשחק היום ואלו הזמנים בהם תשחק את המשחקים הבאים";
                body5.Content = "שים לב, רצוי לשחק כל משחק מתחילתו ועד סופו ואפשר לקחת הפסקה בין המשחקים השונים";
                body6.Content = "אתה יכול לבחור את סדר המשחקים השונים";
                body7.Content = "להתחלה לחץ על המשחק הרצוי";
            }
            else if (status == "game complete")
            {
                body1.Content = " !!!כל הכבוד";
                body2.Content = "נותרו לך עוד משחקים לשחק";
                body3.Content = "שים לב, רצוי לשחק כל משחק מתחילתו ועד סופו ואפשר לקחת הפסקה בין המשחקים השונים";
                body4.Content = "אתה יכול לבחור את סדר המשחקים השונים";
                body5.Content = "להתחלה לחץ על המשחק הרצוי";
                body6.Content = "";
                body7.Content = "";
            }
            else if (status == "finish the day")
            {
                body1.Content = "כל הכבוד על ההשקעה זה היה יום ארוך";
                body2.Content = "נתראה בקרוב";
                body3.Content = "";
                body4.Content = "";
                body5.Content = "";
                body6.Content = "";
                body7.Content = "";
            }
            else if (status == "day is invalid")
            {
                body1.Content = "יום זה אינו רשום במערכת";
                body2.Content = "לשאלות נוספות ניתן לפנות למנהל המערכת";
                body3.Content = "";
                body4.Content = "";
                body5.Content = "";
                body6.Content = "";
                body7.Content = "";
            }
            else if(status == "problem has occurred")
            {
                body1.Content = "חלה שגיאה באפליקציה";
                body2.Content = "לשאלות נוספות ניתן לפנות למנהל המערכת";
                body3.Content = "";
                body4.Content = "";
                body5.Content = "";
                body6.Content = "";
                body7.Content = "";


            }


        }


        //-----------------On click buttons-------------------------//

        private void RobotGameButton(object sender, RoutedEventArgs e)
        {

            try
            {
                Robot_game dm = new Robot_game(_username, _day);
                this.Close();
                dm.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הרובוט, לפרטים נוספים פנה למנהל המערכת");
            }
            
        }

        private void SkyGameButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Sky_game sg = new Sky_game(_username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק השמיים, לפרטים נוספים פנה למנהל המערכת");
            }
            
        }

        private void TMGameButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Treasure_map sg = new Treasure_map("Treasure map game", _username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק מפת האוצר, לפרטים נוספים פנה למנהל המערכת");
            }
           
        }

        private void TMRGameButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Treasure_map sg = new Treasure_map("Treasure map reverse game", _username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק מפת האוצר ההפוכה, לפרטים נוספים פנה למנהל המערכת");
            }
            
        }

        private void CastleGameButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Castle_game sg = new Castle_game(_username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הטירה, לפרטים נוספים פנה למנהל המערכת");
            }
          
        }

        private void VillageGameLocationButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Village_game sg = new Village_game("Village game location",_username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הכפר - מיקום, לפרטים נוספים פנה למנהל המערכת");
            }

        }

        private void VillageGameIdentityButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Village_game sg = new Village_game("Village game identity", _username, _day);
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הכפר - זהות, לפרטים נוספים פנה למנהל המערכת");
            }
          
        }
    }
}
