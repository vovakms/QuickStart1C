using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Стартер1С.ViewModel;

namespace Стартер1С
{
     
    public partial class App : Application
    {
        public App()
        {
             
            var MainWindow = new MainWindow
            {
                DataContext = new CatalogVM()
            };

            MainWindow.Show();
        }
    }
}
