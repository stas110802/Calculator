using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool cleared = true;
        public MainWindow()
        {
            InitializeComponent();
            text.IsReadOnly = true;
            text.Text = "0";
            this.Opacity = 1;
            
            foreach (UIElement uIElement in GroupsOfButtons.Children)
            {
                if(uIElement is Button)
                {
                    ((Button)uIElement).Click += ButtonClick;
                }
            }
        }      
        
        private void ButtonClick(Object sender, RoutedEventArgs routeEvent)
        {
            var textButton = ((Button)routeEvent.OriginalSource).Content.ToString();
            
            // стереть весь текст
            if(textButton == "C")
            {
                text.ClearTextBox(ref cleared, true);
            }
            // стереть 1 символ
            else if (textButton == "x")
            {
                if(text.Text.Length > 0 && text.Text != "0")
                {
                    text.Text = text.Text.Substring(0, text.Text.Length-1);
                    if (text.Text.Length == 0) 
                    {
                        text.ClearTextBox(ref cleared, true);
                    }                   
                }
                else
                {
                    text.Text = "0";
                }
            }
            // посчитать
            else if (textButton == "=")
            {
                try
                {
                    text.Text = new DataTable().Compute(text.Text, null).ToString();
                }
                catch (Exception error)
                {
                    MessageBox.Show($"{error.Message}", "Ошибка при компиляции",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    text.ClearTextBox(ref cleared, true);
                }               
            }
            // добавить  1 символ
            else
            {
                if (cleared)
                {
                    text.Clear();
                    cleared = false;                   
                    text.Text += textButton;
                }
                else
                {
                    text.Check(textButton);
                }
            }
        }        
    }

    

    public static class Utilites
    {
        static string[] operation = { "+", "-", "/", "*",};
        public static void ClearTextBox(this TextBox textBox, ref bool cleared, bool flag)
        {
            textBox.Clear();
            textBox.Text = "0";
            cleared = flag;
        }

        public static void Check(this TextBox textBox, string button)
        {
            foreach (var item in operation)
            {
                if (item == button && button == textBox.Text.Last().ToString())
                {
                    return;
                }
            }
            textBox.Text += button;
        }

    }
}
