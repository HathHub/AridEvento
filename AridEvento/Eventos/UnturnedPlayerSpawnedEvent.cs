using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Helpers;
using OpenMod.Core.Users.Events;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{
    public class UnturnedPlayerRevivedEventListener : IEventListener<UnturnedPlayerSpawnedEvent>
    {
        private readonly ILogger<UserConnectingEventListener> m_Logger;
        public UnturnedPlayerRevivedEventListener(ILogger<UserConnectingEventListener> logger)
        {
            m_Logger = logger;
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerSpawnedEvent @event)
        {
            var asset = Assets.find(EAssetType.EFFECT, new Random().Next(3) switch { 0 => 124, 1 => 130, _ => 134 });
            var eff = new TriggerEffectParameters((EffectAsset)asset)
            {
                position = @event.Player.Transform.Position.ToUnityVector(),
                relevantDistance = 10
            };
            EffectManager.triggerEffect(eff);
            return Task.CompletedTask;
        }
    }
}