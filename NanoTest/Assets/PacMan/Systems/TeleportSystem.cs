using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Gameplay
{
    public class TeleportSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector requests;

        public TeleportSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            requests = contexts.Game.GetGroup().With.WorldObject.With.TeleportToPositionRequest
                .With.Movement.OnAdd;
        }

        public void Execute()
        {
            foreach (var e in requests)
            {
                var moveComponent = e.Movement;
                var transform = e.WorldObject.Transform;
                var targetPosition = e.TeleportToPositionRequest.NewPosition;

                moveComponent.DesiredPosition = targetPosition;
                transform.position = targetPosition.ToVector3(transform.position.y);

                e.SafelyAddWorldObjectNewPositionRequest(targetPosition);
            }
        }
    }
}
