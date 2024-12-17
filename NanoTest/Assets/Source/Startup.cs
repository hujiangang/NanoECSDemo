using NanoEcs;

public class Startup : MonoStartup
{
    public override Systems Setup()
    {
        var contexts = new Contexts();
        var systems = new Systems(contexts);

        systems
            .Add(new HelloWorldSystem(contexts))
            // Add new systems here
            ;

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
