using NanoEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class GameStateSystem : SystemEcs, IExecutable
    {
        readonly PacManContexts contexts;

        readonly GameCollector menus;
        readonly GameCollector requests;

        public GameStateSystem(PacManContexts contexts)
        {
            this.contexts = contexts;

            menus = contexts.Game.GetGroup()
                .With.PauseMenu
                .OnAdd;

            requests = contexts.Game.GetGroup()
                .With.GameStateSwitchRequest
                .OnAdd;
        }

        public void Execute()
        {
            if (!requests.IsNotEmpty) return;

            var gameState = GameStates.Start;
            var needDisableMenu = false;
            var needEnableMenu = false;
            foreach (var e in requests)
            {
                gameState = e.GameStateSwitchRequest.NewState;
                switch (gameState)
                {
                    case GameStates.Pause:
                        Time.timeScale = 0f;
                        needEnableMenu = true;
                        break;
                    case GameStates.Start:
                        Time.timeScale = 1f;
                        needDisableMenu = true;
                        break;
                    case GameStates.Restart:
                        SceneManager.LoadScene(0);
                        break;
                    case GameStates.Exit:
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                        break;
                    case GameStates.GameOver:
                        needEnableMenu = true;
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            if (needDisableMenu == needEnableMenu) return;

            foreach (var e in menus)
            {
                var menu = e.PauseMenu;

                var isGameOver = gameState == GameStates.GameOver;
                menu.MenuText.gameObject.SetActive(isGameOver);
                menu.ContinueBtn.interactable = !isGameOver;
                menu.Root.SetActive(needEnableMenu);
            }
        }
    }
}
