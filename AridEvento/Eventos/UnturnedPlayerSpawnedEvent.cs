using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HarmonyLib;
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
using OpenMod.Extensions.Games.Abstractions.Players;
using OpenMod.API.Commands;
using OpenMod.Core.Console;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;
using EventoMX.Points;
using EventoMX.Points.Kits;
using Steamworks;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

// ... other using statements ...

namespace EventoMX.Eventos
{
    public class UnturnedPlayerRevivedEventListener : IEventListener<UnturnedPlayerSpawnedEvent>
    {
        private readonly ILogger<UserConnectingEventListener> m_Logger;
        private readonly IUnturnedUserDirectory _userDirectory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly Random m_Random = new Random();
        public UnturnedPlayerRevivedEventListener(IUnturnedUserDirectory userDirectory,
            ICommandExecutor commandExecutor,
            IServiceProvider serviceProvider)
        {
            _userDirectory = userDirectory;
            _commandExecutor = commandExecutor;
        }
        public Task HandleEventAsync(object sender, UnturnedPlayerSpawnedEvent @event)
        {

            var asset = Assets.find(EAssetType.EFFECT, m_Random.Next(3) switch { 0 => 124, 1 => 130, _ => 134 });
            Player player = @event.Player.Player;
            var eff = new TriggerEffectParameters((EffectAsset)asset)
            {
                position = player.transform.position,
                relevantDistance = 10
            };
            EffectManager.triggerEffect(eff);
            return Task.CompletedTask;
        }
         
    }
}
