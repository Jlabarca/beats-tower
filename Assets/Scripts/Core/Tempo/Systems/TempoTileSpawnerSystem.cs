using System;
using System.Collections.Generic;
using Jlabarca.BeatsTower.Core.MonoBehaviours;
using Jlabarca.BeatsTower.Core.Pooling;
using Jlabarca.BeatsTower.Core.Tempo.Components;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jlabarca.BeatsTower.Core.Tempo.Systems
{
    public class TempoTileSpawnerSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TempoEvent> _filter = default;
        private readonly EcsWorld _world = default;
        private readonly TilesPool _tilesPool = default;
        private readonly GameState _gameState = default;

        private static readonly int[] SpawnPositionsX = { 2,3,4 };
        private static readonly int SpawnPositionZ = 20;
        private static readonly int[] TargetPositionsZ = { -2,-3,-4 };
        private static readonly float SpawnPositionY = .1f;
        private static readonly Vector3 InitialPosition = new Vector3(0, SpawnPositionY, 0);


        public void Run()
        {
            foreach (var index in _filter)
            {
                Debug.Log(_filter.GetEntitiesCount());
                ref var tempoEvent = ref _filter.Get1(index);
                //Debug.Log($"{Time.time} - {tempoEvent.soundEvent}");
                var (x, y) = LowerHeightValue();

                var tileView = _tilesPool.Get();
                tileView.transform.position = InitialPosition;

                var targetXPosition = SpawnPositionsX[x];
                var targetZPosition = TargetPositionsZ[y];
                var targetYPosition =  SpawnPositionY + _gameState.towerHeightWarm[targetXPosition - 2, Math.Abs(targetZPosition + 2)]++;

                var spawnPosition = new Vector3(targetXPosition, SpawnPositionZ, targetZPosition);
                var targetPosition = new Vector3(targetXPosition, targetYPosition, targetZPosition);

                tileView.movingTile.localPosition = spawnPosition;
                tileView.targetTile.localPosition = targetPosition;

                var tileEntity = _world.NewEntity();
                tileEntity.Replace(new Tile
                    {
                        SpawnPosition = spawnPosition,
                        TargetPosition = targetPosition,
                        ReleaseTime = tempoEvent.ReleaseTime,
                        FinalTime = tempoEvent.MidiEvent.TimeInSeconds,
                        TileView = tileView.GetComponent<TileView>(),
                    });

                tileView.Entity = tileEntity;
                tileView.gameObject.SetActive(true);
            }
        }

        private (int, int) LowerHeightValue()
        {
            var lowerHeightCords = new List<(int, int)>();
            int lowerHeight = int.MaxValue;
            var towerHeight = _gameState.towerHeightWarm;

            for (int x = 0; x < towerHeight.GetLength(0); x ++) {
                for (int y = 0; y < towerHeight.GetLength(1); y ++)
                {
                    var height = towerHeight[x, y];
                    if (height < lowerHeight) {
                        lowerHeight = height;
                        lowerHeightCords.Clear();
                    }
                    if (height == lowerHeight)
                        lowerHeightCords.Add((x, y));
                }
            }

            var a = lowerHeightCords[Random.Range(0, lowerHeightCords.Count)];
            return a;
        }
    }
}
