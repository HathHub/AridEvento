using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using OpenMod.API.Commands;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Users.Events;
using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Players.Connections.Events;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent>
    {
        private readonly IUnturnedUserDirectory _userDirectory;
        private readonly ICommandExecutor _commandExecutor;
        public UnturnedPlayerConnectedEventListener(
        IUnturnedUserDirectory userDirectory,
            ICommandExecutor commandExecutor,
            IServiceProvider serviceProvider)
        {
            _userDirectory = userDirectory;
            _commandExecutor = commandExecutor;
        }

    public Task HandleEventAsync(object sender, UnturnedPlayerConnectedEvent @event)
        {

            Player player = @event.Player.Player;
            CSteamID steamId = @event.Player.SteamId;
            player.clothing.ServerSetVisualToggleState(EVisualToggleType.COSMETIC, true);
            {
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowVirus);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowFood);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowWater);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowOxygen);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy);
                player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowDeathMenu);
            }
            if (@event.Player.Player.inventory.items.Length > 0)
            {
                for (int page = 0; page < PlayerInventory.PAGES - 2; page++)
                {
                    int count = @event.Player.Player.inventory.getItemCount((byte)page);

                    for (int index = 0; index < count; index++)
                    {
                        @event.Player.Player.inventory.removeItem((byte)page, 0);
                    }
                }
                var executingPlayer = _userDirectory.GetUser(@event.Player.Player);
                _commandExecutor.ExecuteAsync(executingPlayer, new string[] { "kit", "nemesis" }, string.Empty);
            }
            EffectManager.sendUIEffect(22000, 13, steamId, true);
            if (Points.Points.PointsTrack.ContainsKey(steamId))
            {
                EffectManager.sendUIEffectText(13, steamId, true, "BoxText (1)", Points.Points.PointsTrack[steamId].ToString());
            }
            else
            {
                EffectManager.sendUIEffectText(13, steamId, true, "BoxText (1)", 0.ToString());
            }
            return Task.CompletedTask;
        }
    }
}