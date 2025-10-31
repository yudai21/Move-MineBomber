using Bomb.Inputs;
using Zenject;

namespace Bomb.Installers
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputController>().AsCached();
        }
    }
}