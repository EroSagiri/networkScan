using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace networkScan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ip 列表
        private IpDataList ipList = new IpDataList();
        // 开始ip和结束ip
        private IpData d = new IpData();

        private bool isStop = false;

        public MainWindow()
        {
            InitializeComponent();

            // 绑定列表
            listBox.SetBinding(ListBox.ItemsSourceProperty, new Binding()
            {
                Source = ipList
            });

            // 绑定开始输入框
            start_ip_TextBox.SetBinding(TextBox.TextProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Source = d,
                Path = new PropertyPath("Start_ip")
            });

            // 绑定结束输入框
            end_ip_TextBox.SetBinding(TextBox.TextProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Source = d,
                Path = new PropertyPath("End_ip")
            });

        }

        /**
         * 扫描按钮点击事件
         */
        private void scan_Button_Click(object sender, RoutedEventArgs e)
        {
            scan_start();
            new Thread(exec_ping_test).Start();

        }

        private void exec_ping_test()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ipList.Clear();
            }));
            if (d.Start_ip.Length < 1)
            {
                MessageBox.Show("无效的开始ip", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    scan_end();
                }));
                return;
            }
            try
            {
                IPAddress.Parse(d.Start_ip);
            }
            catch (FormatException error)
            {
                MessageBox.Show("无效的开始ip", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    scan_end();
                }));
                return;
            }


            string[] startIpArray = d.Start_ip.Split(".");
            string[] endIpArray = d.End_ip.Split(".");


            if (startIpArray.Length != 4)
            {

            }

            if (endIpArray.Length != 4)
            {

            }
            int s1, s2, s3, s4, e1, e2, e3, e4;
            try
            {
                 s1 = int.Parse(startIpArray[0]);
                 s2 = int.Parse(startIpArray[1]);
                 s3 = int.Parse(startIpArray[2]);
                 s4 = int.Parse(startIpArray[3]);

                 e1 = int.Parse(endIpArray[0]);
                 e2 = int.Parse(endIpArray[1]);
                 e3 = int.Parse(endIpArray[2]);
                 e4 = int.Parse(endIpArray[3]);
            } catch (FormatException error)
            {
                MessageBox.Show("错误的ip");
                this.Dispatcher.Invoke(new Action(() =>
                {
                    scan_end();
                }));
                return;
            } catch(IndexOutOfRangeException err)
            {
                MessageBox.Show("错误的ip");
                this.Dispatcher.Invoke(new Action(() =>
                {
                    scan_end();
                }));
                return;
            }

            for (int i1 = s1; i1 <= e1; i1++)
            {
                if(isStop)
                {
                    break;
                }
                for (int i2 = s2; i2 <= e2; i2++)
                {
                    if(isStop)
                    {
                        break;
                    }
                    for (int i3 = s3; i3 <= e3; i3++)
                    {
                        if(isStop)
                        {
                            break;
                        }
                        for (int i4 = s4; i4 <= e4; i4++)
                        {
                            if(isStop)
                            {
                                break;
                            }
                            string ip = i1.ToString() + "." + i2.ToString() + "." + i3.ToString() + "." + i4.ToString();

                            var pingReply = ping_host_test(IPAddress.Parse(ip));

                            string msg = "";

                            if (pingReply.Status == IPStatus.Success)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    ipList.Add(ip);
                                    
                                }));
                                msg = "成功" + ip;

                            } else
                            {
                                msg = "失败" + ip;
                            }

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                label1.Content = msg;

                            }));
                            
                        }
                    }
                }
            }

            this.Dispatcher.Invoke(new Action(() =>
            {
                scan_end();
            }));
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            scan_stop();
        }

        private PingReply ping_host_test(IPAddress ipaddress, int timeout = 100)
        {
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(ipaddress, timeout);

            return pingReply;
        }

        private void start_ip_TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void scan_start()
        {
            isStop = false;
            scanButton.IsEnabled = false;
            stopButton.IsEnabled = true;
        }

        private void scan_stop()
        {
            isStop = true;
        }

        private void scan_end()
        {
            scanButton.IsEnabled = true;
            stopButton.IsEnabled = false;
        }

        private void end_ip_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back)
            {
                if (end_ip_TextBox.Text.Length < 1)
                {
                    start_ip_TextBox.Focus();
                }
            }
        }
    }
}
