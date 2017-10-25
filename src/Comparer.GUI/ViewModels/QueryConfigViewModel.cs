using DataSetTools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Comparer.GUI.ViewModels
{
    [Serializable]
    public class QueryConfigViewModel : ViewModelBase
    {
        private string _query;
        public string QueryText
        {
            get { return _query; }
            set {
                if (_query != value)
                {
                    _query = value;
                    RaisePropertyChanged("QueryText");
                }
            }
        }

        private string _connString;
        public string ConnectionString
        {
            get { return _connString; }
            set
            {
                if (_connString != value)
                {
                    _connString = value;
                    RaisePropertyChanged("ConnectionString");
                }
            }
        }

        private string _outputFile;
        public string OutputFile
        {
            get { return _outputFile; }
            set
            {
                if (_outputFile != value)
                {
                    _outputFile = value;
                    RaisePropertyChanged("OutputFile");
                }
            }
        }


        private InputTypes _inputType;
        public InputTypes InputType
        {
            get { return _inputType; }
            set
            {
                if (_inputType != value)
                {
                    _inputType = value;
                    RaisePropertyChanged("InputType");
                }
            }
        }

        private ICommand _openConnectionComand;
        [XmlIgnore]
        public ICommand OpenConnectionGUICommand { get { return _openConnectionComand; } private set { _openConnectionComand = value; } }


        public QueryConfigViewModel()
        {

            OpenConnectionGUICommand = new RelayCommand(() => {
               
                    ConnectionStringCreatorGUI.SqlConnectionString initialConnStr;

                    try
                    {
                        initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString(ConnectionString);
                    }
                    catch (Exception)
                    {
                        initialConnStr = new ConnectionStringCreatorGUI.SqlConnectionString();
                    }

                    Window win = new ConnectionStringCreatorGUI.ConnectionStringBuilderWindow(initialConnStr, returnConnBuilder =>
                    {
                        ConnectionString = returnConnBuilder.ToString();
                    });

                    win.Show();
            });
        }
    }
}
