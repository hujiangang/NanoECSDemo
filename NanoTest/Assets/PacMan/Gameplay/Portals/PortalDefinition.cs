using UnityEngine;

namespace PacMan.Gameplay
{
    [CreateAssetMenu(menuName = "PacManEcs/" + nameof(PortalDefinition))]
    public sealed class PortalDefinition : ScriptableObject
    {
        public float portalReloadTime = 1f;
    }
}