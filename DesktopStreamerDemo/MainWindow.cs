using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibMJPEGServer;
using LibMJPEGServer.QualityManagement;
using LibThreadedSockets;

namespace DesktopStreamerDemo
{
    public partial class MainWindow : Form
    {
        private ThreadedServer _server = new ThreadedServer(8001);
        private StaticQualityDefinition qualityDefinition = new StaticQualityDefinition(60);
        private Dictionary<string, MJPEGStreamer> _streamers = new Dictionary<string, MJPEGStreamer>();

        private bool _isRunning;

        private Image _blankImage;

        private delegate void AddStreamClientCallback(string clientName);
        private delegate void RemoveStreamClientCallback(string clientName);

        public MainWindow()
        {
            InitializeComponent();

            _blankImage = CreateBlankImage();
            SetupStreamers();

            _server.ClientDataReceived += _server_ClientDataReceived;
        }

        private Image CreateBlankImage()
        {
            Bitmap image = new Bitmap(160, 160);

            using(Graphics graphics = Graphics.FromImage(image))
            using(SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(200, 200, 200)))
            using(SolidBrush foregroundBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            using(Pen foregroundPen = new Pen(foregroundBrush))
            using(Font font = new Font("Consolas", 10))
            {
                graphics.FillRectangle(backgroundBrush, 0, 0, image.Width, image.Height);
                graphics.DrawRectangle(foregroundPen, 0, 0, image.Width - 1, image.Height - 1);

                string inactiveString = "Inactive";
                SizeF stringSize = graphics.MeasureString(inactiveString, font);

                graphics.DrawString(inactiveString, font, foregroundBrush, (image.Width - stringSize.Width) / 2, (image.Height - stringSize.Height) / 2);
            }

            return image;
        }

        private void SetupStreamers()
        {
            int screenId = 0;
            foreach(Screen screen in Screen.AllScreens)
            {
                ScreenVideoSource videoSource = new ScreenVideoSource(screen, screenId++);
                MJPEGStreamer streamer = new MJPEGStreamer(videoSource, qualityDefinition);
                streamer.StreamStarted += (sender, e) => {
                    if(_isRunning)
                        AddStreamClient(GetClientStreamDescription(e.Stream));
                };
                streamer.StreamStopped += (sender, e) => {
                    if(_isRunning)
                        RemoveStreamClient(GetClientStreamDescription(e.Stream));
                };

                string streamName = $"screen{screenId}";

                _streamers[streamName] = streamer;

                _previewImageList.Images.Add(streamName, _blankImage);
                _screensListView.Items.Add(streamName, $"Screen {screenId}", streamName);
            }
        }

        private string GetClientStreamDescription(ClientStream stream)
        {
            return $"[Screen {((ScreenVideoSource)stream.Encoder.Source).ScreenId + 1}]: {stream.TargetClient.RemoteEndPoint.Address.MapToIPv4()}";
        }

        private void AddStreamClient(string clientName)
        {
            if(InvokeRequired)
            {
                Invoke(new AddStreamClientCallback(AddStreamClient), clientName);
                return;
            }

            _clientsListBox.Items.Add(clientName);
        }

        private void RemoveStreamClient(string clientName)
        {
            if(InvokeRequired)
            {
                Invoke(new RemoveStreamClientCallback(RemoveStreamClient), clientName);
                return;
            }

            _clientsListBox.Items.Remove(clientName);
        }

        private void UpdateStateStrings()
        {
            _urlLabel.Text = _isRunning ? $"Server running: {_server.GetServerAddress()}/screenX" : "Server not running.";
            _startStopButton.Text = _isRunning ? "Stop Server" : "Start Server";
        }

        private void UpdatePreviews()
        {
            foreach(MJPEGStreamer streamer in _streamers.Values)
            {
                ScreenVideoSource videoSource = (ScreenVideoSource)streamer.Source;

                if(!videoSource.IsCapturing)
                {
                    _previewImageList.Images[videoSource.ScreenId] = _blankImage;
                    continue;
                }

                using(Image previewImage = videoSource.QueryPreviewImage())
                    if(previewImage != null)
                        _previewImageList.Images[videoSource.ScreenId] = previewImage;
            }

            _screensListView.Refresh();
        }

        private void _server_ClientDataReceived(object sender, ClientDataReceivedEventArgs e)
        {
            string requestHeader = Encoding.UTF8.GetString(e.Data);

            foreach(string line in requestHeader.Split('\n'))
            {
                if(!line.StartsWith("GET"))
                    continue;

                foreach(string part in line.Split(' '))
                {
                    if(!part.StartsWith("/"))
                        continue;

                    string streamName = part.Substring(1);

                    if(!_streamers.ContainsKey(streamName))
                        continue;

                    _streamers[streamName].AddClient(e.ClientConnection);
                    return;
                }
            }

            _server.DisconnectClient(e.ClientConnection);
        }

        private void _startStopButton_Click(object sender, EventArgs e)
        {
            _isRunning = !_isRunning;

            if(_isRunning)
                _server.Start();
            else
                _server.Stop();

            UpdateStateStrings();
            _clientsListBox.Items.Clear();
        }

        private void _previewUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdatePreviews();
        }
    }
}
