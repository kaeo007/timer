using System;
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
using static timer.timer_start;
using System.IO;
using System.Text.Json;


namespace timer
{
    /// <summary>
    /// setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class setting : Page
    {
        public static List<Key> savedKeyName = new List<Key>();

        public setting()
        {
            InitializeComponent();

            if (File.Exists("saved_keys.json"))
            {
                string json = File.ReadAllText("saved_keys.json");
                savedKeyName = JsonSerializer.Deserialize<List<Key>>(json);
            }

            if (savedKeyName.Count > 0)
                setting_key1.Text = savedKeyName[0].ToString();
            if (savedKeyName.Count > 1)
                setting_key2_1.Text = savedKeyName[1].ToString();
            if (savedKeyName.Count > 2)
                setting_key2_2.Text = savedKeyName[2].ToString();
            if (savedKeyName.Count > 3)
                setting_key2_3.Text = savedKeyName[3].ToString();
            if (savedKeyName.Count > 4)
                setting_key3.Text = savedKeyName[4].ToString();


        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string keyName = e.Key.ToString();
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = null;
                tb.Text += keyName;
            }


            e.Handled = true; // 기본 커서 이동, 방향키 동작 막기
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            savedKeyName.Clear();

            if (Enum.TryParse<Key>(setting_key1.Text, out var key1))
                savedKeyName.Add(key1);

            // 백귀 칭호키
            if (Enum.TryParse<Key>(setting_key2_1.Text, out var key2_1))
                savedKeyName.Add(key2_1);
            
            // 주력기1
            if (Enum.TryParse<Key>(setting_key2_2.Text, out var key2_2))
                savedKeyName.Add(key2_2);

            // 주력기2
            if (Enum.TryParse<Key>(setting_key2_3.Text, out var key2_3))
                savedKeyName.Add(key2_3);

            if (Enum.TryParse<Key>(setting_key3.Text, out var key3))
                savedKeyName.Add(key3);


            NavigationService.Navigate(new main());

            string json = JsonSerializer.Serialize(savedKeyName);
            File.WriteAllText("saved_keys.json", json);


        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {

            if (File.Exists("saved_keys.json"))
            {
                File.Delete("saved_keys.json");
            }

            setting_key1.Text = "";
            setting_key2_1.Text = "";
            setting_key2_2.Text = "";
            setting_key2_3.Text = "";
            setting_key3.Text = "";
            savedKeyName.Clear();

        }
    }
}

