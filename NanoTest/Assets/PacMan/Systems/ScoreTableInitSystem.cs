using NanoEcs;
using PacMan.UI;
using UnityEngine.UI;

namespace PacMan.Gameplay
{
    public class ScoreTableInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        public ScoreTableInitSystem(PacManContexts contexts)
        {
            this.contexts = contexts;
        }

        public void Initialize()
        {
            var scoreTables = UnityEngine.Object.FindObjectsOfType<ScoreTableBehaviour>();
            foreach (var behaviour in scoreTables)
            {
                contexts.Game.CreateEntity()
                    .AddScoreTable(behaviour.GetComponent<Text>());
            }
        }
    }
}
