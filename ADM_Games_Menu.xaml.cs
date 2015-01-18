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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class ADM_Games_Menu : Window
    {
        public ADM_Games_Menu()
        {
            InitializeComponent();
        }

        private string getGameMode()
        {
            if (gameModeComboBox.Text == "ניסוי")
                return "experiment";
            else
                return "tutorial";


        }

        //Buttons listeners
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Robot_game dm = new Robot_game(getGameMode(), Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                dm.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הרובוט, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Sky_game sg = new Sky_game(getGameMode(), Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק השמיים, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Treasure_map sg = new Treasure_map(getGameMode(), "Treasure map game", Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק מפת האוצר, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Treasure_map sg = new Treasure_map(getGameMode(), "Treasure map reverse game", Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק מפת האוצר ההפוכה, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                Castle_game sg = new Castle_game(getGameMode(), Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הטירה, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            //Return to the main menu
            ADM_menu menu = new ADM_menu();
            this.Close();
            menu.Show();

        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            try
            {
                Village_game sg = new Village_game("Village game location",getGameMode(), Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
                this.Close();
                sg.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("בעיה חלה במשחק הכפר - מיקום, לפרטים נוספים פנה למנהל המערכת");
            }
        }

        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            try
            {
                Village_game sg = new Village_game("Village game identity",getGameMode(), Convert.ToInt32(StageLabel.Text), Convert.ToInt32(levelLabel.Text));
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
