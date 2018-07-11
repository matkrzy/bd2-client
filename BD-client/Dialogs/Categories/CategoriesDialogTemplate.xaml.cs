using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BD_client.Dialogs.Share;
using MahApps.Metro.Controls.Dialogs;
using UserControl = System.Windows.Controls.UserControl;

namespace BD_client.Dialogs.Categories
{
    public partial class CategoriesDialogTemplate : UserControl
    {
        private readonly Action<CategoriesDialogTemplate> _closeAction;
        private readonly Action<CategoriesDialogTemplate> _openAddCategoryInput;

        public CategoriesDialogTemplate(Action<CategoriesDialogTemplate> closeAction,
            Action<CategoriesDialogTemplate> openAddCategoryInput)
        {
            InitializeComponent();
            _closeAction = closeAction;
            _openAddCategoryInput = openAddCategoryInput;
        }

        private void AddCategoryClick(object sender, RoutedEventArgs e)
        {
            this._openAddCategoryInput(this);
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            this._closeAction(this);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this._closeAction(this);
        }
    }
}