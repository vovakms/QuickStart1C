using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Стартер1С.Model;

namespace Стартер1С.ViewModel
{
    public class CatalogVM : INotifyPropertyChanged
    {
       // public string count;

        public ObservableCollection<Catalog> SpisokDir { get; set; }

        public Catalog CatDir;
         


        private Catalog selectedCatalog;  // выбранная строка в списке
        public Catalog SelectedCatalog
        {
            get { return selectedCatalog; }
            set
            {
                selectedCatalog = value;
                OnPropertyChanged("SelectedCatalog");
            }
        }

        public CatalogVM()  // конструктор
        {
            string path = "";

            if (Properties.Settings.Default.nomStart == 0)  // если первый запуск
            {
                path = Directory.GetCurrentDirectory(); // определяем текущий каталог
                 
                Properties.Settings.Default.path_db = path;

                string[] pathBin1c = { "" };
                try { pathBin1c = Directory.GetFiles(@"C:\Program Files\1cv8", "1cv8.exe", SearchOption.AllDirectories); } catch { }
                try { pathBin1c = Directory.GetFiles(@"C:\Program Files (x86)\1cv8", "1cv8.exe", SearchOption.AllDirectories); } catch { }
                try { pathBin1c = Directory.GetFiles(@"C:\Program Files\1cv82", "1cv8.exe", SearchOption.AllDirectories); } catch { }
                try { pathBin1c = Directory.GetFiles(@"C:\Program Files (x86)\1cv82", "1cv8.exe", SearchOption.AllDirectories); } catch { }
                try
                {
                    foreach (string fb in pathBin1c)
                        Properties.Settings.Default.pathBin1c = Path.GetDirectoryName(fb);
                }
                catch { }

                string[] path1Csestart = { "" };
                try { path1Csestart = Directory.GetFiles(@"C:\Program Files\1cv8\common", "1cestart.exe", SearchOption.AllDirectories); } catch { }
                try { path1Csestart = Directory.GetFiles(@"C:\Program Files (x86)\1cv8\common", "1cestart.exe", SearchOption.AllDirectories); } catch { }
                try { path1Csestart = Directory.GetFiles(@"C:\Program Files\1cv82\common", "1cestart.exe", SearchOption.AllDirectories); } catch { }
                try { path1Csestart = Directory.GetFiles(@"C:\Program Files (x86)\1cv82\common", "1cestart.exe", SearchOption.AllDirectories); } catch { }
                try
                {
                    foreach (string fs in path1Csestart)
                        Properties.Settings.Default.path1Csestart = fs;
                }
                catch { }
 
                Properties.Settings.Default.nomStart = 1;
                Properties.Settings.Default.Save();

                var newForm = new Настройки();
                newForm.Show();
            }
             
            if (Properties.Settings.Default.path_db != "")
                path = Properties.Settings.Default.path_db;

            SpisokDir = new ObservableCollection<Catalog> { };
            CatDir    = new Catalog();
            LoadModel(path);
        }



        private RelayCommand openEnterprise;    // команда открыть в "1СПредриятие"
        public RelayCommand OpenEnterprise
        {
            get
            {
                return openEnterprise ??
                  (openEnterprise = new RelayCommand(obj =>
                  {
                      if (SelectedCatalog.CatalogPath != null)
                      {
                          // string db1c = StatusBarItem1.Content.ToString() + "\\" + ListBox1.SelectedItem.ToString();
                          string command = @" ENTERPRISE /F """ + SelectedCatalog.DirPath + @"""";
                          Process.Start(Properties.Settings.Default.pathBin1c + "\\1cv8.exe", command); // 
                      }
                  }));
            }
        }

        private RelayCommand openConfigurator;  // команда открыть в "Конфигуратор"
        public RelayCommand OpenConfigurator
        {
            get
            {
                return openConfigurator ??
                  (openConfigurator = new RelayCommand(obj =>
                  {
                      if (SelectedCatalog.CatalogPath != null)
                      {
                          //string db1c = StatusBarItem1.Content.ToString() + "\\" + ListBox1.SelectedItem.ToString();
                          string command = @" CONFIG /F """ + SelectedCatalog.DirPath + @"""";
                          Process.Start(Properties.Settings.Default.pathBin1c + "\\1cv8.exe", command); //
                      }
                  }));
            }
        }
         
        private RelayCommand createEmptyBD;     // команда   Создать пустую БД без конфигурации
        public RelayCommand CreateEmptyBD
        {
            get
            {
                return createEmptyBD ??
                  (createEmptyBD = new RelayCommand(obj =>
                  {
                      string nameNewDB = "НоваяБД от " + DateTime.Now.ToString("yyyy-MM-dd hhmss");
                      string pathOld   = SpisokDir[0].CatalogPath;
                      ProcessStartInfo startCMD = new ProcessStartInfo("cmd.exe");
                      startCMD.CreateNoWindow   = true;
                      startCMD.UseShellExecute  = false;

                      startCMD.Arguments = @"/c cd """ + pathOld + @""" && """ + Properties.Settings.Default.path1Csestart + @""" CREATEINFOBASE File=""" + nameNewDB + @"""; /UseTemplate """"";
                      Process.Start(startCMD);

                      Catalog cat = new Catalog();
                      SpisokDir.Insert(0, cat);
                      SelectedCatalog = cat;

                      Thread.Sleep(1000);
                      
                      LoadModel(pathOld);
                      //ListBox1.Items.Add(nameNewDB);
                  }));
            }
        }

        private RelayCommand createBD;          // команда   Создать  БД-х из dt cf
        public RelayCommand CreateBD
        {
            get
            {
                return createBD ??
                  (createBD = new RelayCommand(obj =>
                  {
                      FolderBrowserDialog folderBrowser = new FolderBrowserDialog(); // вызываем диалог выбора каталога
                      DialogResult result = folderBrowser.ShowDialog();

                      if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath)) // если все ОК 
                      {
                          //    ListBox1.Items.Clear();
                          //    StatusBarItem1.Content = folderBrowser.SelectedPath; // 
                          Catalog cat = new Catalog();
                          cat.Create_dt(folderBrowser.SelectedPath);
                          cat.Create_cf();
                           

                          Thread.Sleep(1000);

                          LoadModel(folderBrowser.SelectedPath);
                         
                           
                      //    StatusBarItem2.Content = "Состояние: в указанном каталоге созданы БД";

                      }
                  }));
            }
        }
        
