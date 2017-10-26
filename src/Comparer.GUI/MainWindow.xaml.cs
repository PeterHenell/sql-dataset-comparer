using Comparer.GUI.ViewModels;
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
using System.Xml.Serialization;

namespace Comparer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.VM.SaveState();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.VM = new ProjectViewModel();

            // Load saved state if exists
            if (System.IO.File.Exists(ProjectViewModel.SaveFilePath))
            {
                try
                {
                    using (var stream = System.IO.File.OpenRead(ProjectViewModel.SaveFilePath))
                    {
                        var serializer = new XmlSerializer(typeof(QueryConfiguration));
                        var loaded = serializer.Deserialize(stream) as QueryConfiguration;

                        this.VM.QueryConfiguration = loaded;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    // in case of failed deserialization, we kill the file.
                    //System.IO.File.Delete(MainWindowViewModel.SaveFilePath);
                }
            }


            this.DataContext = this.VM;
        }

        public ProjectViewModel VM { get; set; }
    }
}
