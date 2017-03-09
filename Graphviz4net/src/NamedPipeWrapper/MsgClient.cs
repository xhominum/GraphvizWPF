using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeWrapper
{
    public class MsgClient
    {
        readonly NamedPipeClient<MessageContainer> _client;

        readonly string _pipeName;

        public MsgClient(string pipeName)
        {
            _pipeName = pipeName;
            //			Process.Start(@"..\..\..\DigitalIOControl\bin\Debug\DigitalIOControl.exe");
            //ProcessStartInfo processStartInfo = new ProcessStartInfo(@"DigitalIOControl.exe");

            //Process process = new Process();
            //process.StartInfo = processStartInfo;
            //if (process.Start())
            //{
                _client = TryOpenPipe();
            //}
        }

        private NamedPipeClient<MessageContainer> TryOpenPipe()
        {
            NamedPipeClient<MessageContainer> client = null;
            try
            {
                client = new NamedPipeClient<MessageContainer>(_pipeName);
                client.ServerMessage += OnServerMessage;
                client.Error += OnError;
                client.Start();
            }
            catch
            {
                //TODO
            }
            return client;
        }

        public void SendNewVertex(string VertexName, string ParentName)
        {
            PushMessage(new MsgNewVertex { Name = VertexName, Parent = ParentName });
        }

        public void SendNewEdge(string source, string destination)
        {
            PushMessage(new MsgNewEdge { Source = source, Destination = destination });
        }

        public void SendPing()
        {
            PushMessage(new MsgPing());
        }

        public void SendExit()
        {
            PushMessage(new MsgExit());
        }

        private void PushMessage(MessageType messageType)
        {
            if (_client != null)
                _client.PushMessage(new MessageContainer(messageType));
        }

        private void OnServerMessage(NamedPipeConnection<MessageContainer, MessageContainer> connection, MessageContainer message)
        {
            Console.WriteLine("Server says: {0}", message);
        }

        private void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }
    }
}
