using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comparer.GUI.ViewModels
{
    public class ColumnMetaData
    {
        public string ColumnName { get; set; }
        public bool IsKeyColumn { get; set; }
        public bool IsIgnoredInComparisson { get; set; }
    }
}
