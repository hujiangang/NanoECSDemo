using System.Linq;
using UnityEngine;

namespace PacMan.Gameplay
{
    public sealed class WallRegistry : MonoBehaviour
    {
        public Transform[] walls;

        [ContextMenu(nameof(FindAllWalls))]
        public void FindAllWalls()
        {
            walls = GameObject.FindGameObjectsWithTag("Wall").Select(x => x.transform).ToArray();
        }
    }
}
