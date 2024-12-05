// Не забыть УБРАТЬ ЛИШНИЕ БИБЛИОТЕКИ
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Simple_DES
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char[] data;
        private int key;

        public MainWindow()
        {
            InitializeComponent();
            Console.OutputEncoding = Encoding.Unicode;    // Кодировка Extended ASCII 0-255
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) 
        { 
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); } 
        }

        private void Button_Click_Shutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_Minimalize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            // Исходные данные запихиваем в массив char
            data = Default_TextBox.Text.ToCharArray();

            // Пытаемся запихнуть ключ в переменную key, если не int то выдаем ошибку
            try { key = int.Parse(Key_TextBox.Text); }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message); }


            string selectedOption = "";
            foreach (var child in myStackPanel.Children)
            {
                if (child is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    selectedOption = radioButton.Content.ToString();
                    break;
                }
            }

            if (selectedOption != "")
            {
                // Очищаем поле вывода
                Result_TextBlock.Text = "";
                bool is_checked;

                switch (selectedOption)
                {
                    case "ECB":
                        is_checked = (bool)CBox.IsChecked;
                        Result_TextBlock.Text = SECB.ECB(is_checked, data, key);
                        break;
                    case "CBC":
                        is_checked = (bool)CBox.IsChecked;
                        Result_TextBlock.Text = SCBC.CBC(is_checked, data, key);
                        break;
                    case "CFB":
                        is_checked = (bool)CBox.IsChecked;
                        Result_TextBlock.Text = SCFB.CFB(is_checked, data, key);
                        break;
                    case "OFB":
                        is_checked = (bool)CBox.IsChecked;
                        Result_TextBlock.Text = SOFB.OFB(is_checked, data, key);
                        break;
                    default: break;
                }
            }
            else
            {
                MessageBox.Show("Никакой из режимов не выбран!");
            }
        }

        private void rBtn4_Checked(object sender, RoutedEventArgs e)
        { CBox.IsEnabled = false; CBox.IsChecked = false; }
        private void OtherRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!CBox.IsEnabled) { CBox.IsEnabled = true; }
        }
    }
}
