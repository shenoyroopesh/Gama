using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;

namespace RadiologyTracking.CustomControls
{
    public class CustomGrid : DataGrid
    {
        bool mouseDown = false;
        List<FrameworkElement> selectedCells = new List<FrameworkElement>();

        public CustomGrid():base()
        {
            this.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(CustomGrid_CellEditEnded);
            this.LoadingRow += new EventHandler<DataGridRowEventArgs>(CustomGrid_LoadingRow);
        }

        /// <summary>
        /// After the editing is completed, it is necessary to add the eventhandlers again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            FrameworkElement cellContent = e.Column.GetCellContent(e.Row);
            addCellEventHandlers(cellContent);
        }

        void CustomGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            foreach (var column in this.Columns)
            {
                FrameworkElement cellContent = column.GetCellContent(e.Row);
                addCellEventHandlers(cellContent);
            }
        }

        void addCellEventHandlers(FrameworkElement cellContent)
        {
            cellContent.MouseEnter += new MouseEventHandler(cellContent_MouseEnter);
            cellContent.MouseLeftButtonDown += new MouseButtonEventHandler(cellContent_MouseLeftButtonDown);
            cellContent.MouseLeftButtonUp += new MouseButtonEventHandler(cellContent_MouseLeftButtonUp);
        }


        void cellContent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }

        void cellContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //clear all the selected cells
            foreach (TextBlock item in selectedCells) 
                item.FontWeight = FontWeights.Normal;

            selectedCells.Clear();

            //if this is not textblock, it means it is in edit mode, so do not proceed further
            if (e.OriginalSource.GetType() != typeof(TextBlock))
                return;

            mouseDown = true;
            selectCell(e.OriginalSource);
        }

        void cellContent_MouseEnter(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                selectCell(e.OriginalSource);
            }
        }
   
        void selectCell(Object c)
        {
            if (c.GetType() != typeof(TextBlock))
                return;

            TextBlock cell = (TextBlock)c;
            selectedCells.Add(cell);            
            cell.FontWeight = FontWeights.Bold;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string text = Clipboard.GetText();

                //cleanup the escape characters
                text = text.Replace("\t", "").Replace("\r", "").Replace("\n", "");
                foreach (TextBlock txt in selectedCells)
                {
                    txt.Text = text;
                    txt.GetBindingExpression(TextBlock.TextProperty).UpdateSource();                    
                }
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }
    }
}
