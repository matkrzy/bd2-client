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

        public CategoriesDialogTemplate(Action<CategoriesDialogTemplate> closeAction)
        {
            InitializeComponent();
            _closeAction = closeAction;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            CategoriesDialog context = (CategoriesDialog)this.DataContext;
            context.UpdatePhotoCategories();
            this._closeAction(this);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this._closeAction(this);
        }
    }
}