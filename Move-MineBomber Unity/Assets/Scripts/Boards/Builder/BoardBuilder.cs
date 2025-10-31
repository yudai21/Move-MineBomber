using Bomb.Datas;
using Bomb.Repository;
using HighElixir;
using System;
using UnityEngine;
using Zenject;

namespace Bomb.Boards.Builders
{
    public class BoardBuilder : IBoardBuilder
    {
        [Inject] GameDataRepository _repo;
        // int -> 爆弾の個数
        public int Create(out BoardManager board)
        {
            board = new BoardManager();
            var size = _repo.CurrentRule.MapSize;
            board.SetBoard(new MassInfo[BoardManager.VirtualHeight, BoardManager.VirtualWidth]);
            (var x_min, var y_min) = board.GetCenter();
            var sizeDelta = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var x = x_min - sizeDelta + i;
                    var y = y_min - sizeDelta + j;
                    var mass = new MassInfo();
                    mass.x = x;
                    mass.y = y;
                    mass.type |= MassType.Empty;
                    board.SetMass(x, y, mass);
                }
            }

            var needBomb = (int)(_repo.CurrentRule.BombRate * size * size);
            var allMasses = board.GetAllMasses();

            for (int i = 0; i < needBomb; i++)
            {
                if (allMasses.Count == 0) break;
                int index = RandomExtensions.Rand(0, allMasses.Count);
                try
                {
                    var m = allMasses[index];
                    m.type &= 0;
                    m.type = (m.type & ~MassType.Empty) | MassType.Bomb;
                    board.SetMass(m.x, m.y, m);
                    allMasses.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"index :{index}, allMass:{allMasses.Count}");
                    Debug.LogError(ex);
                    break;
                }
            }
            return needBomb;
        }
    }
}