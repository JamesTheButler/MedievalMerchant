namespace Tilemap
{
    public struct TileFlags
    {
        private int _flags;

        public void AddType(TileType tile)
        {
            _flags |= 1 << (int)tile;
        }

        public void Remove(TileType tile)
        {
            _flags &= ~(1 << (int)tile);
        }

        public bool Has(TileType tile)
        {
            return (_flags & (1 << (int)tile)) != 0;
        }

        public bool Any()
        {
            return _flags != 0;
        }
    }
}