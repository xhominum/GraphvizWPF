using System;
using NamedPipeWrapper;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SimpleLogger;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;

namespace NamedPipeWrapper
{
    public class MsgEventArgs : EventArgs
    {
        public MessageContainer MsgContainer { get; set; }
    }

    public class MsgServer
    {
    	readonly NamedPipeServer<MessageContainer> _server;

		readonly CancellationTokenSource _cancelToken;

        public event EventHandler<MessageType> MsgReceived;


        //BlockingCollection<MessageContainer> _messageQueue = new BlockingCollection<MessageContainer>();

        BufferBlock<MessageType> _messageQueue = new BufferBlock<MessageType>();
    	
    	public bool ServerStarted { get; private set; }

        public MsgServer(string pipeName)//, CancellationTokenSource cancelToken)
        {
        	//_cancelToken = cancelToken;
        	
	        _server = new NamedPipeServer<MessageContainer>(pipeName);
	        _server.ClientConnected += OnClientConnected;
	        _server.ClientDisconnected += OnClientDisconnected;
	        _server.ClientMessage += OnClientMessage;
	        _server.Error += OnError;
	        _server.Start();   
	        ServerStarted = true;
        }
        
        public async Task WaitAndProcessEvents()
        {
        	bool keepRunning = true;
            while (keepRunning)
            {
				MessageType message = await _messageQueue.ReceiveAsync();
                EventHandler<MessageType> handler = MsgReceived;
                if (handler != null)
                {
                    handler(this, message);
                }
                await Task.Delay(200);
            }
            //TODO check if need to stop
//            _server.Stop();
        }
        
        private void ProcessOneEvent(MessageContainer message)
        {
        	if(message == null || message.Message == null)
        	{
        		Logger.Log("message null");
        	}
        	else if(message.Message is MsgExit)
        	{
        		Thread.Sleep(1000);
        		Environment.Exit(0);
        	}
        }

        private void OnClientConnected(NamedPipeConnection<MessageContainer, MessageContainer> connection)
        {
            Console.WriteLine("Client {0} is now connected!", connection.Id);
        }

        private void OnClientDisconnected(NamedPipeConnection<MessageContainer, MessageContainer> connection)
        {
            Console.WriteLine("Client {0} disconnected", connection.Id);
            //_messageQueue.Add(new MessageContainer(new MsgExit()));
            //TODO
        }

        private void OnClientMessage(NamedPipeConnection<MessageContainer, MessageContainer> connection, MessageContainer message)
        {
            Console.WriteLine("Client {0} says: {1}", connection.Id, message);
            //_messageQueue.Add(message);
            if (message == null || message.Message == null)
            {
                Logger.Log("message null");
            }
            else
            {
                _messageQueue.Post(message.Message);
                //EventHandler<MessageType> handler = MsgReceived;
                //if (handler != null)
                //{
                //    handler(this, message.Message);
                //}
            }
           
        }

        private void OnError(Exception exception)
        {
        	Logger.Log("OnError", exception);
        }
        
    }
}