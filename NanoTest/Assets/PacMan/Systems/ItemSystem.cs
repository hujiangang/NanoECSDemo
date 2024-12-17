using System;
using NanoEcs;

namespace PacMan.Gameplay
{
    public class ItemSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector players;

        public ItemSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            players = contexts.Game.GetGroup()
                .With.Player
                .With.WorldObjectNewPositionRequest
                .OnAdd;
        }

        public void Execute()
        {
            foreach (var p in players)
            {
                var newPosition = p.WorldObjectNewPositionRequest.NewPosition;
                var playerEntity = p;

                foreach (var entity in contexts.worldService.GetEntitiesOn(newPosition))
                {
                    if (!entity.HasItemMarker) continue;

                    entity.SafelyAddItemTakenEvent(playerEntity);
                    entity.AddWorldObjectDestroyedEvent(true);
                }
            }
        }
    }
}
