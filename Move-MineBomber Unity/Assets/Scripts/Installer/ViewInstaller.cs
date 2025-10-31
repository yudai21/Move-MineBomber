using Zenject;
using UnityEngine;
using Bomb.Views;

namespace Bomb.Installers
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private BoardViewer _boardViewer;
        [SerializeField] private TextPool _pool;
        [SerializeField] private Canvas _canvas;
        public override void InstallBindings()
        {
            Container.Bind<BoardViewer>().FromComponentOn(_boardViewer.gameObject).AsCached();
            Container.Bind<TextPool>().FromComponentOn(_pool.gameObject).AsCached();
            Container.Bind<Canvas>().FromComponentOn(_canvas.gameObject).AsSingle();
            Container.Bind<ViewObjRooter>().AsCached();
        }
    }
}