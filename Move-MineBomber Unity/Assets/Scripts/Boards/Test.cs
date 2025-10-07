using Bomb.Boards;
using Bomb.Datas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] GameRule rule;

    private List<MassInfo> mass;

    public void Awake()
    {
        BoardBuilder.Create(out var board, rule);
        mass = board.GetAllMasses();
        Debug.Log($"Total Bombs: {mass.Where(m => (m.type & MassType.Bomb) != 0).Count()}");
        Debug.Log(board.CurrentBoardSize.ToString());
    }

    private void OnDrawGizmos()
    {
        if (mass == null) return;
        foreach (var b in mass)
        {
            Color col;
            switch (b.type)
            {
                case MassType.Bomb:
                    col = Color.red;
                    break;
                case MassType.Empty:
                    col = Color.blue;
                    break;
                default:
                    col = Color.green;
                    break;
            }
            Gizmos.color = col;
            Gizmos.DrawSphere(new Vector3(b.x, b.y, 0), 0.2f);
        }
    }
}