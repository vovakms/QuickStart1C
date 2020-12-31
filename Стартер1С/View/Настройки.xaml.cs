using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Стартер1С
{
     
    public partial class Настройки : Window
    {
        public Настройки()
        {
            InitializeComponent();
                       
           
            txtBox1.Text = Properties.Settings.Default.pathBin1c;
            txtBox2.Text = Properties.Settings.Default.path1Csestart;
            
        }

        private void but1_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                txtBox1.Text = folderBrowser.SelectedPath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                txtBox2.Text = folderBrowser.SelectedPath;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.pathBin1c = txtBox1.Text;
            Properties.Settings.Default.path1Csestart = txtBox2.Text;
            Properties.Settings.Default.Save();

            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pathBin1c = txtBox1.Text;
            Properties.Settings.Default.path1Csestart = txtBox2.Text;
            Properties.Settings.Default.Save();

            Close();
        }
    }
}
