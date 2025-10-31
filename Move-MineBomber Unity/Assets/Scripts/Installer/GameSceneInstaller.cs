using Bomb.Boards;
using Bomb.Managers.Timers;
using Zenject;

namespace Bomb.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITimer>().To<GameTimerModel>().AsCached();
            Container.Bind<BoardController>().AsCached();
        }
    }
}