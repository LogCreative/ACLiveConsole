//// ���ļ�ժ�� bilibili-dm, CopyLiu, WTFPL    
//// �������˼��޸ĺ� bug �޸ġ�
//// Ver: 1.1.1.20, Feb 2020.
//// �ð汾���á�

//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;      // ��Ҫ .NET Framework 4.5
//using System.Net.Sockets;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Threading;
//using System.Xml;
//using BilibiliDM_PluginFramework;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace BiliDMLib_old
//{
//    public delegate void DisconnectEvt(object sender, DisconnectEvtArgs e);

//    public delegate void ReceivedDanmakuEvt(object sender, ReceivedDanmakuArgs e);

//    public delegate void ReceivedRoomCountEvt(object sender, ReceivedRoomCountArgs e);
//    public delegate void ConnectedEvt(object sender, ConnectedEvtArgs e);

//    public class ReceivedRoomCountArgs
//    {
//        public uint UserCount;
//    }

//    public class DisconnectEvtArgs
//    {
//        public Exception Error;
//    }

//    public class ReceivedDanmakuArgs
//    {
//        public DanmakuModel Danmaku;
//    }
//    public class ConnectedEvtArgs
//    {
//        public int roomid;
//    }

//    public class DanmakuLoader
//    {
//        private string[] defaulthosts = new string[] {"livecmt-2.bilibili.com", "livecmt-1.bilibili.com"};
//        private string ChatHost = "chat.bilibili.com";
//        private int ChatPort = 2243; // TCPЭ��Ĭ�϶˿������޸ĵ� 2243
//        private TcpClient Client;
//        private NetworkStream NetStream;
//        private string CIDInfoUrl = "https://api.live.bilibili.com/room/v1/Danmu/getConf?room_id=";
//        private bool Connected = false;
//        public Exception Error;
//        public event ReceivedDanmakuEvt ReceivedDanmaku;
//        public event DisconnectEvt Disconnected;
//        public event ReceivedRoomCountEvt ReceivedRoomCount;
//        public event LogMessageEvt LogMessage;
//        private bool debuglog = true;
//        private short protocolversion = 1;
//        private static int lastroomid ;
//        private static string lastserver;
//        private static HttpClient httpClient=new HttpClient(){Timeout = TimeSpan.FromSeconds(5)};
//        private object shit_lock=new object();//ReceiveMessageLoop �ƺ������ŕ�ͬ�r�\�Ѓɂ���bug, ���ǲ�����, �i������


//        // ͬһ������ ͬһ������
//        //Thread thread;
//        //bool setted = false;
//        //bool receivestart;

//        //byte[] stableBuffer;

//        //DispatcherTimer dispatcherReiceve = new DispatcherTimer();

//        public DanmakuLoader()
//        {
//            //thread = new Thread(this.ReceiveMessageLoop);
//            //thread.IsBackground = true;
//            //receivestart = false;
//            //thread.Start();

//            //�ü�ʱ����
//            //Client = new TcpClient();
//            //stableBuffer = new byte[Client.ReceiveBufferSize];
//            //dispatcherReiceve.Tick += new EventHandler(this.ReceiveMessageLoop);
//            //dispatcherReiceve.Interval = new TimeSpan(0, 0, 0, 0, 100);

//        }

//        public async Task<bool> ConnectAsync(int roomId)
//        {
//            try
//            {
//                if (this.Connected) throw new InvalidOperationException();
//                int channelId = roomId;
////
////                var request = WebRequest.Create(RoomInfoUrl + roomId + ".json");
////                var response = request.GetResponse();
////
////                int channelId;
////                using (var stream = response.GetResponseStream())
////                using (var sr = new StreamReader(stream))
////                {
////                    var json = await sr.ReadToEndAsync();
////                    Debug.WriteLine(json);
////                    dynamic jo = JObject.Parse(json);
////                    channelId = (int) jo.list[0].cid;
////                }

//                if (channelId != lastroomid)
//                {
//                    try
//                    {
//                        var req = await httpClient.GetStringAsync(CIDInfoUrl + channelId);
//                        var roomobj = JObject.Parse(req);

//                        ChatHost = roomobj["data"]["host"]+"";

//                        ChatPort = roomobj["data"]["port"].Value<int>();
//                        if (string.IsNullOrEmpty(ChatHost))
//                        {
//                            throw new Exception();
//                        }
                  
//                    }
//                    catch (WebException ex)
//                    {
//                        ChatHost = defaulthosts[new Random().Next(defaulthosts.Length)];

