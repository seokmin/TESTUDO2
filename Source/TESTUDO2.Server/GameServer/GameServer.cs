using System.Threading;
using System.Threading.Tasks;

namespace TESTUDO2.Server.Game
{
    internal class GameServer
    {
		public bool IsRunning { get; set; } = false;
		public AutoResetEvent OnAppFinished = new AutoResetEvent(false);

		private MatchMother _matchMother = null;

		public void Run()
		{
			if(IsRunning)
			{
				// TODO(sorae): log
				return;
			}
			IsRunning = true;

			// TODO(sorae): startup log

			_matchMother = MatchMother.Factory.Create();
			Task.Factory.StartNew(_matchMother.Start);

			OnAppFinished.WaitOne();

			IsRunning = false;
		}

		public static class Factory
		{
			public static GameServer Create(GameServerConfig config)
			{
				var instance = new GameServer();

				// TODO(sorae): impl..

				return instance;
			}
		}
	}
}
