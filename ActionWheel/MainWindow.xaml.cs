using Gma.System.MouseKeyHook;
using HenoohDeviceEmulator;
using RadialMenuDemo.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using WPFTabTip;

namespace RadialMenuDemo
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    };

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("User32.dll")]
        static extern bool ClipCursor([In()]ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool ClipCursor([In()]IntPtr lpRect);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        const int VK_MEDIA_NEXT_TRACK = 0xB0;
        const int VK_MEDIA_PREV_TRACK = 0xB1;
        const int VK_MEDIA_STOP = 0xB2;
        const int VK_VOLUME_MUTE = 0xAD;
        const int VK_RWIN = 0x5C;
        const int VK_SLEEP = 0x5F;
        const int VK_LWIN = 0x5B;
        const int VK_C = 0x43;
        const int VK_F5 = 0x74;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

        KeyboardController kb;

        bool flagOpen1 = false;
        bool flagOpen2 = false;
        bool flagOpen3 = false;
        
        bool openRightMenu = false;

        int middleScreenPosH;
        int middleScreenPosV;

        DispatcherTimer timerOpenRightMenu;

        private bool _isOpen1 = false;
        public bool IsOpen1
        {
            get
            {
                return _isOpen1;
            }
            set
            {
                _isOpen1 = value;
                RaisePropertyChanged();
            }
        }

        private bool _isOpen2 = false;
        public bool IsOpen2
        {
            get
            {
                return _isOpen2;
            }
            set
            {
                _isOpen2 = value;
                RaisePropertyChanged();
            }
        }

        private bool _isOpen3 = false;
        public bool IsOpen3
        {
            get
            {
                return _isOpen3;
            }
            set
            {
                _isOpen3 = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CloseRadialMenu1
        {
            get
            {
                return new RelayCommand(() => IsOpen1 = false);
            }
        }
        public ICommand OpenRadialMenu1
        {
            get
            {
                return new RelayCommand(() => { IsOpen1 = true; IsOpen2 = false; IsOpen3 = false; });
            }
        }

        public ICommand CloseRadialMenu2
        {
            get
            {
                return new RelayCommand(() => IsOpen2 = false);
            }
        }
        public ICommand OpenRadialMenu2
        {
            get
            {
                return new RelayCommand(() => { IsOpen2 = true; IsOpen1 = false; IsOpen3 = false; });
            }
        }

        public ICommand CloseRadialMenu3
        {
            get
            {
                return new RelayCommand(() => IsOpen3 = false);
            }
        }
        public ICommand OpenRadialMenu3
        {
            get
            {
                return new RelayCommand(() => { IsOpen3 = true; IsOpen1 = false; IsOpen2 = false; });
            }
        }

        public ICommand Empty
        {
            get
            {
                return new RelayCommand(() => System.Diagnostics.Debug.WriteLine("Empty"),
                    () =>
                    {
                        return false;
                    });
            }
        }

        public ICommand OpenNotifications
        {
            get
            {
                return new RelayCommand(() => { kb.Window(HenoohDeviceEmulator.Native.VirtualKeyCode.VK_A); });

            }
        }

        public ICommand OpenKeyboard
        {
            get
            {
                return new RelayCommand(() => { TabTip.Open(); });

            }
        }

        public ICommand OpenCortana
        {
            get
            {
                return new RelayCommand(() => { kb.Window(HenoohDeviceEmulator.Native.VirtualKeyCode.VK_Q); });

            }
        }

        public ICommand OpenTaskMngr
        {
            get
            {
                return new RelayCommand(() => { Process.Start("taskmgr"); });

            }
        }

        public ICommand RunSleep
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_SLEEP, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand PlayPause
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand NextTrack
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand Mute
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand Stop
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_MEDIA_STOP, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand PrevTrack
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand Refresh
        {
            get
            {
                return new RelayCommand(() => { keybd_event((byte)VK_F5, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); });

            }
        }

        public ICommand NextPage
        {
            get
            {
                return new RelayCommand(() => { kb.Alt(HenoohDeviceEmulator.Native.VirtualKeyCode.RIGHT); });

            }
        }

        public ICommand FullScreen
        {
            get
            {
                return new RelayCommand(() => { kb.Press(HenoohDeviceEmulator.Native.VirtualKeyCode.F11, new TimeSpan(0)); });

            }
        }

        public ICommand YoutubeNext
        {
            get
            {
                return new RelayCommand(() => { kb.Shift(HenoohDeviceEmulator.Native.VirtualKeyCode.VK_N); });

            }
        }

        public ICommand YoutubePlay
        {
            get
            {
                return new RelayCommand(() => { kb.Press(HenoohDeviceEmulator.Native.VirtualKeyCode.SPACE, new TimeSpan(0)); });

            }
        }

        public ICommand YoutubePrev
        {
            get
            {
                return new RelayCommand(() => { kb.Shift(HenoohDeviceEmulator.Native.VirtualKeyCode.VK_P); });

            }
        }

        public ICommand NewTab
        {
            get
            {
                return new RelayCommand(() => { kb.Control(HenoohDeviceEmulator.Native.VirtualKeyCode.VK_T); });

            }
        }

        public ICommand PrevPage
        {
            get
            {
                return new RelayCommand(() => { kb.Alt(HenoohDeviceEmulator.Native.VirtualKeyCode.LEFT); });

            }
        }
                                
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            timerOpenRightMenu = new DispatcherTimer();
            timerOpenRightMenu.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerOpenRightMenu.Tick += new EventHandler(timerMenu_Tick);
            TabTipAutomation.IgnoreHardwareKeyboard = HardwareKeyboardIgnoreOptions.IgnoreAll;
            
            kb = new KeyboardController();
            Subscribe();
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Topmost = true;
            this.Top = 0;
            this.Left = 0;
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }
        
        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += GlobalHookKeyPress;
            m_GlobalHook.KeyUp += GlobalHookKeyUp;
        }

        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string keydown = e.KeyCode.ToString();
            if ((keydown == "F7")||(keydown == "F8"))
            {
                if (openRightMenu == false)
                {
                    timerOpenRightMenu.Start();
                }
            }
            
            if (openRightMenu == true)
            {

                byte[] keys = new byte[256];
                GetKeyboardState(keys);

                string keyDown = e.KeyCode.ToString();

                if ((keys[(int)Keys.F7] & keys[(int)Keys.F8] & 128) == 128)
                {
                    
                    System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    OpenRadialMenu3.Execute(null);
                    CloseRadialMenu2.Execute(null);
                    CloseRadialMenu1.Execute(null);
                    double height = Screen.PrimaryScreen.WorkingArea.Height / 2;
                    double width = Screen.PrimaryScreen.WorkingArea.Width / 2;

                    middleScreenPosH = (int)width;
                    middleScreenPosV = (int)height;

                    if (flagOpen3 == false)
                    {
                        SetCursorPos(middleScreenPosH, middleScreenPosV);
                        flagOpen1 = false;
                        flagOpen2 = false;
                        flagOpen3 = true;
                        
                        RECT r;

                        r.left = (int)(width - 90);
                        r.top = (int)(height - 90);
                        r.right = (int)(width + 90);
                        r.bottom = (int)(height + 90);
                        ClipCursor(ref r);
                    }
                   
                }
                else if ((keys[(int)Keys.F7] & 128) == 128)
                {
                   
                    System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    OpenRadialMenu1.Execute(null);
                    CloseRadialMenu2.Execute(null);
                    CloseRadialMenu3.Execute(null);
                    double height = Screen.PrimaryScreen.WorkingArea.Height / 2;
                    double width = Screen.PrimaryScreen.WorkingArea.Width / 2;

                    middleScreenPosH = (int)width;
                    middleScreenPosV = (int)height;

                    if (flagOpen1 == false)
                    {
                        SetCursorPos(middleScreenPosH, middleScreenPosV);
                        flagOpen1 = true;
                        flagOpen2 = false;
                        flagOpen3 = false;
                        
                        RECT r;

                        r.left = (int)(width - 90);
                        r.top = (int)(height - 90);
                        r.right = (int)(width + 90);
                        r.bottom = (int)(height + 90);



                        ClipCursor(ref r);
                    }
                   
                }
                else if ((keys[(int)Keys.F8] & 128) == 128)
                {
                    
                    System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    OpenRadialMenu2.Execute(null);
                    CloseRadialMenu1.Execute(null);
                    CloseRadialMenu3.Execute(null);
                    double height = Screen.PrimaryScreen.WorkingArea.Height / 2;
                    double width = Screen.PrimaryScreen.WorkingArea.Width / 2;

                    middleScreenPosH = (int)width;
                    middleScreenPosV = (int)height;

                    if (flagOpen2 == false)
                    {
                        SetCursorPos(middleScreenPosH, middleScreenPosV);
                        flagOpen2 = true;
                        flagOpen1 = false;
                        flagOpen3 = false;
                        
                        RECT r;

                        r.left = (int)(width - 90);
                        r.top = (int)(height - 90);
                        r.right = (int)(width + 90);
                        r.bottom = (int)(height + 90);
                        ClipCursor(ref r);

                       

                    }
                   
                }
            }
            
        }
        
        private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string keyUp = e.KeyCode.ToString();
            

            if(keyUp == "F7")
            {
                if(flagOpen1 == true)
                {
                    CloseRadialMenu1.Execute(null);
                    flagOpen3 = false;
                    flagOpen2 = false;
                    flagOpen1 = false;

                    ClipCursor(IntPtr.Zero);
                    
                    openRightMenu = false;
                    
                }
                else if(flagOpen3 == true)
                {
                    CloseRadialMenu3.Execute(null);
                    flagOpen3 = false;
                    flagOpen2 = false;
                    flagOpen1 = false;
                    ClipCursor(IntPtr.Zero);
                    
                    openRightMenu = false;
                    
                }
            }
            else if(keyUp == "F8")
            {
                if (flagOpen2 == true)
                {
                    CloseRadialMenu2.Execute(null);
                    flagOpen3 = false;
                    flagOpen2 = false;
                    flagOpen1 = false;
                    ClipCursor(IntPtr.Zero);
                   
                    openRightMenu = false;

                    
                   
                }
                else if(flagOpen3 == true)
                {
                    CloseRadialMenu3.Execute(null);
                    flagOpen3 = false;
                    flagOpen2 = false;
                    flagOpen1 = false;
                    ClipCursor(IntPtr.Zero);
                    
                    openRightMenu = false;
                    
                }
            }
            
        }
        
        void timerMenu_Tick(object sender, EventArgs e)
        {
            openRightMenu = true;
            timerOpenRightMenu.Stop();
        }
        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= GlobalHookKeyPress;
            m_GlobalHook.Dispose();
        }
        
    }
   
}