//                        HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
//                        if (errorResponse.StatusCode == HttpStatusCode.NotFound)
//                        {
//                            // ֱ���䲻���ڣ�HTTP 404��
//                            string msg = "��ֱ�������Ʋ����ڣ���Ļ��ֻ֧��ʹ��ԭ���������";
//                            LogMessage?.Invoke(this, new LogMessageArgs() {message = msg});
//                        }
//                        else
//                        {
//                            // Bվ��������Ӧ����
//                            string msg = "Bվ��������Ӧ��Ļ��������ַ����������ʹ�ó�����ַ����";
//                            LogMessage?.Invoke(this, new LogMessageArgs() {message = msg});
//                        }
//                    }
//                    catch (Exception)
//                    {
//                        // ��������XML�������󣿣�
//                        ChatHost = defaulthosts[new Random().Next(defaulthosts.Length)];
//                        string msg = "��ȡ��Ļ��������ַʱ����δ֪���󣬳���ʹ�ó�����ַ����";
//                        LogMessage?.Invoke(this, new LogMessageArgs() {message = msg});
//                    }


//                }
//                else
//                {
//                    ChatHost = lastserver;
//                }

//                Client = new TcpClient();

//                //await  Client.ConnectAsync(ChatHost, ChatPort);
//                //���ؾ����Bվ��Ļ������ 2020-4-4 ��ʱ������ 18:03 GG
//                var ipaddrss = await System.Net.Dns.GetHostAddressesAsync(ChatHost);
//                var random = new Random();
//                var idx = random.Next(ipaddrss.Length);
//                await Client.ConnectAsync(ipaddrss[idx], ChatPort);

//                NetStream = Client.GetStream();


//                if (SendJoinChannel(channelId))
//                {
//                    Connected = true;
//                    this.HeartbeatLoop();

//                    //receivestart = true;

//                    //dispatcherReiceve.Start();
//                    var thread = new Thread(this.ReceiveMessageLoop);
//                    thread.IsBackground = true;
//                    thread.Start();

//                    lastserver = ChatHost;
//                    lastroomid = roomId;
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception ex)
//            {
//                this.Error = ex;
//                return false;
//            }
//        }



//        private void ReceiveMessageLoop()
//        {
//            lock (shit_lock)
//            //ReceiveMessageLoop �ƺ������ŕ�ͬ�r�\�Ѓɂ���bug, ���ǲ�����, �i������
//            {
//                try
//                {
//                var stableBuffer = new byte[Client.ReceiveBufferSize];

//                    while (this.Connected)
//                    {
                
//                        NetStream.ReadB(stableBuffer, 0, 4);
//                        var packetlength = BitConverter.ToInt32(stableBuffer, 0);
//                        packetlength = IPAddress.NetworkToHostOrder(packetlength);

//                        if (packetlength < 16)
//                        {
//                            throw new NotSupportedException("Э��ʧ��: (L:" + packetlength + ")");
//                        }

//                        NetStream.ReadB(stableBuffer, 0, 2);//magic
//                        NetStream.ReadB(stableBuffer, 0, 2);//protocol_version 

//                        NetStream.ReadB(stableBuffer, 0, 4);
//                        var typeId = BitConverter.ToInt32(stableBuffer, 0);
//                        typeId = IPAddress.NetworkToHostOrder(typeId);

//                        Console.WriteLine(typeId);
//                        NetStream.ReadB(stableBuffer, 0, 4);//magic, params?
//                        var playloadlength = packetlength - 16;
//                        if (playloadlength == 0)
//                        {
//                            continue;//û��������
//                            //return ;
//                        }
//                        typeId = typeId - 1;//�ͷ�����Ĵ����Ӧ 
//                        var buffer = new byte[playloadlength];
//                        NetStream.ReadB(buffer, 0, playloadlength);
//                        switch (typeId)
//                        {
//                            case 0:
//                            case 1:
//                            case 2:
//                                {
                                    

//                                    var viewer = BitConverter.ToUInt32(buffer.Take(4).Reverse().ToArray(), 0); //��������
//                                    Console.WriteLine(viewer);
//                                    if (ReceivedRoomCount != null)
//                                    {
//                                        ReceivedRoomCount(this, new ReceivedRoomCountArgs() { UserCount = viewer });
//                                    }
//                                    break;
//                                }
//                            case 3:
//                            case 4://playerCommand
//                                {
                                   
