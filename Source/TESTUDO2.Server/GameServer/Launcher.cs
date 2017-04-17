using System;
using System.Threading;

namespace TESTUDO2.Server.Game
{
    internal class Launcher
    {
        public static void Main(string[] args)
        {
			var config = GameServerConfig.Factory.Create(args);

			var serverInstance = GameServer.Factory.Create(config);

			serverInstance.Run();
        }
    }
}