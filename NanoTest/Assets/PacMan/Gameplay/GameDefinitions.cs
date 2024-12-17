using UnityEngine;

namespace PacMan.Gameplay
{
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(GameDefinitions))]
    public sealed class GameDefinitions : ScriptableObject
    {
        public WorldDefinition worldDefinition;
        public PlayerDefinition playerDefinition;
        public FoodDefinition foodDefinition;
        public GhostDefinition ghostDefinition;
        public PortalDefinition portalDefinition;
    }
}