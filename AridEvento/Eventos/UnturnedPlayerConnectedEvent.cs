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
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent>
    {
        public UnturnedPlayerConnectedEventListener()
        {
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerConnectedEvent @event)
        {

            Player player = @event.Player.Player;
            player.clothing.ServerSetVisualToggleState(EVisualToggleType.COSMETIC, true);
            {
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowVirus);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowFood);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowWater);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowOxygen);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy);
            }


            return Task.CompletedTask;
        }
    }
}