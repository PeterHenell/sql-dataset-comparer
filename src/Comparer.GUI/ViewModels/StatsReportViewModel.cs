using DataSetTools;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;

namespace Comparer.GUI.ViewModels
{
    public class StatsReportViewModel : ViewModelBase
    {

        private DetailedStats _stats;
        public DetailedStats Stats
        {
            get { return _stats; }
            set
            {
                if (_stats != value)
                {
                    _stats = value;
                    RaisePropertyChanged("Stats");
                }
            }
        }

        public RelayCommand ExportToExcelCommand { get; private set; }

        public StatsReportViewModel(DetailedStats stats = null)
        {
            this.Stats = stats;

            ExportToExcelCommand = new RelayCommand(() =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Export To Excel file";
                var res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    using (var writer = System.IO.File.CreateText(sfd.FileName))
                    {
                        writer.WriteLine("ColumnName;Expected;Actual");
                        foreach (var item in Stats.Values)
                        {
                            foreach (var st in item.Value.Members)
                            {
                                writer.WriteLine("{0};{1};{2}", st.ColumnName, st.ExpectedValue, st.ActualValue);
                            }
                        }
                        writer.Flush();
                    }
                }
            });
        }
    }
}
