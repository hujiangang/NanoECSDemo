using NanoEcs;
using PacMan.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.PacMan.Systems
{
    public class PlayerDeathSystem : SystemEcs, IExecutable
    {
        private readonly PacManContexts contexts;

        readonly GameCollector requests;

        public PlayerDeathSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            requests = contexts.Game.GetGroup().With.Player
                .With.PlayerDeathRequest.OnAdd;
        }

        public void Execute()
        {
            foreach (var e in requests)
            {
                var playerEntity = e;
                var deadPlayer = e.Player;

                var spawnPosition = deadPlayer.SpawnPosition;
                if (--deadPlayer.Lives <= 0)
                {
                    spawnPosition = Vector2Int.zero;

                    playerEntity.SafelyAddDeadPlayerMarker(0);
                    playerEntity.SafelyAddWorldObjectDestroyedEvent(false);
                }

                playerEntity.SafelyAddTeleportToPositionRequest(spawnPosition);
                playerEntity.SafelyAddPlayerScoreChangedEvent(0);
            }
        }
    }
}
