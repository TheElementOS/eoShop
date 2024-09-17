using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eoShop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webView21.Dock = DockStyle.Fill;

            // Rejestracja zdarzenia po inicjalizacji CoreWebView2
            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;
            webView21.NavigationCompleted += WebView21_NavigationCompleted;

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentDirectory, "main", "index.html");

            webView21.Source = new Uri($"file:///{filePath.Replace("\\", "/")}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void WebView21_CoreWebView2InitializationCompleted(object sender, EventArgs e)
        {
            // Rejestracja zdarzenia WebMessageReceived po inicjalizacji CoreWebView2
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private void WebView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Możesz użyć tego zdarzenia, aby potwierdzić zakończenie nawigacji
        }

        private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string message = e.TryGetWebMessageAsString();
            if (message == "runCommand")
            {
                await RunCommandAsync("echo ok");
            }
        }

        private Task RunCommandAsync(string command)
        {
            return Task.Run(() =>
            {
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/c {command}")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        MessageBox.Show(result); // Możesz też wyświetlić wynik w innym miejscu
                    }
                }
            });
        }
    }
}
