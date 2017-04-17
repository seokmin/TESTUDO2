using System;
using System.Net.Sockets;
using TESTUDO2.Server.Algorithm;

namespace TESTUDO2.Server.Network
{
	public class TCPService
	{

	}
}
		/*
		private static readonly int LISTEN_PORT = 9393;
		private static readonly int MAX_CONNECTIONS = 4096;
		private static readonly int BUFFER_SIZE = 1024 * 1024;
		private static readonly int BUFFER_PREALLOC_COUNT = 2;

		private Listener listener = null;

		private Pool<SocketAsyncEventArgs> receiveArgsPool = new Pool<SocketAsyncEventArgs>(MAX_CONNECTIONS);
		private Pool<SocketAsyncEventArgs> sendArgsPool = new Pool<SocketAsyncEventArgs>(MAX_CONNECTIONS);

		private BufferManager bufferManager = new BufferManager(MAX_CONNECTIONS * BUFFER_SIZE * BUFFER_PREALLOC_COUNT, BUFFER_SIZE);

		public void Start()
		{
			this.listen();
		}



		private void initializeArgsPools()
		{
			for(int i=0; i<MAX_CONNECTIONS; ++i)
			{
				UserToken token = new UserToken();

				// receive pool
				{
					var arg = new SocketAsyncEventArgs();
					arg.Completed += new EventHandler<SocketAsyncEventArgs>(receiveCompleted);
					arg.UserToken = token;

					this.bufferManager.SetBuffer(arg);

					this.receiveArgsPool.Push(arg);
				}

				// send pool
				{
					var arg = new SocketAsyncEventArgs();
					arg.Completed += new EventHandler<SocketAsyncEventArgs>(sendCompleted);
					arg.UserToken = token;

					this.bufferManager.SetBuffer(arg);

					this.sendArgsPool.Push(arg);
				}
			}
		}

		private void receiveCompleted(object sender, SocketAsyncEventArgs result)
		{
			if(result.LastOperation == SocketAsyncOperation.Receive)
			{
				processReceive(result);
				return;
			}

			throw new ArgumentException("[NetworkService] Last operation completed on the socket was not a receive");
		}

		private void processReceive(SocketAsyncEventArgs result)
		{
			var token = result.UserToken as UserToken;
			if (result.BytesTransferred > 0 && result.SocketError == SocketError.Success)
			{
				token.OnReceive(result.Buffer, result.Offset, result.BytesTransferred);

				bool isPending = token.Socket.ReceiveAsync(result);
				if (false == isPending)
				{
					this.processReceive(result);
				}
			}
			else
			{
				Debug.LogError("[NetworkService] error {0}, transferred {1}", result.SocketError, result.BytesTransferred);
				this.closeClientSocket(token);
			}
		}

		private void closeClientSocket(UserToken token)
		{

		}

		private void sendCompleted(object sender, SocketAsyncEventArgs result)
		{

		}

		private void listen()
		{
			this.listener = new Listener();
			this.listener.OnNewClientAccepted += onNewClientAccepted;
			this.listener.Start("0.0.0.0", LISTEN_PORT);
		}

		public delegate void SessionCreatedHandler(UserToken token);
		public SessionCreatedHandler OnSessionCreated = new SessionCreatedHandler(_=> { });

		private void onNewClientAccepted(Socket clientSocket, object token)
		{
			var receiveArgs = this.receiveArgsPool.Pop();
			var sendArgs = this.sendArgsPool.Pop();

			this.OnSessionCreated?.Invoke(token as UserToken);

			startReceive(clientSocket, receiveArgs, sendArgs);
		}

		private void startReceive(Socket socket, SocketAsyncEventArgs receiveArgs, SocketAsyncEventArgs sendArgs)
		{
			var token = receiveArgs.UserToken as UserToken;
			token.SetEventArgs(receiveArgs,sendArgs);
			token.Socket = socket;

			bool isPending = socket.ReceiveAsync(receiveArgs);
			if (false == isPending)
				processReceive(receiveArgs);
		}
    }
}*/