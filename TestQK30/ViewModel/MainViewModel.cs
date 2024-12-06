using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using TestQK30.Model;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.IO;
using System.Collections.Generic;

namespace TestQK30.ViewModel
{
    public class Tag
    {
        public string Data { get; set; }
        public string DT { get; set; }
        public string Status { get; set; }
    }

    public class VisitorPass
    {
        public string ID { get; set; }
        public DateTime ExpiryDate { get; set; }
    }


    public class MainViewModel : ViewModelBase
    {
        DispatcherTimer dt = new DispatcherTimer();
        SerialPort sr  = null;

        public ICommand CmdOK { get; private set; }
        public ICommand CmdNG { get; private set; }
        public ICommand CmdClear { get; private set; }
        public ICommand CmdConnect { get; private set; }
        public ICommand CmdDisconnect { get; private set; }


        public MainViewModel(IDataService dataService)
        {
            CmdOK = new RelayCommand(SendOKSignal);
            CmdNG = new RelayCommand(SendNGSignal);
            CmdClear = new RelayCommand(ResetForm);
            CmdConnect = new RelayCommand(ConnectCOM);
            CmdDisconnect = new RelayCommand(DisconnectCOM);
            EnableControl = false;
            EnableConnect = true;

            FileStatus = "  ";
            ReadDataList = null;
            ReadDataList = new ObservableCollection<Tag>();

            dt.Interval = TimeSpan.FromMilliseconds(500);
            dt.Tick += Dt_Tick;

            dt.IsEnabled = false;
            ComPort = "5";
            sr = new SerialPort();
            sr.DataReceived += Sr_DataReceived;
        }

        private void DisconnectCOM()
        {
            BarData = "";
            if (sr != null)
            {
                sr.DiscardInBuffer();
                sr.DiscardOutBuffer();
                sr.Close();
            }

            FileStatus = "  ";
            ShowHidden = false;
            ReadDataList = null;
            ReadDataList = new ObservableCollection<Tag>();
            Visitors = null;

            EnableControl = false;
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            BarData = "";
            dt.IsEnabled = false;
        }

        private bool IsValidPort()
        {
            string strPort = comPort;
            int iPort = 0;
            bool bOk = int.TryParse(strPort, out iPort);
            if (iPort <= 0)
                return false;
            else
                return bOk;
        }

        private int ConvertPort()
        {
            int iPort = 0;
            iPort = int.Parse(comPort);
            return iPort;
        }


        private void ConnectCOM()
        {
            try
            {
                EnableControl = false;

                if (string.IsNullOrEmpty(comPort) || !IsValidPort())
                {
                    Messenger.Default.Send("<Warning>" + Environment.NewLine + "Please key in valid COM port number 'in numeric only and greater than 0'.", "MSG");
                    return;
                }

                if (sr.IsOpen != false)
                    sr.Close();

                Thread.Sleep(500);

                ComPort = ConvertPort().ToString();

                if (sr.IsOpen == false)
                {
                    sr.PortName = "COM" + comPort;
                }

                sr.Open();

                sr.DiscardInBuffer();
                sr.DiscardOutBuffer();

                if (LoadDataFile())
                    FileStatus = "OK";
                else
                    FileStatus = "NG";

                EnableControl = true;
            }
            catch (Exception e)
            {
                EnableControl = false;
                Messenger.Default.Send("<Error>" + Environment.NewLine + "Details: " + e.Message, "MSG");
            }            
        }

        public List<VisitorPass> Visitors { get; set; }

        private DateTime ConvertDT(string strData)
        {
            DateTime dt = new DateTime();
            DateTime.TryParse(strData, out dt);

            return dt;
        }

        private bool LoadDataFile()
        {
            if (!File.Exists("DATA.csv"))
                return false;
            try
            {
                string[] strDatas = File.ReadAllLines("DATA.csv");

                Visitors = new List<VisitorPass>();

                foreach (string strData in strDatas)
                {
                    if (string.IsNullOrEmpty(strData))
                        continue;

                    string[] strLine = strData.Split(new string[] { "," }, StringSplitOptions.None);
                    if (strLine.Length < 2)
                        continue;

                    VisitorPass vp = new VisitorPass();
                    vp.ID = strLine[0].Trim();
                    vp.ExpiryDate = ConvertDT(strLine[1]);

                    Visitors.Add(vp);
                }

                 return true;
            }
            catch (Exception e)
            {
                Messenger.Default.Send("<Error>" + Environment.NewLine + "Details: " + e.Message, "MSG");
                return false;
            }
        }


