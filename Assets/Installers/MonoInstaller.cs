using UnityEngine;
using Zenject;

public class MonoInstaller : MonoInstaller<MonoInstaller>
{
    [Inject]
    Config _config;
    public override void InstallBindings()
    {
        //Interfaces
        Container.Bind<EnemySpawner>().AsSingle().NonLazy();

        //Classes
        Container.Bind<CollisionResolver>().AsSingle().NonLazy();

        //Factories

        //Pools
        Container.BindMemoryPool<Enemy, Enemy.Pool>().
            WithInitialSize(3).
            FromNewComponentOnNewPrefab(_config.Enemy).
            UnderTransformGroup("Enemies");

        //Signals
        Container.DeclareSignal<SignalCollision>();
    }
}