        private RelayCommand close;             // команда Закрыть окно
        public RelayCommand Close
        {
            get
            {
                return close ??
                  (close = new RelayCommand(obj =>
                  {
                      Environment.Exit(0);
                      //Application.Current.Shutdown();
                      //System.Windows.Forms.Application.Exit();
                  }));
            }
        }

        private RelayCommand copy;              // команда Копировать 
        public RelayCommand Copy
        {
            get
            {
                return copy ??
                  (copy = new RelayCommand(obj =>
                  {
                      if (SelectedCatalog.CatalogPath != null)
                      {
                           string newDB = SelectedCatalog.DirName + @"_копия"; // имя новой БД
                           string dirDBsour = SelectedCatalog.DirPath ;
                           string dirDBtarg = SelectedCatalog.CatalogPath + "\\" + newDB;

                           try
                           {
                              if (Directory.Exists(dirDBtarg)) // Проверяем, существует ли каталог.
                              {
                                  MessageBox.Show("В этом каталоге уже существует БД с таким именем", "Ошибка при вводе имени");
                                  return;
                              }
                              DirectoryInfo di = Directory.CreateDirectory(dirDBtarg);
                              // Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                          }
                          catch { }

                         File.Copy(dirDBsour + @"\1Cv8.1CD", dirDBtarg + @"\1Cv8.1CD");

                          Thread.Sleep(1000);

                          LoadModel(SelectedCatalog.CatalogPath);

                      }
                  }));
            }
        }
         
        private RelayCommand openDialog;        // команда открываем диалог выбора каталога
        public RelayCommand OpenDialog
        {
            get
            {
                return openDialog ??
                  (openDialog = new RelayCommand(obj =>
                  {
                      FolderBrowserDialog fBD = new FolderBrowserDialog(); // диалоговое окно
                      DialogResult result = fBD.ShowDialog();       // показываем диалоговое окно

                      if (!string.IsNullOrWhiteSpace(fBD.SelectedPath)) // если выбрали каталог
                      {
                           // SpisokDir = new ObservableCollection<Catalog> { };
                           // CatDir    = new Catalog();
                          LoadModel(fBD.SelectedPath); // 
                          //  new MainViewModel(fBD.SelectedPath)  ;
                      }
                  }));
            }
        }

        private RelayCommand openSetting;       // команда окно Настроек
        public RelayCommand OpenSetting
        {
            get
            {
                return openSetting ??
                  (openSetting = new RelayCommand(obj =>
                  {
                      var newForm = new Настройки();
                      newForm.Show();
                  }));
            }
        }



        private void LoadModel(string path) 
        {
            SpisokDir.Clear();

            Catalog[] cd = CatDir.GetSpisokDir(path);
            foreach (var cat in cd)  // в модель передаем   и перебираем массив классов
            {
                SpisokDir.Add(new Catalog
                {
                    CatalogName = cat.CatalogName,
                    CatalogPath = cat.CatalogPath,
                    DirCount    = cd.Count().ToString(),
                    DirName     = cat.DirName,
                    DirPath     = cat.DirPath
                });
            }

            Properties.Settings.Default.path_db = path;
            Properties.Settings.Default.Save();
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
