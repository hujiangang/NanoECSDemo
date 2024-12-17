using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class PortalSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector portals;

        readonly GameCollector moveObjects;

        public PortalSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            portals = contexts.Game.GetGroup().With.Portal.OnAdd;

            moveObjects = contexts.Game.GetGroup().With.WorldObjectNewPositionRequest.With.Movement.OnAdd;
        }

        public void Execute()
        {
            foreach (var e in moveObjects)
            {
                var newPosition = e.WorldObjectNewPositionRequest.NewPosition;
                var movableEntity = e;
                CheckPortalInPosition(movableEntity, newPosition);
            }

            foreach (var e in portals)
            {
                var portalComponent = e.Portal;
                if (portalComponent.EstimateReloadTime > 0)
                {
                    portalComponent.EstimateReloadTime -= Time.deltaTime;
                }
            }
        }

        private void CheckPortalInPosition(in GameEntity movableEntity, in Vector2Int newPosition)
        {
            foreach (var entity in contexts.worldService.GetEntitiesOn(newPosition))
            {
                if (!entity.HasPortal) continue;

                var portal = entity.Portal;
                if (portal.EstimateReloadTime > 0) continue;

                var otherPortalEntity = portal.OtherPortalEntity;
                var otherPortalPosition = otherPortalEntity.Position.Position;
                movableEntity.SafelyAddTeleportToPositionRequest(otherPortalPosition);

                portal.EstimateReloadTime = contexts.GameDefinitions.portalDefinition.portalReloadTime;
                otherPortalEntity.Portal.EstimateReloadTime = contexts.GameDefinitions.portalDefinition.portalReloadTime;
            }
        }

    }
}
