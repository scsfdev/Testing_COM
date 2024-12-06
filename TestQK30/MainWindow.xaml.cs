using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using TestQK30.ViewModel;
using System;

namespace TestQK30
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            Messenger.Default.Register<string>(this, "MSG", ShowMsg);

            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void ShowMsg(string strMsg)
        {
            MessageBox.Show(strMsg, "QK30", MessageBoxButton.OK);
        }
    }
}