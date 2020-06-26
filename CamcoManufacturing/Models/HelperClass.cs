using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CamcoManufacturing
{
    public static class HelperClass
    {
        public static bool IsWindowOpen(Type type)
        {

            foreach (Window w in System.Windows.Application.Current.Windows)
                if (w.GetType().Name == type.Name)

                    return true;
            return false;

        }
        public static void activateWindow(Type type)
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
        public static void ShowWindowPath(Label selectedLabel)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                selectedLabel.Content = selectedLabel.Content + window.Name + " > ";
            }
        }
        public static int ToInteger(this string value)
        {
            int number = 0;
            int.TryParse(value, out number);
            return number;
        }
        public static double ToDouble(this string value)
        {
            try
            {
                double dob = Convert.ToDouble(value);

                return dob;
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static decimal ToDecimal(this string inputString)
        {

            try
            {
                decimal dec = Convert.ToDecimal(inputString);

                return dec;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static float ToFloat(this string inputString)
        {

            try
            {
                float flt = float.Parse(inputString);

                return flt;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static float ToNumeric(this string inputString)
        {

            try
            {
                int temp = Convert.ToInt32(inputString);
                return temp;
            }
            catch (Exception h)
            {
                //  MessageBox.Show("Please provide unit price in numbers only");
                return 0;
            }
        }
    }
}
