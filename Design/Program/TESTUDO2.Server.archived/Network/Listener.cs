using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using TESTUDO2.Server;

namespace TESTUDO2.Server.Network
{
	internal class Listener
	{
		private readonly int CONST_BACKLOG_COUNT = 1024;

		private SocketAsyncEventArgs acceptArguments = null;
		private Socket listenSocket = null;
		// NOTE(sorae): Accept처리의 순서를 제어하기 위한 event
		private AutoResetEvent acceptCompletedEvent = null;

		public delegate void NewClientHandler(Socket clientSocket, object token);
		public NewClientHandler OnNewClientAccepted = null;


		public void Start(string hostIP, int port)
		{
			this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			IPAddress address;
			if (hostIP == "0.0.0.0")
			{
				address = IPAddress.Any;
			}
			else
			{
				try
				{
					address = IPAddress.Parse(hostIP);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					Debug.LogWarning("[Network.Listener] Cannot parse host IP. Trying to start with IPAdress.Any automatically..");
					address = IPAddress.Any;
				}
			}

			var endPoint = new IPEndPoint(address, port);

			try
			{
				this.listenSocket.Bind(endPoint);
				this.listenSocket.Listen(this.CONST_BACKLOG_COUNT);

				this.acceptArguments = new SocketAsyncEventArgs();
				this.acceptArguments.Completed += new System.EventHandler<SocketAsyncEventArgs>(this.onAcceptCompleted);

				Debug.Log("[Network.Listener] Start new listener thread..");
				new Thread(this.doListen).Start();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				// FIXME(sorae): exit code 0은 정상 종료로 알고 있다. 어떻게 바꿔야 할지 공부
				Environment.Exit(0);
			}
		}

		private void onAcceptCompleted(object sender, SocketAsyncEventArgs result)
		{
			if (result.SocketError == SocketError.Success)
			{
				var clientSocket = result.AcceptSocket;

				this.acceptCompletedEvent.Set();

				this.OnNewClientAccepted?.Invoke(clientSocket, result.UserToken);

				return;
			}
			else
			{
				Debug.LogWarning("[Network.Listener] accept failed! - {0}", result.SocketError);
				this.acceptCompletedEvent.Set();
			}
		}

		private bool isListenerThreadRunning = false;
		private void doListen()
		{
			if (this.isListenerThreadRunning)
			{
				Debug.LogWarning("[Network.Listener] Another listener thread is already running..");
				return;
			}
			this.isListenerThreadRunning = true;

			this.acceptCompletedEvent = new AutoResetEvent(false);

			while (true)
			{
				this.acceptArguments.AcceptSocket = null;

				bool isPending = true;
				try
				{
					// NOTE(sorae): 동기로 수행이 완료될 경우 false를 리턴한다.
					isPending = listenSocket.AcceptAsync(this.acceptArguments);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					continue;
				}

				// NOTE(sorae): 즉시 완료 되었을 경우, 이벤트핸들러가 불리지 않는다. 수동 호출하자.
				if (isPending)
				{
					this.onAcceptCompleted(null, this.acceptArguments);
				}

				this.acceptCompletedEvent.WaitOne();
			}
		}
	}
}