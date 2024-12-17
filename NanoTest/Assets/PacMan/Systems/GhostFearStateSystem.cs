using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace PacMan.Gameplay
{
    public class GhostFearStateSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector ghosts;

        readonly GameCollector requests;

        readonly GameCollector fearStateGhosts;

        public GhostFearStateSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            ghosts = contexts.Game.GetGroup().With.Ghost.OnAdd;
            requests = contexts.Game.GetGroup().With.GhostFearStateRequest.OnAdd;
            fearStateGhosts = contexts.Game.GetGroup().With.GhostInFearState.With.Ghost.OnAdd;
        }

        public void Execute()
        {
            var ghostDefinition = contexts.GameDefinitions.ghostDefinition;
            EnableGhostFearIfNeed(ghostDefinition);

            foreach (var e in fearStateGhosts)
            {
                var ghostEntity = e;
                var ghostComponent = e.Ghost;
                var fearState = e.GhostInFearState;

                fearState.EstimateTime -= Time.deltaTime;
                if (fearState.EstimateTime <= 0)
                {
                    RemoveFearState(ghostEntity, ghostDefinition, ghostComponent);
                    return;
                }

                var currentPosition = ghostEntity.Position.Position;
                foreach (var entity in contexts.worldService.GetEntitiesOn(currentPosition))
                {
                    if (!entity.HasPlayer) continue;

                    ghostEntity.SafelyAddWorldObjectDestroyedEvent(false);
                    entity.Player.Scores += ghostDefinition.scoresPerGhost;
                    entity.SafelyAddPlayerScoreChangedEvent(0);
                }
            }
        }

        private void EnableGhostFearIfNeed(GhostDefinition ghostDefinition)
        {
            if (!requests.IsNotEmpty) return;

            foreach (var e in ghosts)
            {
                var ghost = e.Ghost;
                var ghostEntity = e;

                ghostEntity.SafelyAddGhostInFearState(ghostDefinition.fearStateInSec);
                ghost.SetColor(ghostDefinition.fearState);
            }
        }

        private static void RemoveFearState(GameEntity ghostEntity, GhostDefinition ghostDefinition, GhostComponent ghostComponent)
        {
            ghostEntity.RemoveGhostInFearState();
            switch (ghostComponent.GhostType)
            {
                case GhostTypes.Blinky:
                    ghostComponent.SetColor(ghostDefinition.blinky);
                    break;
                case GhostTypes.Pinky:
                    ghostComponent.SetColor(ghostDefinition.pinky);
                    break;
                case GhostTypes.Inky:
                    ghostComponent.SetColor(ghostDefinition.inky);
                    break;
                case GhostTypes.Clyde:
                    ghostComponent.SetColor(ghostDefinition.clyde);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
