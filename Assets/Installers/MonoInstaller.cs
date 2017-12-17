using UnityEngine;
using Zenject;

public class MonoInstaller : MonoInstaller<MonoInstaller>
{
    [Inject]
    Config _config;
    public override void InstallBindings()
    {
        ////Interfaces
        //Container.Bind<ISpawner>().WithId("Asteroid").To<AsteroidSpawner>().AsSingle();
        //Container.Bind<ISpawner>().WithId("Shot").To<ShotSpawner>().AsSingle();
        //Container.Bind<EnemySpawner>().AsSingle().NonLazy();

        ////Classes
        Container.Bind<CollisionResolver>().AsSingle().NonLazy();

        ////Factories

        //Pools
        //Container.BindMemoryPool<Enemy, Enemy.Pool>().
        //    WithInitialSize(3).
        //    FromNewComponentOnNewPrefab(_config.Enemy).
        //    UnderTransformGroup("Enemies");
        //Container.BindMemoryPool<Asteroid, Asteroid.Pool>()
        //    .WithInitialSize(_config.Levels[0].AsteroidAmount)
        //    .FromNewComponentOnNewPrefab(_config.Asteroid)
        //    .UnderTransformGroup("Asteroids");
        //Container.BindMemoryPool<Shot, Shot.Pool>()
        //    .WithInitialSize(5)
        //    .FromNewComponentOnNewPrefab(_config.Shot)
        //    .UnderTransformGroup("Shots");

        //Signals
        Container.DeclareSignal<SignalCollision>();
    }
}