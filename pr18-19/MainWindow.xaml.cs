using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using pr18_19.database;

namespace pr18_19
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            Data.asstoy = null;
            AddRed ar = new AddRed();
            ar.Owner = this;
            ar.ShowDialog();
            LoadDBInDataGrid();
        }

        private void ReClick(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItems != null)
            {
                Data.asstoy = (Asstoy)dg.SelectedItem;
                AddRed ar = new AddRed();
                ar.Owner = this;
                ar.WindowAddEdit.Title = "Редактирование записи";
                ar.btnadded.Content = "Редактировать";
                ar.ShowDialog();
                LoadDBInDataGrid();
            }
        }

        private void DelClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Asstoy row = (Asstoy)dg.SelectedItem;
                    if (row != null)
                    {
                        using (ToyshopContext _db = new ToyshopContext())
                        {
                            _db.Asstoys.Remove(row);
                            _db.SaveChanges();
                        }

                        LoadDBInDataGrid();
                    }
                }

                catch
                {
                    MessageBox.Show("Ошибка удаления");
                }
            }
            else dg.Focus();
        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Создать однотабличную базу данных, в которой отобразятся сведения об ассортименте магазина игрушек.\r\n Разработать к ней интерфейс для работы пользователя.\r\n Выполнила студентка группы ИСП-31 Кулькова Ангелина");
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void LoadDBInDataGrid()
        {
            using (ToyshopContext _db = new ToyshopContext())
            {
                int SelectedIndex = dg.SelectedIndex;
                _db.Asstoys.Load();
                dg.ItemsSource = _db.Asstoys.ToList();
                if (SelectedIndex != -1)
                {
                    if (SelectedIndex == dg.Items.Count) SelectedIndex--;
                    dg.SelectedIndex = SelectedIndex;
                    dg.ScrollIntoView(dg.SelectedItem);
                }
                dg.Focus();
            }
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            autorize f = new autorize();
            f.ShowDialog();
            if(Data.Login == false) Close();
            if (Data.Right == "Администратор" || Data.Right == "Системный администратор") ;
            else if (Data.Right == "Помощник администратора")
            {
                btndel.IsEnabled = false;
            }
            else
            {
                btnadd.IsEnabled = false;
                btnch.IsEnabled = false;
                btndel.IsEnabled = false;
            }
            mainWindow.Title = mainWindow.Title + " " + Data.UserSurname + " " + Data.UserName + "("+ Data.Right +")";
            LoadDBInDataGrid();
        }

        private void SeeClick(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItems != null)
            {
                Data.asstoy = (Asstoy)dg.SelectedItem;
                AddRed ar = new AddRed();
                ar.Owner = this;
                ar.cbage.IsEnabled= false;
                ar.cbcity.IsEnabled = false;
                ar.cbfabrica.IsEnabled = false;
                ar.tbcost.IsEnabled = false;
                ar.tbkolvo.IsEnabled = false;
                ar.tbtoy.IsEnabled = false;
                ar.WindowAddEdit.Title = "Просмотр записи";
                ar.btnadded.IsEnabled = false;
                ar.ShowDialog();
                LoadDBInDataGrid();
            }
        }

        private void DelzaprClick(object sender, RoutedEventArgs e)
        {
            if (drb1.IsChecked == true || drb2.IsChecked == true || drb3.IsChecked == true)
            {
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (drb1.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Cost >= 19999).ExecuteDelete();
                        LoadDBInDataGrid();
                        MessageBox.Show("Были удалены записи, у которых стоимость больше или равна 19999");
                    }
                }
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (drb2.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Kolvo < 5).ExecuteDelete();
                        LoadDBInDataGrid();
                        MessageBox.Show("Были удалены записи, у которых количество меньше 5");
                    }
                }
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (drb3.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Toy == "лего дом").ExecuteDelete();
                        LoadDBInDataGrid();
                        MessageBox.Show("Были удалены записи, у которых название лего дом");
                    }
                }
            }
            else { MessageBox.Show("Выберите номер снизу"); }
        }

        private void NewzaprClick(object sender, RoutedEventArgs e)
        {
            if (nrb1.IsChecked == true || nrb2.IsChecked == true || nrb3.IsChecked == true)
            {
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (nrb1.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Fabrica == "СлаймТаун")
                        .ExecuteUpdate(s => s.SetProperty(p => p.City, "Коломна"));
                        LoadDBInDataGrid();
                        MessageBox.Show("Были обновлены записи, у которых фабрика СлаймТаун - город стал Коломна");
                    }
                }
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (nrb2.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Kolvo >= 700)
                        .ExecuteUpdate(s => s.SetProperty(p => p.Cost, 359));
                        LoadDBInDataGrid();
                        MessageBox.Show("Были обновлены записи, у которых количество больше 700 - стоимость стала 359");
                    }
                }
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (nrb3.IsChecked == true)
                    {
                        _db.Asstoys.Where(p => p.Toy == "Венера")
                        .ExecuteUpdate(s => s.SetProperty(p => p.Age, 12));
                        LoadDBInDataGrid();
                        MessageBox.Show("Были обновлены записи, у которых название Венера - возрастные ограничения стали 12+");
                    }
                }
            }
            else { MessageBox.Show("Выберите номер снизу"); }
        }
        private void viborzaprClick(object sender, RoutedEventArgs e)
        {
            if(vrb1.IsChecked == true || vrb2.IsChecked == true || vrb3.IsChecked == true || vrb4.IsChecked == true || vrb5.IsChecked == true)
            {
                using (ToyshopContext _db = new ToyshopContext())
                {
                    if (vrb1.IsChecked == true)
                    {
                        var nazv = from Asstoy in _db.Asstoys where Asstoy.Toy.StartsWith("В") && Asstoy.Toy.EndsWith("а") select Asstoy;
                        dg.ItemsSource = nazv.ToList();
                        MessageBox.Show("Были выбраны записи, начинающиеся на В и заканчивающиеся на а");
                    }
                    if (vrb2.IsChecked == true)
                    {
                        var cost = from Asstoy in _db.Asstoys where Asstoy.Cost < 1000 select Asstoy;
                        dg.ItemsSource = cost.ToList();
                        MessageBox.Show("Были выбраны записи, в которых стоимость меньше 1000");
                    }
                    if (vrb3.IsChecked == true)
                    {
                        var kolvo = from Asstoy in _db.Asstoys where Asstoy.Kolvo > 10 select Asstoy;
                        dg.ItemsSource = kolvo.ToList();
                        MessageBox.Show("Были выбраны записи, в которых количество игрушек больше 10");
                    }
                    if (vrb4.IsChecked == true)
                    {
                        var age = from Asstoy in _db.Asstoys where Asstoy.Age == 6 select Asstoy;
                        dg.ItemsSource = age.ToList();
                        MessageBox.Show("Были выбраны записи, у которых возрастные ограничения равны 6+");
                    }
                    if (vrb5.IsChecked == true)
                    {
                        var city = from Asstoy in _db.Asstoys where Asstoy.City.Contains("я") select Asstoy;
                        dg.ItemsSource = city.ToList();
                        MessageBox.Show("Были выбраны записи, в которых город содержит букву я");
                    }
                }
            }
            else {MessageBox.Show("Выберите номер снизу"); }
        }
        private void drb1C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут удалены записи, у которых стоимость больше или равна 19999");
        }
        private void drb2C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут удалены записи, у которых количество меньше 5");
        }
        private void drb3C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут удалены записи, у которых название лего дом");
        }
        private void vrb1C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут выбраны записи, начинающиеся на В и заканчивающиеся на а");
        }
        private void vrb2C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут выбраны записи, в которых стоимость меньше 1000");
        }
        private void vrb3C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут выбраны записи, в которых количество игрушек больше 10");
        }
        private void vrb4C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут выбраны записи, у которых возрастные ограничения равны 6+"); ;
        }
        private void vrb5C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут выбраны записи, в которых город содержит букву я");
        }
        private void nrb1C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут обновлены записи, у которых фабрика СлаймТаун - город стал Коломна");
        }
        private void nrb2C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут обновлены записи, у которых количество больше 700 - стоимость стала 359");
        }
        private void nrb3C(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Будут обновлены записи, у которых название Венера - возрастные ограничения стали 12+");
        }
    }
}