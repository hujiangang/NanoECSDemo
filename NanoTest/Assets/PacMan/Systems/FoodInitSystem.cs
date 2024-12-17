using NanoEcs;
using System;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class FoodInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        private readonly Transform[] foodTransforms;
        private readonly Transform[] energizerTransforms;

        public FoodInitSystem(PacManContexts contexts, Transform[] foodTransforms, Transform[] energizerTransforms)
        {
            this.contexts = contexts;
            this.foodTransforms = foodTransforms;
            this.energizerTransforms = energizerTransforms;
        }

        public void Initialize()
        {
            if (!contexts.GameDefinitions.foodDefinition) throw new Exception($"{nameof(FoodDefinition)} doesn't exists!");

            var definition = contexts.GameDefinitions.foodDefinition;
            foreach (var foodObject in foodTransforms)
            {
                var entity = contexts.Game.CreateEntity()
                    .AddItemMarker(0)
                    .AddWorldObjectCreateRequest(foodObject.transform)
                    .AddFood(definition.scoresPerFood, definition.speedPenalty);
            }

            foreach (var energizer in energizerTransforms)
            {
                var entity = contexts.Game.CreateEntity()
                    .AddEnergizerMarker(0)
                    .AddItemMarker(0)
                    .AddWorldObjectCreateRequest(energizer.transform)
                    .AddFood(definition.scoresPerFood * definition.energizerMultiplier, definition.speedPenalty * definition.energizerMultiplier);
            }
        }
    }
}
