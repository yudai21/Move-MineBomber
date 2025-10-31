using Bomb.Managers;
using Zenject;
using UnityEngine;

namespace Bomb.Installers
{
    public class RooterInstaller : MonoInstaller
    {
        [SerializeField] private GameSceneRooter _rooter;
        public override void InstallBindings()
        {

            Container.Bind<GameSceneRooter>().FromComponentOn(_rooter.gameObject).AsSingle();
        }
    }
}