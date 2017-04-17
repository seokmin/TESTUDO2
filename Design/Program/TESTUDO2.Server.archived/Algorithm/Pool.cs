using System;
using System.Collections.Generic;

namespace TESTUDO2.Server.Algorithm
{
	public class Pool<T>
	{
		private Stack<T> container = null;

		public Pool(int capacity)
		{
			container = new Stack<T>(capacity);
		}

		public void Push(T item)
		{
			if (item == null)
				throw new ArgumentNullException("[Algorithm.Pool] Cannot push null object to pull");
			lock (container)
			{
				container.Push(item);
			}
		}

		public T Pop()
		{
			lock (container)
			{
				return container.Pop();
			}
		}

		public int Count { get { return container.Count; } }
	}
}