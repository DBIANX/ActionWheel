using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

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

            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.
            string[] lines = { "1="+txtFirstHotkey.Text, "2="+txtSecondHotkey.Text };
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(@"Configs.txt", lines);

            MessageBox.Show("Settings saved");
        }
    }
}
