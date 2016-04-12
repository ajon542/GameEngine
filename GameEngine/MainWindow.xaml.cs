using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

using GameEngine.ViewModel;
using Microsoft.Win32.SafeHandles;

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        public MainWindow()
        {
            // Create a console window for debug output.
            AllocConsole();
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            InitializeComponent();

            // Example of switching between views based on the view-model.
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationShutdown();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void ApplicationShutdown()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
