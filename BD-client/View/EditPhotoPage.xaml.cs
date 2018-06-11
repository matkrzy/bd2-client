﻿using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for EditPhotoPage.xaml
    /// </summary>
    public partial class EditPhotoPage : Page
    {
        EditPhotoPageViewModel vm = new EditPhotoPageViewModel(DialogCoordinator.Instance);
        public EditPhotoPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

    }
}
