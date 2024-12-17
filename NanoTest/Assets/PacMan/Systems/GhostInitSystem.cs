using NanoEcs;
using System;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class GhostInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        public GhostInitSystem(PacManContexts contexts)
        {
            this.contexts = contexts;
        }

        public void Initialize()
        {
            if (!contexts.GameDefinitions.ghostDefinition) throw new Exception($"{nameof(GhostDefinition)} doesn't exists!");

            var ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
            foreach (var ghostObject in ghostObjects)
            {
                var renderer = ghostObject.GetComponent<Renderer>();
                var materialPropertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(materialPropertyBlock);

                var e = contexts.Game.CreateEntity()
                    .AddGhostInFearState(0.0f)
                    .AddGhost(GetGhostType(ghostObject.name), renderer, materialPropertyBlock)
                    .AddMovement(contexts.GameDefinitions.ghostDefinition.ghostSpeed, contexts.random.NextEnum<Directions>(), ghostObject.transform.position.ToVector2Int())
                    .AddWorldObjectCreateRequest(ghostObject.transform);
            }
        }

        private static GhostTypes GetGhostType(string name)
        {
            switch (name.ToLower())
            {
                case "pinky": return GhostTypes.Pinky;
                case "inky": return GhostTypes.Inky;
                case "clyde": return GhostTypes.Clyde;
                default: return GhostTypes.Blinky;
            }
        }
    }
}
