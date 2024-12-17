using NanoEcs;
using System;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class PlayerInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        public PlayerInitSystem(PacManContexts contexts)
        {
            this.contexts = contexts;
        }

        public void Initialize()
        {
            if (!contexts.GameDefinitions.playerDefinition) throw new Exception($"{nameof(PlayerDefinition)} doesn't exists!");

            var playerCount = 0;
            var playerDefinition = contexts.GameDefinitions.playerDefinition;
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in playerObjects)
            {
                var startPosition = player.transform.position.ToVector2Int();
                var startHeading = playerCount % 2 != 0
                    ? Directions.Right
                    : Directions.Left;

                var e = contexts.Game.CreateEntity()
                    .AddWorldObjectCreateRequest(player.transform)
                    .AddPlayer(++playerCount, 0, playerDefinition.startLives, startPosition)
                    .AddMovement(playerDefinition.startSpeed, startHeading, startPosition);
            }
        }
    }
}
