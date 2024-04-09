using Microsoft.EntityFrameworkCore;
using pr18_19.database;
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
using System.Windows.Threading;

namespace pr18_19
{
    /// <summary>
    /// Логика взаимодействия для autorize.xaml
    /// </summary>
    public partial class autorize : Window
    {
        DispatcherTimer _timer;
        int _countLogin = 1;
        public autorize()
        {
            InitializeComponent();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            tbLogin.Focus();
            Data.Login = false;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 10);
            _timer.Tick += new EventHandler(timer_Tick);
        }
        void GetCaptcha()
        {
            string masChar = "НЕАВИЖУКПЧнеавижукпч" + "1234567890";
            string captcha = "";
            Random rnd = new Random();
            for (int i = 1; i <= 6; i++)
            {
                captcha = captcha + masChar[rnd.Next(0, masChar.Length)];
            }
            grid.Visibility = Visibility.Visible;
            Captcha.Text = captcha;
            tbCaptcha.Text = null;
            Captcha.LayoutTransform = new TranslateTransform(-15, 15);
            line.X1 = 5;
            line.Y1 = rnd.Next(10, 40);
            line.X2 = 250;
            line.Y2 = rnd.Next(10, 40);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            stpanel.IsEnabled = true;
        }

        private void EnterClick(object sender, RoutedEventArgs e)
        {
            using (ToyshopContext _db = new ToyshopContext())
            {
                var user = _db.Users.Where(user => user.UserLogin == tbLogin.Text &&
                user.UserPassword == tbPas.Password);
                if (user.Count() == 1 && Captcha.Text == tbCaptcha.Text)
                {
                    Data.Login = true;
                    Data.UserSurname = user.First().UserSurname;
                    Data.UserName = user.First().UserName;
                    Data.UserPatronymic = user.First().UserPatronymic;
                    _db.Roles.Load();
                    Data.Right = user.First().UserRoleNavigation.RoleName;
                    Close();
                }
                else
                {
                    if (user.Count() == 1)
                    {
                        MessageBox.Show("Повторите ввод капчи");
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль неверны. Повторите ввод");
                    }
                    GetCaptcha();
                    if (_countLogin >= 2)
                    {
                        stpanel.IsEnabled = false;
                        _timer.Start();
                    }
                    _countLogin++;
                    tbLogin.Focus();
                }
            }
        }

        private void EscClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GuestClick(object sender, RoutedEventArgs e)
        {
            Data.Login = true;
            Data.UserSurname = "Гость";
            Data.Right = "Пользователь";
            Data.UserPatronymic = "";
            Data.UserName = "";
            Close();
        }
    }
}
