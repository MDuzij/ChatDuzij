﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Threading;
using ChatClient;
using ChatClient.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNet.SignalR.Client.Hubs;
using OpenChatClient.Models;

namespace OpenChatClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IChatClientService chatSerivce;
        private string _username;
        private string _password;
        private string _errorLabel = string.Empty;

        public LoginViewModel(IChatClientService service)
        {
            chatSerivce = service;
            chatSerivce.chatProxy.On("Login", (valid) => { this.Login(valid); });
            chatSerivce.connection.Start();
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string ErrorLabel
        {
            get { return _errorLabel; }
            set { Set(ref _errorLabel, value); }
        }

        public RelayCommand<object> LoginCommand => new RelayCommand<object>(OnLoginCommand);

        public async void OnLoginCommand(object commandParameter)
        {
            try
            {
                await chatSerivce.chatProxy.Invoke("Login", Username, ((PasswordBox)commandParameter).Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login(bool valid)
        {
            if (valid)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    var win = new MainWindow();
                    win.Show();
                    Messenger.Default.Send(new NotificationMessage<string>(Username, "token"));
                    App.Current.MainWindow.Close();
                });
            }
            else
            {
                ErrorLabel = "User already loged in";
            }
        }
    }
}