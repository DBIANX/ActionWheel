using System;
using System.IO;
using System.Windows;

namespace RadialMenuDemo
{
    /// <summary>
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            InitializeComponent();
            string[] stringSeparators = new string[] { "=" };
            TextReader tr = new StreamReader(@"Configs.txt");

            string[] lineas = new string[] {"","" };
            
            using (StreamReader sr = new StreamReader(@"Configs.txt"))
            {
                int i = 0;
                while (sr.Peek() >= 0)
                {
                    lineas[i] = sr.ReadLine();
                    i++;
                }
            }
            
            string[] partshk1 = new string[] { };
            partshk1 = lineas[0].Split(stringSeparators, StringSplitOptions.None);

            string[] partshk2 = new string[] { };
            partshk2 = lineas[1].Split(stringSeparators, StringSplitOptions.None);

            txtFirstHotkey.Text = partshk1[1].ToString();
            txtSecondHotkey.Text = partshk2[1].ToString();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string path = @"Configs.txt";
            File.WriteAllText(path, String.Empty);

            string[] lines = { "1="+txtFirstHotkey.Text, "2="+txtSecondHotkey.Text };
           
            System.IO.File.WriteAllLines(@"Configs.txt", lines);

            MessageBox.Show("Settings saved. Restart the program for the changes to take effect.", "Action Wheel", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
