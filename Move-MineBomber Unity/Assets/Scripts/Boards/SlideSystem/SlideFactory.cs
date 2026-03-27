using HighElixir;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Boards.Slides
{
    public static class SlideFactory
    {
        private enum Slide { Straight }
        private static List<Slide> _types = EnumWrapper.GetEnumList<Slide>();
        public static ISlideHandler CreateRandom(bool enableDiagonal = false, int distance = int.MaxValue, params MassInfo[] infos)
        {
            var type = _types.RandomPick();
            switch (type)
            {
                case Slide.Straight:
                    return GetStraightRand(enableDiagonal, distance, infos);
            }
            return null;
        }

        public static ISlideHandler GetStraight(Vector2Int direction, int distance, params MassInfo[] infos)
        {
            return new StraightSlide(direction, distance, infos);
        }

        public static ISlideHandler GetStraightRand(bool enableDiagonal = false, int distance = int.MaxValue, params MassInfo[] infos)
        {
            var dir = new Vector2Int();
            if (enableDiagonal)
            {
                dir.x = RandomExtensions.NextOne();
                dir.y = RandomExtensions.NextOne();
            }
            else
            {
                if (RandomExtensions.NextBool())
                    dir.x = RandomExtensions.NextOne();
                else
                    dir.y = RandomExtensions.NextOne();
            }
            if (distance == int.MaxValue)
                distance = RandomExtensions.Rand(-5, 5);
            return GetStraight(dir, distance, infos);
        }
    }
}