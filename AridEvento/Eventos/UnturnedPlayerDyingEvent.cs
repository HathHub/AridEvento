using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventoMX.Points;
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
using UnityEngine;
using static EventoMX.Points.Points;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{

    public class UnturnedPlayerDyingEventEventListener : IEventListener<UnturnedPlayerDyingEvent>
    {
        Points.Points pointsInstance = new Points.Points();
        private readonly IUnturnedUserDirectory m_provider;
        List<string> holidayMessages = new List<string>
{
    "got iced",
    "got his holiday spirit melted away",
    " got wrapped up in skills",
    "got Christmas plans snowed under",
    "got tangled in tinsel",
    "got his gingerbread dreams crumbled",
    "got roasted by holiday firepower",
    "got jingled",
    "sleigh ride ended abruptly",
    "festive cheer got Grinched"
};

        public UnturnedPlayerDyingEventEventListener(IUnturnedUserDirectory provider)
        {
            m_provider = provider;
        }

        [Obsolete]
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

            CSteamID Killer = @event.Killer;
            var killer = m_provider.FindUser(Killer);
            CSteamID Victim = @event.Player.SteamId;
            int points = 1;
            if (Killer != @event.Player.SteamId && killer is not null)
            {
                string randomHolidayMessage = holidayMessages[new System.Random().Next(holidayMessages.Count)];

                ChatManager.serverSendMessage($"<b>{@event.Player.Player.channel.owner.playerID.characterName}</b> <color=green>{randomHolidayMessage}</color> by <b>{killer.Player.Player.channel.owner.playerID.characterName}</b>[<color=red>{killer.Player.PlayerLife.health.ToString()}HP</color>]", Color.white, useRichTextFormatting: true);
                if (@event.Cause == EDeathCause.MELEE)
                {
                    points = 2;
                }

                if (Points.Points.PointsTrack.ContainsKey(Victim))
                {
                    if(points == 2)
                    {
                        if (Points.Points.PointsTrack[Victim].Points >= 2) Points.Points.PointsTrack[Victim] = new PlayerData { Points = Points.Points.PointsTrack[Victim].Points - points, PlayerName = @event.Player.Player.channel.owner.playerID.characterName };
                    }
                }
                else
                {
                    Points.Points.PointsTrack.Add(Victim, new PlayerData { Points = 0, PlayerName = @event.Player.Player.channel.owner.playerID.characterName });
                }

                if (Points.Points.PointsTrack.ContainsKey(Killer))
                {
                    Points.Points.PointsTrack[Killer] = new PlayerData { Points = Points.Points.PointsTrack[Killer].Points + points, PlayerName = killer.Player.Player.channel.owner.playerID.characterName };
                }
                else
                {
                    Points.Points.PointsTrack.Add(Killer, new PlayerData { Points = 1, PlayerName = killer.Player.Player.channel.owner.playerID.characterName});
                }
                EffectManager.sendUIEffectText(13, Killer, true, "BoxText (1)", Points.Points.PointsTrack[Killer].Points.ToString());
                EffectManager.sendUIEffectText(13, Victim, true, "BoxText (1)", Points.Points.PointsTrack[Victim].Points.ToString());
            }

            EffectManager.sendUIEffect(22014, 15, Victim, true);
            {
                var uis = new List<String>
                {
                    "top_0",
                    "top_1",
                    "top_2",
                    "top_3",
                    "top_4",
                    "top_5",
                };
                var sortedEntries = Points.Points.PointsTrack.OrderByDescending(pair => pair.Value.Points)
                    .Select((pair, index) => new { Index = index, Entry = pair })
                    .ToList();
                foreach (var sortedEntry in sortedEntries)
                {
                    EffectManager.sendUIEffectText(15, Victim, true, $"top_{sortedEntry.Index.ToString()}", $"#{(sortedEntry.Index + 1).ToString()} {sortedEntry.Entry.Value.PlayerName} [{sortedEntry.Entry.Value.Points}]");
                }
            }
            return Task.CompletedTask;
        }
    }
}