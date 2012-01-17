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

namespace Vagsons.Controls
{
    public class CustomGrid : DataGrid
    {
        bool mouseDown = false;
        Dictionary<DataGridCell, Brush> selectedCells = new Dictionary<DataGridCell, Brush>();
        String copiedText = String.Empty;

        public CustomGrid():base()
        {
            this.RowEditEnded += new EventHandler<DataGridRowEditEndedEventArgs>(CustomGrid_RowEditEnded);
            this.BeginningEdit += new EventHandler<DataGridBeginningEditEventArgs>(CustomGrid_BeginningEdit);
            this.MouseLeave += new MouseEventHandler(CustomGrid_MouseLeave);
            this.MouseEnter += new MouseEventHandler(CustomGrid_MouseEnter);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomGrid_MouseLeftButtonUp);
            this.LoadingRow += new EventHandler<DataGridRowEventArgs>(CustomGrid_LoadingRow);
            this.AutoGenerateColumns = false;
        }

        void CustomGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            CustomGrid_LoadingRow(sender, new DataGridRowEventArgs(e.Row));
        }

        void CustomGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //done here since when row enters edit mode, the textblock cannot capture the mouseup event
            mouseDown = false;
            clearAllSelection();
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

        void CustomGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            ReleaseMouseCapture();
        }

        void CustomGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ReleaseMouseCapture();
        }

        /// <summary>
        /// Capture the mouse if it is leaving when down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if(mouseDown)
                CaptureMouse();
        }
        
        void cellContent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }


        void cellContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if ctrl button is not held down, clear all selection
            if((Keyboard.Modifiers & ModifierKeys.Control) == 0)
                clearAllSelection();

            //if this is not textblock, it means it is in edit mode, so do not proceed further
            if (e.OriginalSource.GetType() != typeof(TextBlock))
                return;

            mouseDown = true;
            selectCell(e.OriginalSource);
        }

        /// <summary>
        /// Clears all the selected cells and reverts them back to their original state
        /// </summary>
        void clearAllSelection()
        {
            //clear all the selected cells
            foreach (var item in selectedCells)
            {
                //make the background its original value
                item.Key.Background = item.Value;
            }

            selectedCells.Clear();
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

            TextBlock tblock = (TextBlock)c;
            DataGridCell cell = (DataGridCell)tblock.Parent;

            //if already added, return
            if (selectedCells.ContainsKey(cell)) return;

            selectedCells.Add(cell, cell.Background);
            cell.Background = new SolidColorBrush(Colors.Purple);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string text = copiedText;

                //cleanup the escape characters
                text = text.Replace("\t", "").Replace("\r", "").Replace("\n", "");
                foreach (var item in selectedCells)
                {
                    //this applies to only textblocks within cells
                    if (item.Key.Content.GetType() != typeof(TextBlock))
                        continue;

                    TextBlock txt = (TextBlock)item.Key.Content;
                    string originalText = txt.Text;
                    //if there is any exception, only for that cell revert to old value
                    try
                    {
                        txt.Text = text;
                        txt.GetBindingExpression(TextBlock.TextProperty).UpdateSource();
                    }
                    catch
                    {
                        txt.Text = originalText;
                    }
                }
                e.Handled = true;
            }
            // handle copying so that entire row does not get copied even if a row is selected
            else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (this.CurrentColumn.GetCellContent(this.CurrentItem).GetType() == typeof(TextBlock))
                    copiedText = ((TextBlock)this.CurrentColumn.GetCellContent(this.CurrentItem)).Text;
                else
                    base.OnKeyDown(e);
            }
            else
            {
                base.OnKeyDown(e);
            }
        }
    }
}
