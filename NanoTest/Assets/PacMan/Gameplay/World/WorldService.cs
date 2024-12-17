using NanoEcs;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan.Gameplay
{
    public sealed class WorldService
    {
        public HashSet<GameEntity>[][] worldField;

        public HashSet<GameEntity> GetEntitiesOn(in Vector2Int position) => worldField[position.x][position.y];
    }
}
