using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class MovementTargetSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        private readonly GameCollector entities;


        public MovementTargetSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            entities = contexts.Game.GetGroup()
                .With.Position
                .With.Movement
                .With.MovementTarget
                .With.MovementStoppedMarker
                .OnAdd;
        }

        public void Execute()
        {
            foreach (var e in entities)
            {
                var movement = e.Movement;
                var currentPosition = e.Position.Position;
                var targetPosition = e.MovementTarget.Target;

                Directions? newHeading = null;
                Directions? alternateHeading = null;
                var sqrMagnitude = (targetPosition - currentPosition).sqrMagnitude;
                foreach (var direction in DirectionUtils.availableDirections)
                {
                    var possiblePosition = direction.GetPosition(currentPosition);
                    if (WallOnPosition(possiblePosition)) continue;

                    var possibleSqrMagnitude = (targetPosition - possiblePosition).sqrMagnitude;
                    if (possibleSqrMagnitude <= sqrMagnitude)
                    {
                        newHeading = direction;
                        sqrMagnitude = possibleSqrMagnitude;
                    }
                    else
                    {
                        alternateHeading = direction;
                    }
                }

                movement.Heading  = newHeading ?? alternateHeading ?? movement.Heading;
            }
        }

        private bool WallOnPosition(in Vector2Int position)
        {
            // to do.
            //return worldService.GetEntitiesOn(position).Any(entity => entity.IsAlive() && entity.Has<WallMarker>());
            return contexts.worldService.GetEntitiesOn(position).Any(entity => entity.HasWallMarker);
        }
    }
}
