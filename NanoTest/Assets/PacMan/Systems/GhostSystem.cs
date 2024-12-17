using NanoEcs;
using System;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class GhostSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        private readonly GameCollector players;

        private readonly GameCollector ghosts;

        public GhostSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            players = contexts.Game.GetGroup()
                .With.Player
                .With.Movement
                .With.Position
                .OnAdd;

            ghosts = contexts.Game.GetGroup()
                .With.Ghost
                .With.Position
                .With.Movement
                .OnAdd;
        }


        public void Execute()
        {
            var playerPosition = default(Vector2Int);
            var playerDirection = default(Directions);

            foreach (var p in players) 
            {
                playerPosition = p.Position.Position;
                playerDirection = p.Movement.Heading;

            }

            foreach (var g in ghosts)
            {
                var movement = g.Movement;
                var ghostEntity = g;
                var currentPosition = g.Position.Position;

                // 是否处于恐惧中的幽灵.
                if (ghostEntity.HasGhostInFearState)
                {
                    if (ghostEntity.HasMovementTarget)
                    {
                        ghostEntity.RemoveMovementTarget();
                    }
                    if (ghostEntity.HasMovementStoppedMarker)
                    {
                        movement.Heading = contexts.random.NextEnum<Directions>();
                    }
                }
                else
                {
                    var ghostComponent = g.Ghost;
                    ghostEntity.SafelyAddMovementTarget(GetGhostTarget(ghostComponent.GhostType, currentPosition, playerPosition, playerDirection));
                    if (TryGetAlivePlayerOnPosition(currentPosition, out var GameEntity))
                    {
                        GameEntity.AddPlayerDeathRequest(0);
                    }
                }
            }
        }

        private bool TryGetAlivePlayerOnPosition(in Vector2Int currentPosition, out GameEntity playerEntity)
        {
            playerEntity = null;

            foreach (var entity in contexts.worldService.GetEntitiesOn(currentPosition))
            {
                if (entity.HasPlayer && !entity.HasPlayerDeathRequest)
                {
                    playerEntity = entity;
                    return true;
                }
            }

            return false;
        }

        private Vector2Int GetGhostTarget(GhostTypes ghostType, in Vector2Int currentPosition, in Vector2Int playerPosition, Directions playerDirection)
        {
            switch (ghostType)
            {
                case GhostTypes.Blinky: return playerPosition;
                case GhostTypes.Pinky: return playerPosition + playerDirection.GetPosition() * 6;

                case GhostTypes.Inky:
                    {
                        var distanceToPlayer = currentPosition - playerPosition;
                        if (distanceToPlayer.sqrMagnitude > 8 * 8)
                        {
                            return contexts.GameDefinitions.worldDefinition.worldSize;
                        }

                        return playerPosition;
                    }

                case GhostTypes.Clyde:
                    {
                        var distanceToPlayer = currentPosition - playerPosition;
                        if (distanceToPlayer.sqrMagnitude > 8 * 8)
                        {
                            return playerPosition;
                        }

                        return -Vector2Int.one;
                    }

                default: throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null);
            }
        }
    }
}
