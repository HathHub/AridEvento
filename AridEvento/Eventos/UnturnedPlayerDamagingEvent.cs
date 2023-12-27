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
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{
    public class UnturnedPlayerDamagingEventListener : IEventListener<UnturnedPlayerDamagingEvent>
    {
        public UnturnedPlayerDamagingEventListener()
        {
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerDamagingEvent @event)
        {
            @event.RagdollEffect = ERagdollEffect.ZERO_KELVIN;
            return Task.CompletedTask;
        }
    }
}