//                                    var json = Encoding.UTF8.GetString(buffer, 0, playloadlength);
//                                    if (debuglog)
//                                    {
//                                        Console.WriteLine(json);

//                                    }
//                                    try
//                                    {
//                                        DanmakuModel dama = new DanmakuModel(json, 2);
//                                        if (ReceivedDanmaku != null)
//                                        {
//                                            ReceivedDanmaku(this, new ReceivedDanmakuArgs() { Danmaku = dama });
//                                        }

//                                    }
//                                    catch (Exception)
//                                    {
//                                        // ignored
//                                    }

//                                    break;
//                                }
//                            case 5://newScrollMessage
//                                {
                                    
//                                    break;
//                                }
//                            case 7:
//                                {
                                   
//                                    break;
//                                }
//                            case 16:
//                                {
//                                    break;
//                                }
//                            default:
//                                {
                                   
//                                    break;
//                                }
//                                //                     
//                        }
//                    }
                
//                }
//                catch (NotSupportedException ex)
//                {
//                    this.Error = ex;
//                    _disconnect();
//                }
//                catch (Exception ex)
//                {
//                    this.Error = ex;
//                    _disconnect();

//                }
//                //Disconnect();   //�ֶ�
//                return ;
//            }

//        }

//        private async void HeartbeatLoop()
//        {

//            try
//            {
//                while (this.Connected)
//                {
//                    this.SendHeartbeatAsync();
//                    await Task.Delay(30000);
//                }
//            }
//            catch (Exception ex)
//            {
//                this.Error = ex;
//                _disconnect();

//            }
//        }

//#pragma warning disable CS0618
//        public void Disconnect()
//        {

//            Connected = false;
//            try
//            {
//                Client.Close();
//            }
//            catch (Exception)
//            {

//            }

//            //�߳���ֹ
//            //thread.Suspend();
//            //thread.Abort();
//            //receivestart = false;
//            //dispatcherReiceve.Stop();

//            NetStream = null;
//        }

//        private void _disconnect()
//        {
//            if (Connected)
//            {
//                Debug.WriteLine("Disconnected");

//                Connected = false;

//                Client.Close();

//                //�߳���ֹ
//                //thread.Abort();
//                //receivestart = false;
//                //dispatcherReiceve.Stop();

//                NetStream = null;
//                if (Disconnected != null)
//                {
//                    Disconnected(this, new DisconnectEvtArgs() {Error = Error});
//                }
//            }

//        }

//        private void SendHeartbeatAsync()
//        {
//            SendSocketData(2);
//            Debug.WriteLine("Message Sent: Heartbeat");
//        }

//        void SendSocketData(int action, string body = "")
//        {
//            SendSocketData(0, 16, protocolversion, action, 1, body);
//        }
//        void SendSocketData(int packetlength, short magic, short ver, int action, int param = 1, string body = "")
//        {
//            var playload = Encoding.UTF8.GetBytes(body);
//            if (packetlength == 0)
//            {
//                packetlength = playload.Length + 16;
//            }
//            var buffer = new byte[packetlength];
//            using (var ms = new MemoryStream(buffer))
//            {


//                var b = BitConverter.GetBytes(buffer.Length).ToBE();

//                ms.Write(b, 0, 4);
//                b = BitConverter.GetBytes(magic).ToBE();
//                ms.Write(b, 0, 2);
//                b = BitConverter.GetBytes(ver).ToBE();
//                ms.Write(b, 0, 2);
//                b = BitConverter.GetBytes(action).ToBE();
//                ms.Write(b, 0, 4);
//                b = BitConverter.GetBytes(param).ToBE();
//                ms.Write(b, 0, 4);
//                if (playload.Length > 0)
//                {
//                    ms.Write(playload, 0, playload.Length);
//                }
//                NetStream.WriteAsync(buffer, 0, buffer.Length);
//                NetStream.FlushAsync();
//            }
//        }

//        private bool SendJoinChannel(int channelId)
//        {
            
//            Random r=new Random();
//            var tmpuid = (long)(1e14 + 2e14*r.NextDouble());
//            var packetModel = new {roomid = channelId, uid = tmpuid};
//            var playload = JsonConvert.SerializeObject(packetModel);
//            SendSocketData(7, playload);
//            return true;
//        }


//    }

//    public delegate void LogMessageEvt(object sender, LogMessageArgs e);
//    public class LogMessageArgs
//    {
//        public string message = string.Empty;
//    }                         
//}