using CamcoManufacturing.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CamcoManufacturing
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
        private void GotoMCSPage(object sender, RoutedEventArgs e)
        {
            Views.ManufacturingCheckSheet view = new Views.ManufacturingCheckSheet();
            view.ShowDialog();
        }

        private void Sheet_Click(object sender, RoutedEventArgs e)
        {
            SetUpSheet obj = new SetUpSheet();
            obj.ShowDialog();
        }

        private void MCS_Click(object sender, RoutedEventArgs e)
        {
            Views.ManufacturingCheckSheet view = new Views.ManufacturingCheckSheet();
            view.ShowDialog();
        }

        private void ViewODSDrawing(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(View_ODSDrawing)))
            {
                View_ODSDrawing obj = new View_ODSDrawing();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(View_ODSDrawing));


            }
        }

        private void EditODSDrawing(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Edit_ODSDrawing)))
            {
                Edit_ODSDrawing obj = new Edit_ODSDrawing();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(Edit_ODSDrawing));


            }
        }

        private void CreateODSDrawing(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(CreateNew_ODSDrawing)))
            {
                CreateNew_ODSDrawing obj = new CreateNew_ODSDrawing();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(CreateNew_ODSDrawing));


            }
        }

        private void ViewSetupSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(ListView)))
            {
                ListView obj = new ListView();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(ListView));
            }
        }

        private void EditSetupSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Edit_SetupSheet)))
            {
                Edit_SetupSheet obj = new Edit_SetupSheet();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(Edit_SetupSheet));


            }
        }

        private void CreateSetupSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(SetUpSheet)))
            {
                SetUpSheet obj = new SetUpSheet();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(SetUpSheet));
            }
        }

        private void ViewCheckSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(View_CheckSheet)))
            {
                View_CheckSheet obj = new View_CheckSheet();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(View_CheckSheet));


            }
        }

        private void EditCheckSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(Edit_CheckSheet)))
            {
                Edit_CheckSheet obj = new Edit_CheckSheet();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(Edit_CheckSheet));


            }
        }

        private void CreateCheckSheet(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen(typeof(CreateNew_CheckSheet)))
            {
                CreateNew_CheckSheet obj = new CreateNew_CheckSheet();
                obj.ShowDialog();
            }
            else
            {
                activateWindow(typeof(CreateNew_CheckSheet));


            }
        }

        
    }
}
