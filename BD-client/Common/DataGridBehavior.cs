﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BD_client.Common
{
    /// <summary>
    /// Using this behavior on a dataGRid will ensure to display only columns with "Browsable Attributes"
    /// </summary>
    public static class DataGridBehavior
    {
        public static readonly DependencyProperty UseBrowsableAttributeOnColumnProperty =
            DependencyProperty.RegisterAttached("UseBrowsableAttributeOnColumn",
            typeof(bool),
            typeof(DataGridBehavior),
            new UIPropertyMetadata(false, UseBrowsableAttributeOnColumnChanged));

        public static bool GetUseBrowsableAttributeOnColumn(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseBrowsableAttributeOnColumnProperty);
        }

        public static void SetUseBrowsableAttributeOnColumn(DependencyObject obj, bool val)
        {
            obj.SetValue(UseBrowsableAttributeOnColumnProperty, val);
        }

        private static void UseBrowsableAttributeOnColumnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = obj as DataGrid;
            if (dataGrid != null)
            {
                if ((bool)e.NewValue)
                {
                    dataGrid.AutoGeneratingColumn += DataGridOnAutoGeneratingColumn;
                }
                else
                {
                    dataGrid.AutoGeneratingColumn -= DataGridOnAutoGeneratingColumn;
                }
            }
        }

        private static void DataGridOnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propDesc = e.PropertyDescriptor as PropertyDescriptor;

            if (propDesc != null)
            {
                foreach (Attribute att in propDesc.Attributes)
                {
                    var browsableAttribute = att as BrowsableAttribute;
                    if (browsableAttribute != null)
                    {
                        if (!browsableAttribute.Browsable)
                        {
                            e.Cancel = true;
                        }
                    }

                    // As proposed by "dba" stackoverflow user on webpage: 
                    // https://stackoverflow.com/questions/4000132/is-there-a-way-to-hide-a-specific-column-in-a-datagrid-when-autogeneratecolumns
                    // I added few next lines:
                    var displayName = att as DisplayNameAttribute;
                    if (displayName != null)
                    {
                        e.Column.Header = displayName.DisplayName;
                    }
                }
            }
        }
    }
}