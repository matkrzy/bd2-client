using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.ViewModels
{
    class HelpPageViewModel : INotifyPropertyChanged
    {

        private IDialogCoordinator dialogCoordinator;
        public event PropertyChangedEventHandler PropertyChanged = null;

        public HelpPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
        }
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

}
