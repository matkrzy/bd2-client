using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BD_client.Dialogs
{
    public class CustomDialog : BaseMetroDialog
    {
        public CustomDialog()
            : this(null, null)
        {
        }

        public CustomDialog(MetroWindow parentWindow)
            : this(parentWindow, null)
        {
        }

        public CustomDialog(MetroDialogSettings settings)
            : this(null, settings)
        {
        }

        public CustomDialog(MetroWindow parentWindow, MetroDialogSettings settings)
            : base(parentWindow, settings)
        {
        }
    }
}
