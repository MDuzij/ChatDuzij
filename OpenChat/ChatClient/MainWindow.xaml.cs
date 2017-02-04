﻿using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using ChatClient.ViewModel;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            ((INotifyCollectionChanged)ChatView.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(ChatViewSourceChanged);
        }

        private void ChatViewSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ChatView.Items.MoveCurrentToLast();
                ChatView.ScrollIntoView(ChatView.Items.CurrentItem);
            }
        }

        private void Contacts_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (MainViewModel)DataContext;
            if (viewModel.LoadRoomMessages.CanExecute(null))
                viewModel.LoadRoomMessages.Execute(null);
        }
    }
}