using Comparer.GUI.Logging;
using DataSetTools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;


namespace Comparer.GUI.ViewModels
{
    public class ConfigureQueriesViewModel : ViewModelBase
    {
        private string _output;
        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    RaisePropertyChanged("Output");
                }
            }
        }

        public ProjectViewModel MainViewModel { get; set; }
        
        public ICommand RunCommand { get; private set; }
        public ICommand PerformDetailedComparisonCommand { get; private set; }
        public ICommand CopyToRightSideCommand { get; private set; }


        public QueryConfiguration QueryConfiguration
        {
            get
            {
                return MainViewModel.QueryConfiguration;
            }
        }

        public ConfigureQueriesViewModel()
            : this(new ProjectViewModel())
        {
        }

        public ConfigureQueriesViewModel(ProjectViewModel mainViewModel)
        {

            this.MainViewModel = mainViewModel;

            CopyToRightSideCommand = new RelayCommand(() =>
            {
                QueryConfiguration.Right.ConnectionString = QueryConfiguration.Left.ConnectionString;
                QueryConfiguration.Right.QueryText = QueryConfiguration.Left.QueryText;
                QueryConfiguration.Right.OutputFile = QueryConfiguration.Left.OutputFile + "B";
            });
           

            RunCommand = new RelayCommand(() =>
            {
                Output = string.Empty;
                MainViewModel.SaveState();

                var a = new Action(() =>
                {
                    MainViewModel.Lock();   
                    var setComparer = QueryConfiguration.GetComparer(MainViewModel.LogWriter);
                    try
                    {
                        setComparer.CompareCommands();
                    }
                    catch (Exception ex)
                    {
                        global::System.Windows.Forms.MessageBox.Show(ex.ToString());
                    }                   
                });
                a.BeginInvoke(completed, null);
            });

            PerformDetailedComparisonCommand = new RelayCommand(() =>
            {
                MainViewModel.SaveState();
                var setComparer = QueryConfiguration.GetComparer(MainViewModel.LogWriter);

                MainViewModel.SelectDetailedComparisonTab();
            });
        }

        private void completed(IAsyncResult ar)
        {
            MainViewModel.UnLock();
        }
    }
}
