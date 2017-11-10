using DataSetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Comparer.GUI.Selectors
{
    public class ComparerTypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CsvFileTemplate { get; set; }
        public DataTemplate QueryTemplate { get; set; }
        public DataTemplate NotImplementedTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is InputTypes)
            {
                var value = (InputTypes)item;
                switch (value)
                {
                    case InputTypes.Query:
                        return QueryTemplate;
                    case InputTypes.CsvFile:
                        return CsvFileTemplate;
                    case InputTypes.DataTable:
                        return NotImplementedTemplate;
                }

                return base.SelectTemplate(item, container);
            }
            else
            {
                return base.SelectTemplate(QueryTemplate, container);
            }
        }
    }
}
