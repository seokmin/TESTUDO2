using System.Collections.Concurrent;
using System.Threading;

namespace TESTUDO2.Server.TCPService
{
    public class PacketFeeder
    {
		// null sessionId means all sessions
		public void SubscribePacket<T>(uint? sessionId, ConcurrentQueue<Packet> receivePacketQueue, AutoResetEvent eventForNotice) where T : Packet
		{

		}
	}
}