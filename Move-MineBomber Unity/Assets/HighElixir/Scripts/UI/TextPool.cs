using TMPro;
using UnityEngine;

namespace HighElixir.Pool
{
    public class TextPool : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private TMP_Text _prefab;
        [SerializeField] private int _size;
        private Pool<TMP_Text> _textPool;
        public Pool<TMP_Text> Pool
        {
            get
            {
                if (_textPool == null) Create();
                return _textPool;

            }
        }

        private void Awake()
        {
            Create();
        }

        private void Create()
        {
            _textPool = new Pool<TMP_Text>(_prefab, _size, transform); ;
        }
    }
}