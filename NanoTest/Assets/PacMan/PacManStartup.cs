using Assets.PacMan.Systems;
using NanoEcs;
using PacMan.Gameplay;
using UnityEngine;

public class PacManStartup : MonoStartup
{
    /// <summary>
    /// 游戏数值定义.
    /// </summary>
    public GameDefinitions gameDefinitions;

    /// <summary>
    /// 食物对象列表.
    /// </summary>
    [Header("Food")]
    public Transform[] foodTransforms;

    /// <summary>
    /// 墙注册.
    /// </summary>
    public WallRegistry wallRegistry;

    public PauseMunuRef pauseMunuRef;


    /// <summary>
    /// 能量对象列表.
    /// </summary>
    public Transform[] energizerTransforms;

    public override Systems Setup()
    {
        var contexts = new PacManContexts(gameDefinitions);
        var systems = new Systems(contexts);

        systems
            .Add(new WorldInitSystem(contexts))
            .Add(new PlayerInitSystem(contexts))
            .Add(new GhostInitSystem(contexts))
            .Add(new WallInitSystem(contexts, wallRegistry))
            .Add(new PortalInitSystem(contexts))
            .Add(new FoodInitSystem(contexts, foodTransforms, energizerTransforms))
            .Add(new ScoreTableInitSystem(contexts))

            .Add(new PlayerInputSystem(contexts))
            .Add(new GhostSystem(contexts))
            .Add(new MovementTargetSystem(contexts))
            .Add(new MovementSystem(contexts))
            .Add(new ItemSystem(contexts))
            .Add(new FoodSystem(contexts))
            .Add(new EnergizerSystem(contexts))

            //remove.
            .Add(new OneFrameRemove<ItemTakenEvent>(contexts))

            .Add(new GhostFearStateSystem(contexts))


            //remove.
            .Add(new OneFrameRemove<GhostFearStateRequest>(contexts))

            .Add(new PlayerDeathSystem(contexts))

            //remove.
            .Add(new OneFrameRemove<PlayerDeathRequest>(contexts))

            .Add(new PortalSystem(contexts))
            .Add(new TeleportSystem(contexts))

            //remove.
            .Add(new OneFrameRemove<TeleportToPositionRequest>(contexts))

            .Add(new WorldSystem(contexts))

            //remove.
            .Add(new OneFrameRemove<WorldObjectCreateRequest>(contexts))
            .Add(new OneFrameRemove<WorldObjectDestroyedEvent>(contexts))
            .Add(new OneFrameRemove<WorldObjectNewPositionRequest>(contexts))

            // UI 组件.
            .Add(new GameStateInputSystem(contexts))
            .Add(new GameStateSystem(contexts))
            //remove.
            .Add(new OneFrameRemove<GameStateSwitchRequest>(contexts))
            .Add(new ScoreTableSystem(contexts))

            //remove.
            .Add(new OneFrameRemove<PlayerScoreChangedEvent>(contexts))
            ;


        // 增加菜单组件.
        contexts.Game.CreateEntity().AddPauseMenu(
            pauseMunuRef.root, pauseMunuRef.menuText,
            pauseMunuRef.continueBtn, pauseMunuRef.restartBtn,
            pauseMunuRef.quitBtn);

        // 增加游戏状态组件..
        contexts.Game.CreateEntity().AddGameStateSwitchRequest(GameStates.Start);

        return systems;
    }



    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        systems.Initialize();
    }

    void Update()
    {
        systems.Execute();
        systems.LateExecute();
    }

    void OnDestroy()
    {
        systems.Stop();
    }
}
