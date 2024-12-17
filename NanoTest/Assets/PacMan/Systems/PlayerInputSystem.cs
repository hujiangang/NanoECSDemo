using NanoEcs;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class PlayerInputSystem : SystemEcs, IExecutable
    {
        PacManContexts contexts;

        GameCollector players;

        public PlayerInputSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            players = contexts.Game.GetGroup()
                .With.Player
                .With.Movement
                .OnAdd;
        }

        public void Execute()
        {
            foreach (var p in players)
            {
                var playerNum = p.Player.Num;
                var yAxis = Input.GetAxis($"Player{playerNum.ToString()}Y");
                var xAxis = Input.GetAxis($"Player{playerNum.ToString()}X");

                var movement = p.Movement;
                if (yAxis > 0)
                {
                    movement.Heading = Directions.Up;
                }
                else if (yAxis < 0)
                {
                    movement.Heading = Directions.Down;
                }
                else if (xAxis > 0)
                {
                    movement.Heading = Directions.Right;
                }
                else if (xAxis < 0)
                {
                    movement.Heading = Directions.Left;
                }
            }
        }
    }
}
