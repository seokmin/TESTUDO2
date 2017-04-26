﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TESTUDO2.Server.TCPService;

namespace TESTUDO2.Server.Game
{
    internal class MatchMother
    {
		internal bool IsRunning { get; private set; } = false;

		private PacketEater packetEater { get; set; } = null;

		internal void Run()
		{
			if (this.IsRunning)
				return;
			this.IsRunning = true;

			while(this.IsRunning)
			{
				packetEater?.GetPacketAsync();
			}
		}

		internal void Stop()
		{
		}

		private void update()
		{
			// NOTE(sorae): wait for a packet
			
		}

		internal static class Factory
		{
			public static MatchMother Create()
			{
				var instance = new MatchMother();

				// TODO(sorae): impl..

				return instance;
			}
		}
	}
}
