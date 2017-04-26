using System;
using System.Collections.Generic;

namespace TESTUDO2.GameCore
{
    public abstract class GameCore
    {
		private Queue<GameCommand> CommandQueue { get; set; } = new Queue<GameCommand>();

		private void foo()
		{
			return;
		}
    }
}
