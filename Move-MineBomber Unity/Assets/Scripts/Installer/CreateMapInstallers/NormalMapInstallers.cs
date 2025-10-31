using Bomb.Boards.Builders;
using Zenject;

namespace Bomb.Installers
{
    public class NormalMapInstallers : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardBuilder>().To<BoardBuilder>().AsSingle();
        }
    }
}