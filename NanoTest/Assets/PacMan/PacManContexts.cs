using PacMan.Gameplay;

public partial class PacManContexts : Contexts
{
    public GameDefinitions GameDefinitions;
    public readonly WorldService worldService = null;
    public System.Random random;

    public PacManContexts(GameDefinitions gameDefinitions) : base()
    {
        GameDefinitions = gameDefinitions;
        random = new System.Random();
        worldService = new();
    }
}
