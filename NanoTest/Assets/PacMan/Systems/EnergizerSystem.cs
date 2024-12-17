using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Gameplay
{
    public class EnergizerSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector takenEnergizers;

        public EnergizerSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            takenEnergizers = contexts.Game.GetGroup()
                .With.EnergizerMarker
                .With.ItemTakenEvent
                .OnAdd;
        }

        public void Execute()
        {
            if (takenEnergizers.IsNotEmpty)
            {
                contexts.Game.CreateEntity().AddGhostFearStateRequest(0);
            }
        }
    }
}
