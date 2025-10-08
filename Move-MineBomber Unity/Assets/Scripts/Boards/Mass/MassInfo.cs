using System;

namespace Bomb.Boards
{
#if UNITY_EDITOR
    [Serializable]
#endif
    public struct MassInfo
    {
        public int x, y;
        public MassType type;
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