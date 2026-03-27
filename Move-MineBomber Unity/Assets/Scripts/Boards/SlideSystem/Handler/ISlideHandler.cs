using Bomb.Datas;
using System.Collections.Generic;

namespace Bomb.Boards.Slides
{
    public interface ISlideHandler
    {
        List<SlideResult> Slide(BoardManager board);
    }
}
