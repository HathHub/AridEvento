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
using OpenMod.Core.Users.Events;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Players.Connections.Events;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{

    public class UnturnedPlayerDyingEventEventListener : IEventListener<UnturnedPlayerDyingEvent>
    {
        Points.Points pointsInstance = new Points.Points();
        private readonly IUnturnedUserDirectory m_provider;
        public UnturnedPlayerDyingEventEventListener(IUnturnedUserDirectory provider)
        {
            m_provider = provider;
        }
        public Task HandleEventAsync(object sender, UnturnedPlayerDyingEvent @event)
        {
            Player _player = @event.Player.Player;

            _player.clothing.vestState = new byte[0];
            _player.clothing.hatState = new byte[0];
            _player.clothing.backpackState = new byte[0];
            for (int page = 0; page < PlayerInventory.PAGES - 2; page++)
            {
                int count = _player.inventory.getItemCount((byte)page);

                for (int index = 0; index < count; index++)
                {
                    _player.inventory.removeItem((byte)page, 0);
                }
            }
            UnturnedUser Killer = m_provider.FindUser(@event.Killer);
            if(Killer is not null)
            {
                int _points = 1;
                if (@event.Cause is EDeathCause.MELEE)
                {
                    _points++;
                    if (Points.Points.PointsTrack.ContainsKey(@event.Player.SteamId) && Points.Points.PointsTrack[@event.Player.SteamId] >= _points)
                    {
                        // If the SteamID already exists, add points to the existing total
                        Points.Points.PointsTrack[@event.Player.SteamId] -= _points;
                    }
                }
                CSteamID steamId = Killer.Player.SteamId;
                if (Points.Points.PointsTrack.ContainsKey(steamId))
                {
                    // If the SteamID already exists, add points to the existing total
                    Points.Points.PointsTrack[steamId] += _points;
                }
                else
                {
                    // If the SteamID doesn't exist, add a new entry with the specified points
                    Points.Points.PointsTrack.Add(steamId, _points);
                }
            }
            return Task.CompletedTask;
        }
    }
}