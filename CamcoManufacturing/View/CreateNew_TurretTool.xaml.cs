﻿using System;
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
    /// Interaction logic for CreateNew_TurretTool.xaml
    /// </summary>
    public partial class CreateNew_TurretTool : Window
    {
        public CreateNew_TurretTool()
        {
            InitializeComponent();
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            if (!HelperClass.IsWindowOpen(typeof(MainWindow)))
            {
                MainWindow obj = new MainWindow();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(MainWindow));
            }
        }

        private void ButtonReturnToCreateSetup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(SetUpSheet)))
            {
                SetUpSheet obj = new SetUpSheet();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(SetUpSheet));
            }
        }

        private void ButtonBump_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(CreateNew_BumpToolHeader)))
            {
                CreateNew_BumpToolHeader obj = new CreateNew_BumpToolHeader();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(CreateNew_BumpToolHeader));
            }
        }
    }
}
