using System;
using System.Threading.Tasks;

namespace TESTUDO2.Server.TCPService
{
	public class PacketEater
	{
		private PacketEater() { }

		private PacketFeeder packetFeeder = null;

		public async Task<Packet> GetPacketAsync()
		{
			return null;
			// TODO(sorae): impl..
		}


		public static class Factory
		{
			public static PacketEater Create(PacketFeeder targetFeeder)
			{
				if (targetFeeder == null)
				{
					throw new ArgumentNullException("PacketFeeder is essential for PacketEater");
				}

				return null;
				// TODO(sorae): impl..
			}
		}
	}
}