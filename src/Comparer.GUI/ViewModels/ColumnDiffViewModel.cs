using DataSetTools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Comparer.GUI.ViewModels
{
    public class ColumnDiffViewModel : ViewModelBase
    {

        public ICommand RunCommand { get; private set; }
        public ICommand PeekColumnsCommand { get; private set; }
        public ICommand ViewComparisonReportCommand { get; private set; }

        private DetailedCompareResult _detailedCompareResult;
        public DetailedCompareResult DetailedCompareResult
        {
            get { return _detailedCompareResult; }
            set
            {
                if (_detailedCompareResult != value)
                {
                    _detailedCompareResult = value;
                    RaisePropertyChanged("DetailedCompareResult");
                    RaisePropertyChanged("MissingKeyErrors");
                    RaisePropertyChanged("MultipleRowsPerKeyErrors");
                    RaisePropertyChanged("ValuesDoesNotMatchErrors");
                }
            }
        }

        public IEnumerable<CompareError> MissingKeyErrors
        {
            get
            {
                return DetailedCompareResult.ComparisonErrors.Where(e => e.MissingInB);
            }
        }
        public IEnumerable<CompareError> MultipleRowsPerKeyErrors
        {
            get
            {
                return DetailedCompareResult.ComparisonErrors.Where(e => e.MultipleRowPerKeyInB);
            }
        }
        public IEnumerable<CompareError> ValuesDoesNotMatchErrors
        {
            get
            {
                return DetailedCompareResult.ComparisonErrors.Where(e => e.ValuesDoesNotMatch);
            }
        }

        private TableMetaData _queryMetaData;
        public TableMetaData QueryMetaData
        {
            get { return _queryMetaData; }
            set
            {
                if (_queryMetaData != value)
                {
                    _queryMetaData = value;
                    RaisePropertyChanged("QueryMetaData");
                }
            }
        }

        public QueryConfiguration QueryConfiguration
        {
            get
            {
                return MainWindowViewModel.QueryConfiguration;
            }
        }

        public ColumnDiffViewModel()
        {

        }

        public ColumnDiffViewModel(ProjectViewModel mainWindowViewModel)
        {
            this.MainWindowViewModel = mainWindowViewModel;
            this._detailedCompareResult = new DetailedCompareResult();
            this._queryMetaData = new TableMetaData();

            RunCommand = new RelayCommand( async () =>
            {
                MainWindowViewModel.Lock();

                DetailedCompareResult result = await GetDetailedDifferences();

                Dispatcher.CurrentDispatcher.Invoke(
                              new Action(() =>
                              {
                                  if (result != null)
                                  {
                                      DetailedCompareResult = result;
                                  }
                              }));
                MainWindowViewModel.UnLock();
            });

            PeekColumnsCommand = new RelayCommand( async () =>
            {
                MainWindowViewModel.Lock();
                QueryMetaData.Columns.Clear();
                var columns = await GetQueryColumnsAsync();

                Dispatcher.CurrentDispatcher.Invoke(
                              new Action(() =>
                              {
                                  QueryMetaData.TableName = "a";
                                  foreach (var col in columns)
                                  {
                                      QueryMetaData.Columns.Add(col);
                                  }
                              }));
                MainWindowViewModel.UnLock();
            });

            ViewComparisonReportCommand = new RelayCommand(() => {
                var stats = DetailedCompareResult.GetCalculatedStats();
                var win = new StatsReportWindow(stats);
                win.ShowDialog();
            }, ViewReportCanExecute);

            
        }

        private bool ViewReportCanExecute()
        {
            return DetailedCompareResult.ComparisonErrors.Count > 0;
        }

       

        private async Task<DetailedCompareResult> GetDetailedDifferences()
        {
            var keyColumns = QueryMetaData.Columns
                .Where(c => c.IsKeyColumn)
                .Select(c => c.ColumnName);

            if (keyColumns.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("At least one column need to be marked as key");
                return new DetailedCompareResult();
            }

            var ignoredColumns = QueryMetaData.Columns
                .Where(c => c.IsIgnoredInComparisson)
                .Select(c => c.ColumnName);

            DetailedCompareResult result = null ;

            var a = new Action( () => {
                try
                {
                    var setComparer = QueryConfiguration.GetComparer(MainWindowViewModel.LogWriter);
                    result = setComparer.GetDetailedDifferences(
                    keyColumns,
                    ignoredColumns
                    );
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                
            });
            await Task.Run(a);

            return result;
        }

        private async Task<List<ColumnMetaData>> GetQueryColumnsAsync()
        {
            var cols = new List<ColumnMetaData>();
            
            var a = new Action( async () =>
            {
                try
                {
                    var setComparer = QueryConfiguration.GetComparer(MainWindowViewModel.LogWriter);
                    List<string> columns = setComparer.PeekResultSetColumnNames();
                    foreach (var c in columns)
                    {
                        cols.Add(new ColumnMetaData { ColumnName = c });
                    }
                }
                catch (Exception ex)
                {
                    global::System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            });
            await Task.Run(a);
           
            return cols;
        }

        public ProjectViewModel MainWindowViewModel { get; set; }
    }
}
