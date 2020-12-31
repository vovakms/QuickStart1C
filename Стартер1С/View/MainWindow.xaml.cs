using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Documents;
using System.Collections;

namespace Стартер1С
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
 
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)     // перемещение окна 
        {
            this.DragMove();

            ListBox1.SelectedIndex = -1;
        }
         
    }
     
}
