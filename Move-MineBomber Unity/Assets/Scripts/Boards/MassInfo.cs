namespace Bomb.Boards
{
    public struct MassInfo
    {
        public int x, y;
        public MassType type;
        public bool isOpened; // 既に開かれているかどうか
        public int aroundBombCount;

        public bool IsDummy
        {
            get
            {
                return (type == MassType.None);
            }
        }
    }
}