using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Gameplay
{
    public class ScoreTableSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector players;
        readonly GameCollector scoreTables;
        readonly GameCollector updateEvents;
        readonly GameCollector tablesToInit;

        private readonly StringBuilder stringBuilder = new StringBuilder("P10 Scores:100000 Lives:100");


        public ScoreTableSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            players = contexts.Game.GetGroup()
                .With.Player
                .OnAdd;

            scoreTables = contexts.Game.GetGroup()
                .With.ScoreTable
                .OnAdd;

            updateEvents = contexts.Game.GetGroup()
                .With.PlayerScoreChangedEvent
                .OnAdd;

            tablesToInit = contexts.Game.GetGroup()
                .With.ScoreTable.Without
                .InitializedScoreTableMarker
                .OnAdd;
        }


        public void Execute()
        {
            if (!updateEvents.IsNotEmpty && !tablesToInit.IsNotEmpty) return;

            stringBuilder.Clear();

            foreach (var e in players)
            {
                var player = e.Player;

                stringBuilder.Append("P");
                stringBuilder.Append(player.Num);

                if (e.HasDeadPlayerMarker)
                {
                    stringBuilder.Append(" IS DEAD");
                }
                else
                {
                    stringBuilder.Append(" Lives:");
                    stringBuilder.Append(player.Lives);
                }

                stringBuilder.Append(" Scores:");
                stringBuilder.Append(player.Scores);
                stringBuilder.AppendLine();
            }

            foreach (var e in scoreTables)
            {
                e.ScoreTable.ScoreText.text = stringBuilder.ToString();
                e.SafelyAddInitializedScoreTableMarker(0);
            }
        }
    }
}