        private string fileStatus;
        public string FileStatus
        {
            get { return fileStatus; }
            set { Set(ref fileStatus, value); }
        }


        private bool enableControl;
        public bool EnableControl
        {
            get { return enableControl; }
            set
            {
                Set(ref enableControl, value);

                EnableConnect = !value;
            }
        }



        private bool enableConnect;
        public bool EnableConnect
        {
            get { return enableConnect; }
            set { Set(ref enableConnect, value); }
        }


        private bool showHidden;
        public bool ShowHidden
        {
            get { return showHidden; }
            set { Set(ref showHidden, value); }
        }



        private void Sr_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string strReadData = "";
            string strHidden = "";
            strReadData = sr.ReadTo("\r");

            if (strReadData.Contains("*"))
            {
                strHidden = strReadData.Substring(strReadData.IndexOf("*") + 1);
                if (showHidden)
                    strReadData = strHidden;
                else
                    strReadData = strReadData.Substring(0, strReadData.IndexOf("*"));
            }

            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Tag t = new Tag();
                t.Data = strReadData;
                t.DT = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                if (fileStatus.ToUpper() == "OK")
                {
                    var vPass = (from v in Visitors
                                 where v.ID == (strHidden == "" ? strReadData : strHidden)
                                 select v).FirstOrDefault();

                    Thread.Sleep(300);
                    if (vPass == null)
                    {
                        SendNGSignal();
                        t.Status = "Unknown";
                    }
                    else if (Convert.ToInt32(vPass.ExpiryDate.ToString("yyyyMMdd")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        SendNGSignal();
                        t.Status = "Expired";
                    }
                    else
                    {
                        SendOKSignal();
                        t.Status = "Valid";
                    }
                }
                else
                {
                    t.Status = "File NG";
                }
                                
                readDataList.Add(t);
            });

            BarData = strReadData;

            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
        }

        private void SendNGSignal()
        {
            sr.Write("ID5" + Environment.NewLine);
            Thread.Sleep(200);
            sr.Write("IT1" + Environment.NewLine);
            //Thread.Sleep(200);
            //sr.Write("IT0" + Environment.NewLine);
            //Thread.Sleep(200);
            //sr.Write("IT0" + Environment.NewLine);
        }

        private void SendOKSignal()
        {
            // This line not working.
            //sr.Write("IT0,ID0" + Environment.NewLine);


            // This will work.
            sr.Write("ID3" + Environment.NewLine);
            Thread.Sleep(120);
            sr.Write("IT0" + Environment.NewLine);
            Thread.Sleep(120);
            sr.Write("IT0" + Environment.NewLine);
            Thread.Sleep(120);
            sr.Write("IT0" + Environment.NewLine);
        }


        private string comPort;
        public string ComPort
        {
            get { return comPort; }
            set { Set(ref comPort, value); }
        }


        private void ResetForm()
        {
            BarData = "";
            ShowHidden = false;
            //if (sr != null)
            //{
            //    sr.DiscardInBuffer();
            //    sr.DiscardOutBuffer();
            //    sr.Close();
            //}

            ReadDataList = null;
            ReadDataList = new ObservableCollection<Tag>();

            //EnableControl = false;
        }


        private ObservableCollection<Tag> readDataList;
        public ObservableCollection<Tag> ReadDataList
        {
            get { return readDataList; }
            set { Set(ref readDataList, value); }
        }


        public override void Cleanup()
        {
            // Clean up if needed

            base.Cleanup();

            if (sr != null)
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
        }


        private string barData;
        public string BarData
        {
            get { return barData; }
            set
            {
                Set(ref barData, value);
                if (!string.IsNullOrEmpty(value))
                {
                    dt.IsEnabled = true;
                }
            }
        }

    }
}