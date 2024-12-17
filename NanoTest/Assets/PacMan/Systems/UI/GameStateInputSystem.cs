using NanoEcs;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class GameStateInputSystem : SystemEcs, Iinitializable, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector menus;
        readonly GameCollector alivePlayers;
        readonly GameCollector gameState;

        public GameStateInputSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            menus = contexts.Game.GetGroup()
                .With.PauseMenu
                .OnAdd;

            alivePlayers = contexts.Game.GetGroup()
                .With.Player
                .Without.DeadPlayerMarker
                .OnAdd;

            gameState = contexts.Game.GetGroup()
                .With.GameStateSwitchRequest
                .OnAdd;
        }

        public void Initialize()
        {
            foreach (var e in menus)
            {
                var menu =  e.PauseMenu;

                menu.ContinueBtn.onClick.AddListener(ContinueGame);
                menu.RestartBtn.onClick.AddListener(RestartGame);
                menu.QuitBtn.onClick.AddListener(QuitGame);
            }
        }

        public void Execute()
        {
            var alivePlayersCount = alivePlayers.Count;
            if (alivePlayersCount < 1)
            {
                contexts.Game.CreateEntity().SafelyAddGameStateSwitchRequest(GameStates.GameOver);
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                contexts.Game.CreateEntity().SafelyAddGameStateSwitchRequest(Time.timeScale < 1
                    ? GameStates.Start
                    : GameStates.Pause);
            }
        }

        private void ContinueGame()
        {
            contexts.Game.CreateEntity().SafelyAddGameStateSwitchRequest(GameStates.Start);
        }

        private void RestartGame()
        {
            contexts.Game.CreateEntity().SafelyAddGameStateSwitchRequest(GameStates.Restart);
        }

        private void QuitGame()
        {
            contexts.Game.CreateEntity().SafelyAddGameStateSwitchRequest(GameStates.Exit);
        }
    }
}
