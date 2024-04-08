using pr18_19;
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
using System.Xml.Linq;

namespace pr18_19
{
    /// <summary>
    /// Логика взаимодействия для AddRed.xaml
    /// </summary>
    public partial class AddRed : Window
    {
      ToyshopContext _db = new ToyshopContext();
        Asstoy _asstoy;
        public AddRed()
        {
            InitializeComponent();
        }

        private void AddEditClick(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (tbtoy.Text.Length == 0) errors.AppendLine("Введите название");
            if (Int32.TryParse(tbcost.Text, out int a) == false|| a<=0) errors.AppendLine("Введите корректную стоимость");
            if (Int32.TryParse(tbkolvo.Text, out a) == false || a <= 0) errors.AppendLine("Введите корректное количество");
            if (cbage.Text != "0" && cbage.Text!="3" && cbage.Text != "6" && cbage.Text != "12" && cbage.Text != "16") errors.AppendLine("Выберите возрастные ограничения");
            if (cbcity.Text != "Волгоград" && cbcity.Text != "Коломна" && cbcity.Text != "Москва" && cbcity.Text != "Нижний Новгород" && cbcity.Text != "Егорьевск" 
                && cbcity.Text != "Рязань") errors.AppendLine("Выберите фабрику");
            if (cbfabrica.Text != "Плюшевая фабрика" && cbfabrica.Text != "Бумажный Человек" && cbfabrica.Text != "Кукольный завод" && cbfabrica.Text != "ПластиДом" 
                && cbfabrica.Text != "СлаймТаун" && cbfabrica.Text != "Воздух" && cbfabrica.Text != "Лего") errors.AppendLine("Выберите город");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (Data.asstoy == null)
                {
                    _db.Asstoys.Add(_asstoy);
                    _db.SaveChanges();
                }
                else
                {
                    _db.SaveChanges();
                }
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Data.asstoy == null)
            {
                WindowAddEdit.Title = "Добавление записи";
                btnadded.Content = "Добавить";
                tbtoy.IsEnabled = true;
                _asstoy = new Asstoy();
            }
            else
            {
                _asstoy = _db.Asstoys.Find(Data.asstoy.Toy);
            }
            WindowAddEdit.DataContext = _asstoy;
        }
    }
}
