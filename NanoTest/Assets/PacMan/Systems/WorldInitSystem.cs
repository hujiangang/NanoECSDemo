using NanoEcs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class WorldInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        public WorldInitSystem(PacManContexts contexts)
        {
            this.contexts = contexts;
        }

        public void Initialize()
        {
            if (!contexts.GameDefinitions.worldDefinition) throw new Exception($"{nameof(WorldDefinition)} doesn't exists!");

            var worldDefinition = contexts.GameDefinitions.worldDefinition;
            contexts.worldService.worldField = new HashSet<GameEntity>[worldDefinition.sizeX][];
            for (int xIndex = 0, xMax = worldDefinition.sizeX; xIndex < xMax; xIndex++)
            {
                var yFields = new HashSet<GameEntity>[worldDefinition.sizeY];
                for (int yIndex = 0, yMax = worldDefinition.sizeY; yIndex < yMax; yIndex++)
                {
                    yFields[yIndex] = new HashSet<GameEntity>();
                }

                contexts.worldService.worldField[xIndex] = yFields;
            }

            var finalX = Vector3.right * worldDefinition.sizeX;
            var finalY = Vector3.forward * worldDefinition.sizeY;
            var final = finalX + finalY;

            Debug.DrawLine(Vector3.zero, finalX, UnityEngine.Color.yellow, 5000);
            Debug.DrawLine(Vector3.zero, finalY, UnityEngine.Color.yellow, 5000);
            Debug.DrawLine(finalX, final, UnityEngine.Color.yellow, 5000);
            Debug.DrawLine(finalY, final, UnityEngine.Color.yellow, 5000);
        }
    }
}
