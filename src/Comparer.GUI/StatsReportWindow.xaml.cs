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
using System.Windows.Shapes;

namespace Comparer.GUI
{
    /// <summary>
    /// Interaction logic for StatsReportWindow.xaml
    /// </summary>
    public partial class StatsReportWindow : Window
    {
        private DataSetTools.DetailedStats stats;
        StatsReportViewModel vm;

        public StatsReportWindow(DataSetTools.DetailedStats stats)
        {
            InitializeComponent();
            this.stats = stats;

            this.Loaded += StatsReportWindow_Loaded;
        }

        void StatsReportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new StatsReportViewModel(stats);
            this.DataContext = vm;
        }
    }
}
