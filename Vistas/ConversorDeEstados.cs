using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace Vistas
{
    public class ConversorDeEstados : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string estado = value as string;

            switch (estado)
            { 
                case "PENDIENTE":
                    return Brushes.Red;
                case "PAGADA":
                    return Brushes.Green;
                case "CONTABILIZADA":
                    return Brushes.Blue;
                case "ANULADA":
                    return Brushes.Gray;
                default:
                    return Brushes.Transparent;
            
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
