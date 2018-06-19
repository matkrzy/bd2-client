using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.ViewModels;

namespace BD_client.Dialogs.Share
{
    public class ShareDialog : INotifyPropertyChanged
    {
        public String Message { get; set; }

        public string Email { get; set; }

        public bool MakePublic { get; set; }


        public ShareDialog()
        {
        }

        public void SetMessage(String message)
        {
            Message = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}