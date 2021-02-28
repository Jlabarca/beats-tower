using Jlabarca.BeatsTower.Core.MonoBehaviours;
using Leopotam.Ecs;

namespace Jlabarca.BeatsTower.Core.Pooling
{
    public class TilesPool : GenericObjectPool<TileView>
    {
        public override void ReturnToPool(TileView objectToReturn)
        {
            var entity = objectToReturn.Entity;
            entity.Del<Tempo.Components.Tile>();
            base.ReturnToPool(objectToReturn);
        }
    }
}
