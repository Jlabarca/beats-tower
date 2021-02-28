using Jlabarca.BeatsTower.Core.Pooling;
using Jlabarca.BeatsTower.Core.Tempo.Components;
using Jlabarca.BeatsTower.Core.Tempo.Systems;
using Jlabarca.BeatsTower.ScriptableObjects;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.BeatsTower
{
    public class GameStartup : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;

        [SerializeField]
        private GameState _gameState;

        private TilesPool _tilesPool;

        private AudioSource _audioSource;

        public Configuration configuration;

        private void OnEnable()
        {
            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
            _gameState = new GameState();

            _tilesPool = gameObject.GetComponentInChildren<TilesPool>();
            _tilesPool.Prewarm(20, configuration.tileView);

            _audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_ecsWorld);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif

            _systems
                .Add(new TempoEmitSystem())
                .Add(new TempoTileSpawnerSystem())
                .Add(new TempoTilesMoveSystem())
                .Add(new TempoHitSystem())

                .Inject(_gameState)
                .Inject(configuration)
                .Inject(_tilesPool)
                .Inject(_audioSource)

                .OneFrame<TempoEvent>()
                .OneFrame<TempoHitEvent>()

                .Init();
        }

        private void Update() {
            _systems.Run();
        }

        private void OnDisable() {
            _systems.Destroy();
            _systems = null;

            _ecsWorld.Destroy();
            _ecsWorld = null;
        }
    }
}
