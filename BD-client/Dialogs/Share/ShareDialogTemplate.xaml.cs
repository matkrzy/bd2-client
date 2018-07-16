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
using UserControl = System.Windows.Controls.UserControl;

namespace BD_client.Dialogs.Share
{
    /// <summary>
    /// Interaction logic for ShareDialogTemplate.xaml
    /// </summary>
    public partial class ShareDialogTemplate : UserControl
    {
        private readonly Action<ShareDialogTemplate> _closeActionConfirm;
        private readonly Action<ShareDialogTemplate> _closeActionCancel;


        public ShareDialogTemplate(Action<ShareDialogTemplate> closeActionConfirm, Action<ShareDialogTemplate> closeActionCancel)
        {
            InitializeComponent();
            _closeActionConfirm = closeActionConfirm;
            _closeActionCancel = closeActionCancel;

        }


        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            this._closeActionConfirm(this);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this._closeActionCancel(this);
        }


    }
}