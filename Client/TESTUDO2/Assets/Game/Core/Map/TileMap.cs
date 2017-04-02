using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgMaker.Game.Core
{
	public class TileMap
	{
		private Dictionary<HexCoord, Tile> _tiles;
		public TileIndexer Tiles { get; private set; }

		// Hide ctor without Factory
		private TileMap() { }

		// This class prepared in order to support to access tiles with [i] operator
		public class TileIndexer : IEnumerable<Tile>
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

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _map._tiles.Values.GetEnumerator();
			}

			IEnumerator<Tile> IEnumerable<Tile>.GetEnumerator()
			{
				return _map._tiles.Values.GetEnumerator();
			}
		}

		internal static class Factory
		{
			public static TileMap Create(TileMapSource source)
			{
				var instance = new TileMap { _tiles = new Dictionary<HexCoord, Tile>() };

				foreach (var node in source.Nodes)
					instance._tiles.Add(node._coord, new Tile(node));

				return instance;
			}
		}
	}
}