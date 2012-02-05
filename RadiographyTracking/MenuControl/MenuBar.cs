using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace MenuControl
{
    [TemplatePart(Name="baseRectMB", Type = typeof(StackPanel))]
    [ContentPropertyAttribute("items")]
    public class MenuBar : Control
    {
        //graphic items
        public StackPanel baseRectMB;

        public MenuBar()
        {            
            this.DefaultStyleKey = typeof(MenuBar);
            this.Loaded += new RoutedEventHandler(MenuBar_Loaded);
            
            if (items == null)
                items = new ObservableCollection<MenuBarItem>();           
            
        }

      
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            baseRectMB = (StackPanel)GetTemplateChild("baseRectMB");
            foreach (MenuBarItem item in items)
            {
                item.parentMenu = this;
            }

        }

        public void MenuBar_Loaded(object sender, EventArgs e)
        {
            this.ApplyTemplate();

            foreach (MenuBarItem item in items)
            {
                baseRectMB.Children.Add(item);
            }
        }

        public static readonly DependencyProperty itemsProperty =
                DependencyProperty.Register("items", typeof(ObservableCollection<MenuBarItem>), typeof(MenuBar), null);

        public ObservableCollection<MenuBarItem> items
        {
            get { return (ObservableCollection<MenuBarItem>)GetValue(itemsProperty); }
            set { SetValue(itemsProperty, value); }
        }


        internal void CollapseChildDropDownMenus()
        {
            foreach (MenuBarItem item in items)
            {
                item.CollapseDropDown();
            }
        }
    }
}
