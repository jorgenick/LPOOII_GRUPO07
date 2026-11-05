using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ClasesBase;

namespace Vistas
{
    /// <summary>
    /// Vista previa e impresión del listado de clientes (documento dinámico).
    /// Recibe la lista a mostrar: todos los clientes o sólo los filtrados.
    /// </summary>
    public partial class VistaPreviaClientes : Window
    {
        public VistaPreviaClientes(IEnumerable<Cliente> clientes)
        {
            InitializeComponent();
            lvClientes.ItemsSource = clientes;
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pdlg = new PrintDialog();
            if (pdlg.ShowDialog() == true)
            {
                // Se guarda la configuración de página del documento para restaurarla después de imprimir
                double pageWidth = DocMain.PageWidth;
                double pageHeight = DocMain.PageHeight;
                double columnWidth = DocMain.ColumnWidth;
                double columnGap = DocMain.ColumnGap;
                Thickness pagePadding = DocMain.PagePadding;

                try
                {
                    // Se ajusta el documento al área imprimible de la hoja
                    DocMain.PageWidth = pdlg.PrintableAreaWidth;
                    DocMain.PageHeight = pdlg.PrintableAreaHeight;
                    DocMain.PagePadding = new Thickness(50);
                    DocMain.ColumnGap = 0;
                    DocMain.ColumnWidth = pdlg.PrintableAreaWidth;

                    pdlg.PrintDocument(((IDocumentPaginatorSource)DocMain).DocumentPaginator, "Listado de Clientes");
                }
                finally
                {
                    DocMain.PageWidth = pageWidth;
                    DocMain.PageHeight = pageHeight;
                    DocMain.ColumnWidth = columnWidth;
                    DocMain.ColumnGap = columnGap;
                    DocMain.PagePadding = pagePadding;
                }
            }
        }
    }
}
