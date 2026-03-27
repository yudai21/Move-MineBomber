using Bomb.Boards;

namespace Bomb.Datas
{
    public readonly struct SlideResult
    {
        public readonly MassInfo New;
        public readonly MassInfo Old;

        public SlideResult(MassInfo @new, MassInfo old)
        {
            New = @new; Old = old;
        }
    }
}