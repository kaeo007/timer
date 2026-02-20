using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Navigation;

namespace timer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("saved_keys.json"))
            {
                MessageBox.Show("설정 먼저 해주세요.");
                return;
            }
            else
            {
                MainFrame.Navigate(new timer_start());
            }

        }
    }
}


