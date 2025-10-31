using Bomb.Views;
using UnityEngine;
using Zenject;

namespace Bomb.Installers
{
    public class ResultViewInstaller : MonoInstaller
    {
        [SerializeField] private ResultTime _time;

        public override void InstallBindings()
        {
            Container.Bind<ResultTime>().FromComponentOn(_time.gameObject).AsCached();
        }
    }
}