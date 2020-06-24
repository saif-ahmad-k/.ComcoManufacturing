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

namespace CamcoManufacturing.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        public bool IsWindowOpen(Type type)
        {

            foreach (Window w in System.Windows.Application.Current.Windows)
                if (w.GetType().Name == type.Name)

                    return true;
            return false;

        }
        private void activateWindow(Type type)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType().Name == type.Name)
                {
                    if (window.WindowState == WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    window.Activate();

                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(MainWindow)))
            {
                MainWindow obj = new MainWindow();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(MainWindow));


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(View.View_SubCategory)))
            {
                View.View_SubCategory obj = new View.View_SubCategory(0, 0);
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(View.View_SubCategory));
            }
        }

        private void ButtonOperations_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(View.View_Operation)))
            {
                View.View_Operation obj = new View.View_Operation();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(View.View_Operation));
            }
        }
    }
}
