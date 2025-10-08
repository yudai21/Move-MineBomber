using UnityEngine;

namespace HighElixir.Pool
{
    public class PoolObj : MonoBehaviour 
    {
        [Header("PoolSettings")]
        [SerializeField] private GameObject _pref;
        [SerializeField] private int _capacity;
        [SerializeField] private bool _lazy = true;

        private Pool<GameObject> _pool;

        public Pool<GameObject> Pool => _pool;

        private void Awake()
        {
            _pool = new(_pref, _capacity, transform, _lazy);
        }

        private void OnDestroy()
        {
            _pool.Dispose();
        }

        public static explicit operator Pool<GameObject>(PoolObj obj) => obj.Pool;
    }
}