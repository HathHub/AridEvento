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
using OpenMod.Core.Users.Events;
using OpenMod.Unturned.Players.Connections.Events;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{
    public class UnturnedPlayerDyingEventEventListener : IEventListener<UnturnedPlayerDyingEvent>
    {
        public UnturnedPlayerDyingEventEventListener()
        {
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerDyingEvent @event)
        {
            for (int page = 0; page < PlayerInventory.PAGES - 2; page++)
            {
                int count = @event.Player.Player.inventory.getItemCount((byte)page);

                for (int index = 0; index < count; index++)
                {
                    @event.Player.Player.inventory.removeItem((byte)page, 0);
                }
            }
            return Task.CompletedTask;
        }
    }
}