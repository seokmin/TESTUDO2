using System.Collections.Concurrent;
using System.Threading;
using TESTUDO2.Server.Game.Protocols;
using TESTUDO2.Server.TCPService;

namespace TESTUDO2.Server.Game
{
	internal class MatchMother
    {
		internal bool IsRunning { get; private set; } = false;

		private PacketFeeder feeder { get; set; } = null;

		private ConcurrentQueue<Packet> receivePacketQueue { get; set; } = null;

		internal void Start()
		{
			if (this.IsRunning)
				return;
			this.IsRunning = true;

			var signalForPacketArrival = new AutoResetEvent(false);
			this.feeder.SubscribePacket<PacketJoinMatchReq>(null, this.receivePacketQueue, signalForPacketArrival);

			while (this.IsRunning)
			{
				signalForPacketArrival.WaitOne();
				processArrivedPackets();
			}
		}

		private void processArrivedPackets()
		{
			while (this.receivePacketQueue.TryDequeue(out Packet recentPacket))
			{
				processSinglePacket(recentPacket);
			}
		}

		private void processSinglePacket(Packet packet)
		{
			if(packet is PacketJoinMatchReq)
			{
			}
			else
			{
				// TODO(sorae): Log error
			}
		}

		private void acceptNewPlayer(uint sessionId)
		{
			
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