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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class  Login: Window
    {

        //Global params
        BLL_ms bll_ms;
        public Login()
        {
            InitializeComponent();
            bll_ms = new BLL_ms();

            /////////////////temppp
            //Exam_menu menu = new Exam_menu(userNameTextBox.Text, "first entry");
            //this.Close();
            //menu.Show();
        }

        //Submit to the application
        private void Submit(object sender, RoutedEventArgs e)
        {


            try
            {
                if (bll_ms.CheckUserCredentials(userNameTextBox.Text, passwordTextBox.Password) == true)//Check if the credentials are valid
                {
                    string privilege = bll_ms.CheckUserPrivileges(userNameTextBox.Text);
                    if (privilege == "מנהל מערכת")//participant user, not administrator
                    {
                        ADM_menu menu = new ADM_menu();
                        this.Close();
                        menu.Show();
                    }
                    else if (privilege == "נבדק")//participant user, not administrator
                    {
                        Exam_menu menu = new Exam_menu(userNameTextBox.Text, "first entry");
                        this.Close();
                        menu.Show();
                    }
                    else
                    {
                        MessageBox.Show("יש בעיה עם פרטי המשתמש, פנה למנהל המערכת", "שגיאה");
                    }


                }
                else
                {
                    MessageBox.Show(".שם המשתמש או סיסמה אינם נכונים", "שגיאה");
                }
            }
            catch (Exception)
            {
                 MessageBox.Show("יש בעיה עם ההתחברות למערכת, פנה למנהל המערכת", "שגיאה");
            }
        }




    }
}
