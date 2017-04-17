using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TESTUDO2.Server.Game
{
    internal class MatchMother
    {
		public void Run()
		{
			while(true)
			{

			}
		}

		public static class Factory
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
