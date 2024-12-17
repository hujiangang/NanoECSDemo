using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Gameplay
{
    public class FoodSystem : SystemEcs, IExecutable
    {
        private readonly PacManContexts contexts;

        readonly GameCollector takenFoods;

        public FoodSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            takenFoods = contexts.Game.GetGroup()
                .With.Food
                .With.ItemTakenEvent
                .OnAdd;
        }

        public void Execute()
        {
            foreach (var e in takenFoods)
            {
                var playerEntity = e.ItemTakenEvent.PlayerEntity;
                if (!playerEntity.HasPlayer) continue;

                var player = playerEntity.Player;
                player.Scores += e.Food.Scores;

                var moveComponent = playerEntity.Movement;
                moveComponent.Speed -= e.Food.SpeedPenalty;

                playerEntity.SafelyAddPlayerScoreChangedEvent(0);
            }
        }
    }
}
