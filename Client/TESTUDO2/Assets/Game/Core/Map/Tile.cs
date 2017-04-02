using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgMaker.Game.Core
{
	public struct HexCoord
	{
		int x;
		int y;
		int z;

		public HexCoord(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		static public readonly HexCoord Zero = new HexCoord(0, 0, 0);

	}

	public enum TileType
	{
		kNone	= 0,
		kPlain	= 1,
		kOcto	= 2,
		kForest = 3,
		kWater	= 4,

	}

	public class Tile
	{
		internal Tile(TileMapSource.Node node)
		{
			Type = node._type;
		}
		
		internal Tile() { }

		public HexCoord Coord { get; internal set; }
		public TileType Type { get; internal set; }
		public List<TileObject> TileObjects { get; internal set; }
	}
}