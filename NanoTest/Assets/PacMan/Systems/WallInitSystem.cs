using NanoEcs;

namespace PacMan.Gameplay
{
    public class WallInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        private readonly WallRegistry registry = null;

        public WallInitSystem(PacManContexts contexts, WallRegistry wallRegistry)
        {
            this.contexts = contexts;
            this.registry = wallRegistry;
        }

        public void Initialize()
        {
            foreach (var wall in registry.walls)
            {
                var e = contexts.Game.CreateEntity()
                    .AddWallMarker(0)
                    .AddWorldObjectCreateRequest(wall.transform);
            }
        }
    }
}
