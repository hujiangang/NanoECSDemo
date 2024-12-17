using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Gameplay
{
    public class WorldSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector createRequests;

        readonly GameCollector positionRequests;

        readonly GameCollector destroyedObjects;

        public WorldSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            createRequests = contexts.Game.GetGroup().With.WorldObjectCreateRequest.OnAdd;
            positionRequests = contexts.Game.GetGroup().With.Position.With.WorldObjectNewPositionRequest.OnAdd;
            destroyedObjects = contexts.Game.GetGroup().With.WorldObject.With.Position.With.WorldObjectDestroyedEvent.OnAdd;
        }

        public void Execute()
        {
            foreach (var e in createRequests)
            {
                var newObject = e.WorldObjectCreateRequest.Transform;
                var entity = e;

                var position = newObject.position.ToVector2Int();
                contexts.worldService.worldField[position.x][position.y].Add(entity);
                entity.SafelyAddPosition(position);
                entity.SafelyAddWorldObject(newObject);
            }

            foreach (var e in positionRequests)
            {
                var positionComponent = e.Position;
                var oldPosition = positionComponent.Position;
                var newPosition = e.WorldObjectNewPositionRequest.NewPosition;
                var entity = e;

                contexts.worldService.worldField[oldPosition.x][oldPosition.y].Remove(entity);
                contexts.worldService.worldField[newPosition.x][newPosition.y].Add(entity);

                positionComponent.Position = newPosition;
            }

            foreach (var e in destroyedObjects)
            {
                var objectToDestroy = e.WorldObject.Transform;
                var position = e.Position.Position;
                var deleteEntity = e.WorldObjectDestroyedEvent.DeleteEntity;
                var entity = e;

                contexts.worldService.worldField[position.x][position.y].Remove(entity);
                objectToDestroy.gameObject.SetActive(false);

                if (deleteEntity)
                {
                    contexts.Game.Destroy(entity);
                }
                else
                {
                    entity.RemoveWorldObject();
                    entity.RemovePosition();
                }
            }
        }
    }
}
