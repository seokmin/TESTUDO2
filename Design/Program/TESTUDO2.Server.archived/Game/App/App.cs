using TESTUDO2.Server.Network;

namespace TESTUDO2.Server.Game.App
{
	class App
	{
		public static void Main(string[] args)
		{
			var network = new TCPService();
			network.Start();


		}
	}
}