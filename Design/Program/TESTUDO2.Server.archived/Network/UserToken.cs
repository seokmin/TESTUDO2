using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TESTUDO2.Server.Network
{
	public class UserToken
	{
		public Socket Socket { get; internal set; }

		// to be implemented..
		public void SetEventArgs(SocketAsyncEventArgs receiveArgs, SocketAsyncEventArgs sendArgs)
		{
			throw new NotImplementedException();
		}

		internal void OnReceive(byte[] buffer, int offset, int bytesTransferred)
		{
			throw new NotImplementedException();
		}
	}
}
