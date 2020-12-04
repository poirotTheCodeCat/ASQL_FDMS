using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Data;

namespace dg_sm_jd_em_FDMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private static List<TcpClient> clientList;
        private Thread listenerThread;
        private bool isConnected;
        private List<Telemetry> liveTel;
        private List<Telemetry> searchTel;
        private String dbConStr;
        public bool realTimeOn;
        private DataTable liveTable;
        private DataTable searchTable;

        public MainWindow()
        {
            InitializeComponent();

            realTimeOn = true;

            // set the database connection string 
            // NOTE - THIS WOULD NOT NORMALLY BE STORED HERE - THIS IS FOR DEMO PURPOSES ONLY 
            dbConStr = "data source=DESKTOP-AKELUKN;database = GroundTerminal; integrated security =SSPI";

            clientList = new List<TcpClient>();
            liveTel = new List<Telemetry>();
            searchTel = new List<Telemetry>();

            // bind the database to the live telemetry list
            realTimeGrid.ItemsSource = liveTel;


            //searchGrid.ItemsSource = searchTel;

            // initialize the tcp socket listener
            // set the port and Ip address
            Int32 port = 15000;
            IPAddress localIP = IPAddress.Parse("127.0.0.1");

            try
            {
                // start listening on the server
                server = new TcpListener(localIP, port);
                server.Start();

                // start listening for a connection from a flight system
                ThreadStart clientListenTs = new ThreadStart(waitForClient);
                listenerThread = new Thread(clientListenTs);

                listenerThread.Start();
            }
            catch
            {
                throw new Exception("something went wrong when connecting to client");
            }


        }

        private void waitForClient()
        {
            isConnected = true;
            try
            {
                //while(isConnected)
                //{
                //    // wait for a client to connect
                //    TcpClient client = server.AcceptTcpClient();

                //    // prepare thread to listen for messages
                //    ParameterizedThreadStart messageThreadStart = new ParameterizedThreadStart(waitForMessage);
                //    Thread waitThread = new Thread(messageThreadStart);

                //    clientList.Add(client); // add client to the client list
                //    waitThread.Start(client);
                //}
                TcpClient client = server.AcceptTcpClient();
                waitForMessage(client);
            }
            catch
            {
                server.Stop();
            }
            server.Stop();
        }


        /*
         * Function : waitForMessage()
         * Parameters : object o
         * Description : This waits for a client to send a message and calls upon another function to send 
         *              that message to all other connected users
         * Returns : Nothing
         */
        private void waitForMessage(object o)
        {
            TcpClient client = (TcpClient)o;
            Byte[] bytes = new byte[256];       // bytes will be used to read data


            NetworkStream stream = client.GetStream();      // used to recieve message
            int i;


            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) // iterate through read stream
            {
                String recData = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                // process the message that has been recieved 
                Telemetry telRecord = TelProcess.process(bytes);

                if(telRecord != null) // check if the packet was processed correctly
                {
                    // if correctly recieved - insert into database 
                    try
                    {
                        SqlDataAccess.insertRecord(telRecord, dbConStr);

                        // if live data is on and the return is not Null then - add the new file to live_telemetry list
                        if (realTimeOn == true)
                        {
                            liveTel.Add(telRecord);

                        }
                        stream.Write(bytes);    // send confirmation response
                        stream.Flush();
                    }
                    catch(Exception e)
                    {
                        stream.Close();
                        throw new Exception($"Real time functionality not working \nProblem: {e}");
                    }
                }
                else
                {
                    break;
                }
            }
            if(realTimeOn)
            {
                this.Dispatcher.Invoke(() =>
                {
                    realTimeGrid.ItemsSource = null;
                    realTimeGrid.ItemsSource = liveTel;
                });
            }
            // clientList.Remove(client);  // remove user from the user list
            client.Close(); // shut down connection when user disconnects
        }


        private void updateLiveTable()
        {
            realTimeGrid.ItemsSource = null;
            realTimeGrid.ItemsSource = liveTel;
        }

        /*
         * Function : stopAllClients()
         * Parameters : none
         * Description : This closes all of the connections to the clients and empties the dictionaries
         * Returns : nothing
         */
        static void stopAllClients()
        {
            foreach (TcpClient tcpSend in clientList)
            {
                tcpSend.Close();
            }
            clientList.Clear();
        }


        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            // clear any error statements
            searchError.Text = "";
            
            // check if there is anything in the textbox
            if(searchTxtBox.Text.Trim() == "")
            {
                searchError.Text = "You must enter a tail ID in the search box";
            }
            else
            {
                searchTel.Clear();
                searchGrid.ItemsSource = null;
                searchGrid.Items.Clear();

                String search = searchTxtBox.Text;
                List<Telemetry> telSearch = SqlDataAccess.getRecords(search, dbConStr);
                if(telSearch != null && telSearch.Count != 0)
                {
                    foreach (Telemetry tel in telSearch)
                    {
                        searchTel.Add(tel);
                    }
                    searchGrid.ItemsSource = searchTel;
                }
                else
                {
                    searchError.Text = $"No records with tail number {search} exists";
                }
            }
        }

        /*
         * Function: Program_Closing(object sender, CancelEventArgs e)
         * Description: upon the program closing, this method is called to close all client connections 
         */
        void Program_Closing(object sender, CancelEventArgs e)
        {
            stopAllClients();
            server.Stop();
        }

        private void real_time_switch_Checked(object sender, RoutedEventArgs e)
        {
            realTimeOn = true;
        }

        private void real_time_switch_Unchecked(object sender, RoutedEventArgs e)
        {
            realTimeOn = false;
        }
    }
}
