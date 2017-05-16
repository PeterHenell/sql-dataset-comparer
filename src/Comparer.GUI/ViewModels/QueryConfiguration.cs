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
    public class QueryConfiguration : ViewModelBase
    {
        public QueryConfiguration()
        {
            Left = new QueryConfigViewModel();
            Right = new QueryConfigViewModel();
            this.OutputFolder = @"c:\temp";
        }

        private QueryConfigViewModel _left;
        public QueryConfigViewModel Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    RaisePropertyChanged("Left");
                }
            }
        }

        private QueryConfigViewModel _right;
        public QueryConfigViewModel Right
        {
            get { return _right; }
            set
            {
                if (_right != value)
                {
                    _right = value;
                    RaisePropertyChanged("Right");
                }
            }
        }

        private string _outputFolder;
        public string OutputFolder
        {
            get { return _outputFolder; }
            set
            {
                if (_outputFolder != value)
                {
                    _outputFolder = value;
                    RaisePropertyChanged("OutputFolder");
                }
            }
        }

        internal DataSetComparer GetComparer(TextWriter logWriter)
        {
            var setComparer = CompareBuilder.Build(OutputFolder, logWriter, (comparer) => comparer
                             .Compare(Left.InputType, Left.QueryText, Left.ConnectionString, Left.OutputFile)
                             .To(Right.InputType, Right.QueryText, Right.ConnectionString, Right.OutputFile)
                             .End());
            return setComparer;
        }
    }
}
