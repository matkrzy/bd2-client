using BD_client.Common;
using BD_client.Services;
using BD_client.ViewModels;
using BD_client.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Dialogs.Share;
using BD_client.Dto;
using BD_client.Enums;

namespace BD_client.Pages
{
    /// <summaryPhotos
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private CategoriesPageViewModel ViewModel = new CategoriesPageViewModel();


        /// <summary>
        /// Konstruktor
        /// </summary>
        public CategoriesPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            
        }
    }
}