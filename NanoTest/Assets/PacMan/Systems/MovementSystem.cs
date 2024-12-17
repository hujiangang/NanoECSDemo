using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class MovementSystem : SystemEcs, IExecutable
    {
        private readonly PacManContexts contexts;
        private readonly GameCollector moveEntities;

        private const float epsilon = 0.1f;

        public MovementSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            moveEntities = contexts.Game.GetGroup()
                .With.Position
                .With.Movement
                .With.WorldObject
                .OnAdd;
        }

        public void Execute()
        {
            foreach (var e in moveEntities)
            {
                var movingEntity = e;
                var positionComponent = e.Position;
                var moveComponent = e.Movement;
                var transform = e.WorldObject.Transform;

                var curPosition = transform.position;
                var desiredPosition = moveComponent.DesiredPosition.ToVector3(curPosition.y);
                var estimatedVector = desiredPosition - curPosition;
                if (estimatedVector.magnitude > epsilon)
                {
                    transform.position = Vector3.Lerp(transform.position, desiredPosition,
                                                      moveComponent.Speed / estimatedVector.magnitude * Time.deltaTime);
                    continue;
                }

                var newDesiredPosition = UpdatePosition(movingEntity, ref moveComponent, transform, positionComponent.Position);
                CheckStuckToWall(movingEntity, ref moveComponent, newDesiredPosition);
            }
        }

        private static Vector2Int UpdatePosition(in GameEntity movingEntity, ref MovementComponent movement, Transform transform, in Vector2Int oldPosition)
        {
            var newPosition = movement.DesiredPosition;
            if (!oldPosition.Equals(newPosition))
            {
                movingEntity.SafelyAddWorldObjectNewPositionRequest(newPosition);
            }

            transform.rotation = movement.Heading.GetRotation();
            return movement.Heading.GetPosition(newPosition);
        }

        private void CheckStuckToWall(in GameEntity movingEntity, ref MovementComponent movement, in Vector2Int newDesiredPosition)
        {
            var stuckToWall = false;
            foreach (var entity in contexts.worldService.GetEntitiesOn(newDesiredPosition))
            {
                // to do
                //if (entity.IsAlive() && entity.HasWallMarker)
                //{
                //    stuckToWall = true;
                //}
                if (entity.HasWallMarker)
                {
                    stuckToWall = true;
                }
            }

            if (stuckToWall)
            {
                movingEntity.SafelyAddMovementStoppedMarker(0);
            }
            else
            {
                movement.DesiredPosition = newDesiredPosition;
                if (movingEntity.HasMovementStoppedMarker)
                {
                    movingEntity.RemoveMovementStoppedMarker();
                }
            }
        }
    }
}
