using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgMaker.Game.Core
{
	public class TileMap
	{
		private Dictionary<HexCoord, Tile> _tiles;
		public TileIndexer Tiles { get; private set; }

		public class TileIndexer
		{
			private TileMap _map;

			private Tile _emptyTile = new Tile { Type = TileType.kNone };

			public TileIndexer(TileMap map)
			{
				_map = map;
			}

			public Tile this[HexCoord coord]
			{
				get
				{
					var result = _emptyTile;
					_map._tiles.TryGetValue(coord, out _emptyTile);
					return result;
				}
			}
		}

		internal static class Factory
		{
			public static TileMap Create(TileMapSource source)
			{
				// TODO(sorae): impl..
				return null;
			}
		}
	}
}