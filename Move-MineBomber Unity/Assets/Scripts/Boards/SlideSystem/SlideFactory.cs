using HighElixir;
using HighElixir.Collections;
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
                dir.x = RandomExtensions.GetDir();
                dir.y = RandomExtensions.GetDir();
            }
            else
            {
                if (RandomExtensions.Chance(0.5))
                    dir.x = RandomExtensions.GetDir();
                else
                    dir.y = RandomExtensions.GetDir();
            }
            if (distance == int.MaxValue)
                distance = RandomExtensions.Rand(-5, 5);
            return GetStraight(dir, distance, infos);
        }
    }
}