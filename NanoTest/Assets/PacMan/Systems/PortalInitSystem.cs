using NanoEcs;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan.Gameplay
{
    public class PortalInitSystem : SystemEcs, Iinitializable
    {
        PacManContexts contexts;

        public PortalInitSystem(PacManContexts contexts) { this.contexts = contexts; }

        public void Initialize()
        {
            var portals = GameObject.FindGameObjectsWithTag("Portal");
            var channelDict = new Dictionary<int, GameEntity>();
            var filledChannels = new HashSet<int>();

            foreach (var portal in portals)
            {
                var channelNum = GetChannelFrom(portal);
                if (!channelNum.HasValue)
                {
                    Debug.LogError($"Portal {portal.name} has wrong name!");
                    continue;
                }

                var channel = channelNum.Value;
                if (filledChannels.Contains(channel))
                {
                    Debug.LogError($"Channel {channel.ToString()} for portal {portal.name} already used!");
                    continue;
                }

                var portalEntity = contexts.Game.CreateEntity();
                portalEntity.AddWorldObjectCreateRequest(portal.transform);
                portalEntity.AddPortal(null, 0.0f);
                PortalComponent portalComponent = portalEntity.Portal;

                if (channelDict.ContainsKey(channel))
                {
                    filledChannels.Add(channel);
                    var otherPortalEntity = channelDict[channel];
                    portalComponent.OtherPortalEntity = otherPortalEntity;
                    otherPortalEntity.Portal.OtherPortalEntity = portalEntity;
                }
                else
                {
                    channelDict.Add(channel, portalEntity);
                }
            }
        }

        private static int? GetChannelFrom(UnityEngine.Object portal)
        {
            var colonPosition = portal.name.IndexOf(':');
            if (colonPosition < 0) return null;

            var channelString = portal.name.Substring(colonPosition + 1, 1);
            return int.TryParse(channelString, out var channelNum)
                ? (int?)channelNum
                : null;
        }
    }
}
