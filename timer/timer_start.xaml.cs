using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;
using static timer.setting;
using System.Timers;


namespace timer
{

    public partial class timer_start : Page
    {
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        DispatcherTimer timer3 = new DispatcherTimer();
        static int real_time1 = 0;
        static int real_time2 = 0;
        static int real_time3 = 0;

        // 키보드 입력 구조체
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // 델리게이트 정의
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // WinAPI 함수 정의
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        // 상수 정의
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x0100;

        // 후크 관련 변수
        private static IntPtr hhook = IntPtr.Zero;
        private  LowLevelKeyboardProc _proc;
        public timer_start()
        {
            InitializeComponent();
            _proc = hookPorc;

            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += Timer1_Tick;

            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick += Timer2_Tick;

            timer3.Interval = TimeSpan.FromSeconds(1);
            timer3.Tick += Timer3_Tick;

            SetHook();
        }

        private void SetHook()
        {
            IntPtr hInstance = LoadLibrary("user32.dll");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        private static void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        static Queue<Key> keysequence = new Queue<Key>();

        private void Timer1_Tick(object sender, EventArgs e)
        {
            real_time1--;
            title_timer1.Text = real_time1.ToString()+" 초";

            if (real_time1 <= 0)
            {
                timer1.Stop();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            real_time2--;
            title_timer2.Text = real_time2.ToString() + " 초";

            if (real_time2 <= 0)
            {
                timer2.Stop();
            }
        }
        private void Timer3_Tick(object sender, EventArgs e)
        {
            real_time3--;
            title_timer3.Text = real_time3.ToString() + " 초";

            if (real_time3 <= 0)
            {
                timer3.Stop();
            }
        }




        public IntPtr hookPorc(int code, IntPtr wParam, IntPtr lParam)
        {

            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {

                var keyInfo = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                Key vkCode = KeyInterop.KeyFromVirtualKey((int)keyInfo.vkCode);

                keysequence.Enqueue(vkCode);

                if (keysequence.Count > 3)
                {
                    keysequence.Dequeue();
                }
                if (keysequence.Count == 3)
                {
                    var keys = keysequence.ToArray();


                    // 풀그 (칭호 스위칭 키 + 스위칭 방향 + 각성)
                    if (!timer1.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[0] && keys[2] == savedKeyName[5])
                       
                    {
                        keysequence.Clear();

                        real_time1 = 60;

                        timer1.Start();

                    }




                    // 백귀 (칭호 스위칭 키 + 스위칭 방향 + 주력기 스킬키) savedKeyName 1 칭호키 /savedKeyName 2,3 가 주력기 두개
                    else if ((!timer2.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[1] && keys[2] == savedKeyName[2]) ||
                       (!timer2.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[1] && keys[2] == savedKeyName[3]))
                    {
                        keysequence.Clear();

                        real_time2 = 25;

                        timer2.Start();

                    }

                    // 황혼 (칭호 스위칭 키 + 스위칭 방향 + 각성키) 
                    else if (!timer3.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[4] && keys[2] == savedKeyName[5])
                    {
                        keysequence.Clear();

                        real_time3 = 30;

                        timer3.Start();

                    }

                    // 빼꼼 양파 (풀그) (칭호 스위칭 키 +  스위칭(풀그) 방향 + 양파 키) 
                    else if (!timer1.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[0] && keys[2] == savedKeyName[6])
                    {
                        keysequence.Clear();

                        real_time1 = 60;

                        timer1.Start();

                    }

                    // 빼꼼 양파 (황혼) (칭호 스위칭 키 +  스위칭(황혼) 방향 + 양파 키) 
                    else if (!timer1.IsEnabled && keys[0] == savedKeyName[7] && keys[1] == savedKeyName[4] && keys[2] == savedKeyName[6])
                    {
                        keysequence.Clear();

                        real_time3 = 30;

                        timer3.Start();

                    }

                    // 딜 칭호로 입장한다는 기준 -> 풀그로 입장하면 처음 풀그는 카운트 안돌아감

                }
            }


            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }


        // (초기화 후) 메인화면 버튼
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnHook();
            timer1.Stop();
            real_time1 = 60;
            title_timer1.Text = "60 초";
            timer2.Stop();
            real_time2 = 25;
            title_timer2.Text = "25 초";
            timer3.Stop();
            real_time3 = 30;
            title_timer3.Text = "30 초";
            NavigationService.Navigate(new main());
        }


        // 초기화 버튼
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            timer1.Stop();
            real_time1 = 60;
            title_timer1.Text = "60 초";
            timer2.Stop();
            real_time2 = 25;
            title_timer2.Text = "25 초";
            timer3.Stop();
            real_time3 = 30;
            title_timer3.Text = "30 초";

        }
    }
}


