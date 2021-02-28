using System;
using Jlabarca.BeatsTower.Core.Tempo.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.BeatsTower.Core.Tempo.Systems
{
    public class TempoHitSystem :IEcsRunSystem
    {
        private readonly EcsFilter<TempoHitEvent> _hitEventsFilter = default;
        private readonly EcsWorld _world = default;
        private readonly GameState _gameState = default;

        public void Run()
        {
            foreach (var hitEventIndex in _hitEventsFilter)
            {
                var hitEvent = _hitEventsFilter.Get1(hitEventIndex);

                // var actorEntity = _world.NewEntity();
                // actorEntity.Get<Spawn>().Position = hitEvent.Position;
                _gameState.towerHeight[(int) hitEvent.Position.x - 2 ,  Math.Abs( (int) hitEvent.Position.z+2)]++;
            }
        }

        private bool InsideSquare(Vector3 center, float width, float height, Vector3 innerPosition)
        {
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            return innerPosition.x <= center.x + halfWidth
                   && innerPosition.x >= center.x - halfWidth
                   && innerPosition.z <= center.z + halfHeight
                   && innerPosition.z >= center.z - halfHeight;
        }
    }
}
