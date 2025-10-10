using Bomb.Boards.Helpers;
using HighElixir;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Boards.Slides
{
    public static class SlideTargetSelecter
    {
        public static List<MassInfo> GetRandLine(this BoardManager board)
        {
            var bounds = board.GetActiveBounds();
            //Debug.Log($"minX:{bounds.minX}, maxX:{bounds.maxX}, minY:{bounds.minY}, maxY:{bounds.maxY}");
            var masses = new List<MassInfo>();
            if (RandomExtensions.Percent(50f))
            {
                int randX = RandomExtensions.Rand(bounds.minX, bounds.maxX);
                for (int y = bounds.minY; y <= bounds.maxY; y++)
                {
                    var mass = board.GetMass(y, randX);
                    if (mass.IsDummy) continue;
                    masses.Add(mass);
                }
            }
            else
            {
                int randY = RandomExtensions.Rand(bounds.minY, bounds.maxY);
                for (int x = bounds.minX; x <= bounds.maxX; x++)
                {
                    var mass = board.GetMass(randY, x);
                    if (mass.IsDummy) continue;
                    masses.Add(mass);
                }
            }
            return masses;
        }
    }
}