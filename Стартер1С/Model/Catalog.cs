using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Стартер1С.Model
{
    public class Catalog : INotifyPropertyChanged
    {
        private string catalogName;
        private string catalogPath;
        private string dirCount;
        private string dirName;
        private string dirPath;

        public string CatalogName
        {
            get { return catalogName; }
            set
            {
                catalogName = value;
                OnPropertyChanged("CatalogName");
            }
        }
        public string CatalogPath
        {
            get { return catalogPath; }
            set
            {
                catalogPath = value;
                OnPropertyChanged("CatalogPath");
            }
        }
        public string DirCount
        {
            get { return dirCount; }
            set
            {
                dirCount = value;
                OnPropertyChanged("DirCount");
            }
        }
        public string DirName
        {
            get { return dirName; }
            set
            {
                dirName = value;
                OnPropertyChanged("DirName");
            }
        }
        public string DirPath
        {
            get { return dirPath; }
            set
            {
                dirPath = value;
                OnPropertyChanged("DirPath");
            }
        }


        public Catalog[] GetSpisokDir(string path) // функция делаем массив классов
        {
            List<Catalog> list = new List<Catalog>(); // список классов 

            string nameCat = Path.GetFileName(path); // имя каталога получаем из полного пути
            int count = Directory.GetDirectories(path).Length;
            if (File.Exists(path + "\\" + nameCat + ".v8i")) // если файл списка уже существует
            {
                File.Delete(path + "\\" + nameCat + ".v8i"); // удаляем его
            }
            int nd = 1; // счетчик папок БД1С
            foreach (string dir in Directory.GetDirectories(path)) // перебираем папки в нашем каталоге 
            {
                string nameDir = Path.GetFileName(dir); // имя папки из полного пути

                if (File.Exists(dir + "\\" + "1Cv8.1CD")) // если в папке присутствут файл 1Cv8.1CD значит это папка БД1С
                {
                    StreamWriter sw = new StreamWriter(path + "\\" + nameCat + ".v8i", true, System.Text.Encoding.UTF8);

                    string nameK = Path.GetFileName(dir);

                    sw.WriteLine("[" + nameK + "]");
                    sw.WriteLine(@"Connect=File=""" + dir + @""";");
                    sw.WriteLine(@"Folder=/" + nameCat);
                    sw.WriteLine(@"External=0");
                    sw.WriteLine(@"ClientConnectionSpeed=Normal");
                    sw.WriteLine(@"App=Auto");
                    sw.WriteLine(@"WA=1");
                    sw.WriteLine(@"Version=8.3");
                    sw.Close();
                    // заполняем очередной элемент нашего списка    
                    list.Add(new Catalog
                    {
                        CatalogName = nameCat,
                        CatalogPath = path,
                        DirCount = nd.ToString(),
                        DirName = nameDir,
                        DirPath = dir
                    });
                    nd++;
                }
            }
            return list.ToArray(); // возращаем список преобразованный в массив классов
        }
 
        public void Create_dt(string path)
        {
             string[] AllDT = Directory.GetFiles(path, "*.dt"); // сканируем указанный каталог
             
             foreach (string filedt in AllDT)
             {
                ProcessStartInfo startCMD = new ProcessStartInfo("cmd.exe");
                startCMD.CreateNoWindow = true;
                startCMD.UseShellExecute = false;
                string fileName = System.IO.Path.GetFileName(filedt);
                string namedt = fileName.Remove(fileName.Length - 3);
                startCMD.Arguments = @"/c cd """ + path + @""" & """ + Properties.Settings.Default.path1Csestart + @""" CREATEINFOBASE File=""" + namedt + @"""; /UseTemplate """ + filedt + @"""";
                Process.Start(startCMD);
                 
                //ListBox1.Items.Add(namedt);
            }

        }

        public void Create_cf()
        {
          //  string[] AllCF = Directory.GetFiles(StatusBarItem1.Content.ToString(), "*.cf"); // сканируем указанный каталог 

            //foreach (string filecf in AllCF)
            //{
            //    string fileName = System.IO.Path.GetFileName(filecf);
            //    string namecf = fileName.Remove(fileName.Length - 3);
            //    ProcessStartInfo startCMD = new ProcessStartInfo("cmd.exe");
                
            //    startCMD.CreateNoWindow = true;
            //    startCMD.UseShellExecute = false;
                
            // //   startCMD.Arguments = @"/c cd """ + StatusBarItem1.Content.ToString() + @""" && """ + Properties.Settings.Default.path1Csestart + @""" CREATEINFOBASE File=""" + namecf + @"""; /UseTemplate """ + filecf + @"""";
            //    Process.Start(startCMD);
                 
            //  //  ListBox1.Items.Add(namecf);
            //}
        }






        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
