using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgMaker.Game.Core
{
	internal class TileMapSource
	{
		public class Node
		{
			public TileType _type = TileType.kNone;
			public HexCoord _coord = HexCoord.Zero;
		}
		public List<Node> Nodes { get; set; }

		public static class Factory
		{
			public static TileMapSource Create(string filePath)
			{
				// TODO(sorae): impl..
				return null;
			}
		}
	}
}