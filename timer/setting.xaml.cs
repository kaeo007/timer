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

            while (savedKeyName.Count < 8)
                savedKeyName.Add(Key.None);

            // 풀그 or 콘체
            setting_key1.Text = savedKeyName[0] == Key.None ? "" : savedKeyName[0].ToString();
            // 백귀 칭호키
            setting_key2_1.Text = savedKeyName[1] == Key.None ? "" : savedKeyName[1].ToString();
            // 백귀 주력기1
            setting_key2_2.Text = savedKeyName[2] == Key.None ? "" : savedKeyName[2].ToString();
            // 백귀 주력기2
            setting_key2_3.Text = savedKeyName[3] == Key.None ? "" : savedKeyName[3].ToString();
            // 황혼
            setting_key3.Text = savedKeyName[4] == Key.None ? "" : savedKeyName[4].ToString();
            // 각성
            setting_key_awaken.Text = savedKeyName[5] == Key.None ? "" : savedKeyName[5].ToString();
            // 빼꼼양파
            setting_key_onion.Text = savedKeyName[6] == Key.None ? "" : savedKeyName[6].ToString();
            // 칭호 스위칭 키
            setting_key_change_title.Text = savedKeyName[7] == Key.None ? "" : savedKeyName[7].ToString();


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

        // json 저장시 저장값 밀리는거 방지. 키값이 없으면 Key.None으로 저장
        private Key ParseOrNone(string text)
        {
            if (Enum.TryParse<Key>(text, out var k))
                return k;

            return Key.None;
        }


        // 저장후 메인화면
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            savedKeyName.Clear();

            savedKeyName.Add(ParseOrNone(setting_key1.Text));          // 풀그 or 콘체
            savedKeyName.Add(ParseOrNone(setting_key2_1.Text));        // 백귀 칭호키
            savedKeyName.Add(ParseOrNone(setting_key2_2.Text));        // 백귀 주력기1
            savedKeyName.Add(ParseOrNone(setting_key2_3.Text));        // 백귀 주력기2
            savedKeyName.Add(ParseOrNone(setting_key3.Text));          // 황혼
            savedKeyName.Add(ParseOrNone(setting_key_awaken.Text));    // 각성
            savedKeyName.Add(ParseOrNone(setting_key_onion.Text));     // 양파
            savedKeyName.Add(ParseOrNone(setting_key_change_title.Text)); // 칭호변환키


            NavigationService.Navigate(new main());

            string json = JsonSerializer.Serialize(savedKeyName);
            File.WriteAllText("saved_keys.json", json);


        }

        // 초기화
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
            setting_key_awaken.Text = "";
            setting_key_onion.Text = "";
            setting_key_change_title.Text = "";
            savedKeyName.Clear();

        }
    }
}

