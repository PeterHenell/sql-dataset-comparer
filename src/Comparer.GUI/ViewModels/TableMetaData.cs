using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Comparer.GUI.ViewModels
{
    public class TableMetaData
    {
        public ObservableCollection<ColumnMetaData> Columns { get; set; }
        public string TableName { get; set; }

        public TableMetaData()
        {
            Columns = new ObservableCollection<ColumnMetaData>();
        }
    }
}
