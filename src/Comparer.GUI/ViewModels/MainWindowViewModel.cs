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
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _locked;
        public bool UnLocked
        {
            get { return _locked; }
            set
            {
                if (_locked != value)
                {
                    _locked = value;
                    RaisePropertyChanged("UnLocked");
                }
            }
        }

        private ColumnDiffViewModel _columnDiffViewModel;
        public ColumnDiffViewModel ColumnDiffViewModel
        {
            get { return _columnDiffViewModel; }
            set
            {
                if (_columnDiffViewModel != value)
                {
                    _columnDiffViewModel = value;
                    RaisePropertyChanged("ColumnDiffViewModel");
                }
            }
        }

        private ConfigureQueriesViewModel _configureQueriesViewModel;
        public ConfigureQueriesViewModel ConfigureQueriesViewModel
        {
            get { return _configureQueriesViewModel; }
            set
            {
                if (_configureQueriesViewModel != value)
                {
                    _configureQueriesViewModel = value;
                    RaisePropertyChanged("ConfigureQueriesViewModel");
                }
            }
        }

        private bool _isDetailedComparisonSelected;
        public bool IsDetailedComparisonSelected
        {
            get { return _isDetailedComparisonSelected; }
            set
            {
                if (_isDetailedComparisonSelected != value)
                {
                    _isDetailedComparisonSelected = value;
                    RaisePropertyChanged("IsDetailedComparisonSelected");
                }
            }
        }

        private bool _isConfigureQueriesSelected;
        public bool IsConfigureQueriesSelected
        {
            get { return _isConfigureQueriesSelected; }
            set
            {
                if (_isConfigureQueriesSelected != value)
                {
                    _isConfigureQueriesSelected = value;
                    RaisePropertyChanged("IsConfigureQueriesSelected");
                }
            }
        }

        private PropertyAppendLogWriter<ConfigureQueriesViewModel> _logWriter;
        public PropertyAppendLogWriter<ConfigureQueriesViewModel> LogWriter { get { return _logWriter; } }

        public QueryConfiguration QueryConfiguration { get; set; }

        public MainWindowViewModel()
        {
            UnLocked = true;
            IsConfigureQueriesSelected = true;
            IsDetailedComparisonSelected = false;

            ColumnDiffViewModel = new ColumnDiffViewModel(this);
            ConfigureQueriesViewModel = new ConfigureQueriesViewModel(this);
            QueryConfiguration = new QueryConfiguration();

            this._logWriter = 
                new PropertyAppendLogWriter<ConfigureQueriesViewModel>(
                    this.ConfigureQueriesViewModel, x => x.Output);
        }

        public void Lock()
        {
            UnLocked = false;
        }
        public void UnLock()
        {
            UnLocked = true;
        }

        public void SelectEditQueryTab()
        {
            IsDetailedComparisonSelected = false;
            IsConfigureQueriesSelected = true;
        }

        public void SelectDetailedComparisonTab()
        {
            IsDetailedComparisonSelected = true;
            IsConfigureQueriesSelected = false;
        }

        public static string SaveFilePath =
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CompareResults.GUI.xml");

        private void SaveState(string path)
        {
            using (var writer = new System.IO.StreamWriter(path))
            {
                var serializer = new XmlSerializer(typeof(QueryConfiguration));
                serializer.Serialize(writer, QueryConfiguration);
                writer.Flush();
            }
        }

        internal void SaveState()
        {
            SaveState(SaveFilePath);
        }
    }
}
