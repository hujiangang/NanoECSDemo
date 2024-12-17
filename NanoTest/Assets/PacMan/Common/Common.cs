using UnityEngine;

public class Common
{

}

public enum GameStates
{
    Start,
    Pause,
    Restart,
    Exit,
    GameOver,
}

public enum GhostTypes
{
    Blinky,
    Pinky,
    Inky,
    Clyde,
}

public static class GhostComponentExtensions
{
    private static readonly int mainColorId = Shader.PropertyToID("_Color");

    public static void SetColor(this GhostComponent ghost, Color color)
    {
        ghost.MaterialPropertyBlock.SetColor(mainColorId, color);
        ghost.Renderer.SetPropertyBlock(ghost.MaterialPropertyBlock);
    }
}