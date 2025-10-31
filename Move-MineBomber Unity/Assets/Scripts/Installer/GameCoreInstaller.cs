using Bomb.Repository;
using Zenject;

namespace Bomb.Installers
{
    public class GameCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // どこからでもアクセス可能なシングルトンなクラス
            Container.Bind<GameDataRepository>().AsSingle();
            Container.Bind<GameStateHolder>().AsSingle();
        }
    }
}
