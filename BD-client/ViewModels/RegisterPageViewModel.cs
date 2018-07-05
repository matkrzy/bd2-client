using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using RestSharp;

namespace BD_client.ViewModels
{
    class RegisterPageViewModel 
    {
        private IDialogCoordinator dialogCoordinator;

        public ICommand CancelCmd { get; set; }
        public ICommand RegisterCmd { get; set; }
        public String Password { get; set; } = "";
        public String RepeatedPassword { get; set; } = "";
        public String FirstName { get; set; } = "";
        public String LastName { get; set; } = "";
        public String Email { get; set; } = "";


        public RegisterPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            CancelCmd = new RelayCommand(x => Cancel());
            RegisterCmd = new RelayCommand(x => Register());
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "LogInPage.xaml";
        }

        private async void Register()
        {
            if (Email.Equals(""))
            {
                await dialogCoordinator.ShowMessageAsync(this, "Validation", "Email cannot be empty");
                return;
            }

            if (Password.Equals(""))
            {
                await dialogCoordinator.ShowMessageAsync(this, "Validation", "Password cannot be empty");
                return;
            }

            if (!Password.Equals(RepeatedPassword))
            {
                await dialogCoordinator.ShowMessageAsync(this, "Validation", "Password does not match");
                return;
            }

            var values = new { firstName = FirstName, lastName = LastName, email = Email, password = Password };
            //User user = new User() {FirstName = FirstName, LastName = LastName, Email = Email, Password = Password};
            IRestResponse response = await new Request("/users").DoPost(values);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "User registered");
                MainWindow.MainVM.Page = "LogInPage.xaml";
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Ooops..", "Register failed. Try again");
            }
        }
    }
}