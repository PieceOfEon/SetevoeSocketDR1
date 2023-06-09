﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetevoeSocketDR1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonServer_Click(object sender, RoutedEventArgs e)
        {
            WindowServer windowServer = new WindowServer();
            windowServer.Show();
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            WindowClient windowClient = new WindowClient();
            windowClient.Show();
        }
    }
}
