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
using System.Collections.ObjectModel;
using System.Windows.Markup;


namespace MenuControl
{

    [TemplatePart(Name = "LayoutRootMI2", Type = typeof(Canvas))]
    [TemplatePart(Name = "ItemTextMI", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ItemHighlightMI", Type = typeof(Canvas))]
    [TemplatePart(Name = "ItemHighlightrMI", Type = typeof(Rectangle))]
    [TemplatePart(Name = "ItemText_copyMI", Type = typeof(TextBlock))]
    [TemplateVisualState(Name = "ItemHighlightedMI", GroupName = "menuNavigationMI")]
    [TemplateVisualState(Name = "noneHighlightedMI", GroupName = "menuNavigationMI")]

    [ContentPropertyAttribute("items")]
    public class MenuItem : Control
    {
        //graphic elements
        public Canvas LayoutRootMI;
        public Canvas LayoutRootMI2;
        public TextBlock ItemTextMI;
        public Canvas ItemHighlightMI;
        public Rectangle ItemHighlightrMI;
        public TextBlock ItemText_copyMI;
        public Path arrow;
        public Path arrowHighlight;
        public Canvas ItemDropDownMI;
        public Rectangle baseRectMI;
        public StackPanel itemHolderMI;

        private bool isNested;
        
        public Point xy;

        public MenuItem parentMenuItem;
        public MenuBarItem parentMenuBarItem;

        public event RoutedEventHandler Click;

       
        public MenuItem()
        {
            this.DefaultStyleKey = typeof(MenuItem);
            this.Loaded += new RoutedEventHandler(MenuItem_Loaded);
            if (items == null)
                items = new ObservableCollection<MenuItem>();
            if (string.IsNullOrEmpty(MenuText))
                MenuText = "Default";

            isNested = false;

            parentMenuBarItem = null;
            parentMenuItem = null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRootMI = (Canvas)GetTemplateChild("LayoutRootMI");
            LayoutRootMI2 = (Canvas)GetTemplateChild("LayoutRootMI2");
            ItemTextMI = (TextBlock)GetTemplateChild("ItemTextMI");
            ItemHighlightMI = (Canvas)GetTemplateChild("ItemHighlightMI");
            ItemHighlightrMI = (Rectangle)GetTemplateChild("ItemHighlightrMI");
            ItemText_copyMI = (TextBlock)GetTemplateChild("ItemText_copyMI");
            arrow = (Path)GetTemplateChild("arrow");
            arrowHighlight = (Path)GetTemplateChild("arrowHighlight");
            ItemDropDownMI = (Canvas)GetTemplateChild("ItemDropDownMI");
            baseRectMI = (Rectangle)GetTemplateChild("baseRectMI");
            itemHolderMI = (StackPanel)GetTemplateChild("itemHolderMI");

            ItemHighlightMI.MouseEnter += new MouseEventHandler(ItemHighlight_MouseEnter);
            ItemHighlightMI.MouseLeave += new MouseEventHandler(ItemHighlight_MouseLeave);
            ItemHighlightMI.MouseLeftButtonDown += new MouseButtonEventHandler(ItemHighlight_MouseLeftButtonDown);
            ItemDropDownMI.MouseLeave += new MouseEventHandler(ItemDropDownMI_MouseLeave);

            // set dimensions
            ItemTextMI.Width = xy.X;
            ItemTextMI.Height = xy.Y;

            ItemText_copyMI.Width = xy.X;
            ItemText_copyMI.Height = xy.Y;


            //set width and height for canvas
            LayoutRootMI.Width = xy.X + 15;
            LayoutRootMI.Height = xy.Y + 10;
            ItemHighlightMI.Width = xy.X + 15;
            ItemHighlightMI.Height = xy.Y + 10;
            ItemHighlightrMI.Width = xy.X + 15;
            ItemHighlightrMI.Height = xy.Y + 10;

        }

                

        void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.ApplyTemplate();

            if (!string.IsNullOrEmpty(MenuText))
            {
                ItemTextMI.Text = MenuText;
                ItemText_copyMI.Text = MenuText;
            }

            // add menu items if any exists
            if (items != null && items.Count > 0)
            {
                isNested = true;
                arrow.Visibility = Visibility.Visible;
                arrowHighlight.Visibility = Visibility.Visible;


                Point xy = getLargest(items);
                xy.X += 15; //add space for arrow placement


                arrow.SetValue(Canvas.LeftProperty, this.xy.X + 12);
                arrowHighlight.SetValue(Canvas.LeftProperty, this.xy.X + 12);

                //set menu holder dimensions
                ItemDropDownMI.Width = xy.X + 23;
                baseRectMI.Width = xy.X + 23;

                //ItemDropDownMI.SetValue(Canvas.TopProperty, LayoutRootMI.Height);
                ItemDropDownMI.SetValue(Canvas.LeftProperty, LayoutRootMI.Width + 5);

                // height is the height of the textbox plus 10 since we add 10
                //for the box around the text box. Then multiply by count and add 10
                // for margins
                ItemDropDownMI.Height = (xy.Y + 10) * (items.Count) + 8;
                baseRectMI.Height = (xy.Y + 10) * (items.Count) + 8;


                foreach (MenuItem item in items)
                {
                    //set menuItem dimensions before adding
                    item.setDimension(xy);

                    item.parentMenuItem = this;
                    
                    itemHolderMI.Children.Add(item);
                }
            }
        }


        internal void setDimension(Point xy)
        {
            this.xy = xy;
        }

        public void CollapseDropDown()
        {
            if (isNested)
            {
                foreach (MenuItem item in items)
                {
                    item.CollapseDropDown();
                }
            }
            ItemDropDownMI.Visibility = Visibility.Collapsed;
        }

        internal void CollapseChildDropDownMenus()
        {
            foreach (MenuItem item in items)
            {
                item.CollapseDropDown();
            }
        }

        public void ShowDropDown()
        {
            ItemDropDownMI.Visibility = Visibility.Visible;
        }

        public Point getLargest(ObservableCollection<MenuItem> menuItems)
        {
            double width = 0;
            double height = 0;
            foreach (MenuItem item in menuItems)
            {
                Point xy = getTextWidthHeight(item.MenuText, ItemTextMI);
                if (xy.X > width) width = xy.X;
                if (xy.Y > height) height = xy.Y;
            }
            return new Point(width, height);
        }


        // return the dimension of the text after aplying styles from given textblock
        private Point getTextWidthHeight(string text, TextBlock itemTextTB)
        {
            TextBlock tb = new TextBlock();
            tb.Text = text;
            tb.Style = itemTextTB.Style;
            tb.FontFamily = itemTextTB.FontFamily;
            tb.FontSize = itemTextTB.FontSize;
            tb.FontSource = itemTextTB.FontSource;
            tb.FontStretch = itemTextTB.FontStretch;
            tb.FontStyle = itemTextTB.FontStyle;
            tb.FontWeight = itemTextTB.FontWeight;

            Point xy = new Point(tb.ActualWidth, tb.ActualHeight);
            return xy;
        }

        //Properties
        
        public static readonly DependencyProperty itemsProperty =
               DependencyProperty.Register("items", typeof(ObservableCollection<MenuItem>), typeof(MenuItem), null);

        public ObservableCollection<MenuItem> items
        {
            get { return (ObservableCollection<MenuItem>)GetValue(itemsProperty); }
            set { SetValue(itemsProperty, value); }
        }

        public static readonly DependencyProperty menuTextProperty =
                DependencyProperty.Register("MenuText", typeof(string), typeof(MenuItem), null);

        public string MenuText
        {
            get { return (string)GetValue(menuTextProperty); }
            set { SetValue(menuTextProperty, value); }
        }

        /// <summary>
        /// This is used only for passing information from XAML to the code behind, the menu control does not act on this
        /// </summary>
        public string NavigationURI { get; set; }

        //END Properties



        //EVENTS

        private void ItemHighlight_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "ItemHighlightedMI", true);
            //if it's a root level menuItem collapse all other drop downs
            if (parentMenuBarItem != null)
                parentMenuBarItem.CollapseChildDropDownMenus();
            else if (parentMenuItem != null)
                parentMenuItem.CollapseChildDropDownMenus();

        }

        private void ItemHighlight_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "noneHighlightedMI", true);
            
            
        }

        void ItemDropDownMI_MouseLeave(object sender, MouseEventArgs e)
        {
            CollapseDropDown();
            
        }

        private void ItemHighlight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isNested)
            {
                ShowDropDown();
            }
            else
            {
                if (Click != null)
                {
                    Click(this, new RoutedEventArgs());
                }
            }
            
        }

        //END EVENTS
        
       
    }
}
