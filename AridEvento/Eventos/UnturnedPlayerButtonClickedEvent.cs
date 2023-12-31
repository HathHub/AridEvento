using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventoMX.Points.Kits;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using OpenMod.API.Commands;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Core.Commands;
using OpenMod.Core.Users.Events;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Players.Connections.Events;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Players.UI.Events;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{

    public class UnturnedPlayerButtonClickedEventEventListener : IEventListener<UnturnedPlayerButtonClickedEvent>
    {
        private readonly IUnturnedUserDirectory _userDirectory;
        private readonly ICommandExecutor _commandExecutor;
        public UnturnedPlayerButtonClickedEventEventListener(IUnturnedUserDirectory userDirectory,
            ICommandExecutor commandExecutor,
            IServiceProvider serviceProvider)
        {
            _userDirectory = userDirectory;
            _commandExecutor = commandExecutor;
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerButtonClickedEvent @event)
        {
            String button = @event.ButtonName;
            UnturnedPlayer player = @event.Player;
            CSteamID steamid = player.SteamId;
            if (button.StartsWith("kit"))
            {
                if (Kits.KitsSelected.ContainsKey(steamid))
                {
                    Kits.KitsSelected[steamid] = button.Replace("kit_", "");
                }
                else
                {
                    Kits.KitsSelected.Add(steamid, button.Replace("kit_", ""));
                }
            }
            else
            {
                EffectManager.askEffectClearByID(22014, steamid);
                System.Threading.Tasks.Task.Delay(100);
                player.PlayerLife.ServerRespawn(false);
                var executingPlayer = _userDirectory.GetUser(player.Player);
                String command = Kits.KitsSelected.ContainsKey(steamid) ? Kits.KitsSelected[steamid] : "paintball";
                _commandExecutor.ExecuteAsync(executingPlayer, new string[] { "kit", command }, string.Empty);
            }
            return Task.CompletedTask;
        }

    }
}