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
using OpenMod.Unturned.Players.Inventory.Events;
using Steamworks;
using OpenMod.Unturned.Players.Chat.Events;
using UnityEngine;
using static EventoMX.Points.Points;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

// ... other using statements ...

namespace EventoMX.Eventos
{
    public class UnturnedPlayerOpenedStorageEventListener : IEventListener<UnturnedPlayerOpenedStorageEvent>
    {
        private readonly ILogger<UnturnedPlayerOpenedStorageEventListener> m_Logger;

        public UnturnedPlayerOpenedStorageEventListener(ILogger<UnturnedPlayerOpenedStorageEventListener> logger)
        {
            m_Logger = logger;
        }
        public Task HandleEventAsync(object sender, UnturnedPlayerOpenedStorageEvent @event)
        {
            UnityEngine.Transform trans = @event.Player.Player.inventory.storage.transform;
            BarricadeDrop barricade = BarricadeManager.FindBarricadeByRootTransform(trans);
            BarricadeManager.destroyBarricade(barricade, (byte)trans.position.x, (byte)trans.position.y, 0);
            CSteamID steamId = @event.Player.SteamId;
            int pointsToAdd = 2;
            if (barricade.asset.id == 59708)
            {
                pointsToAdd += 3;
                ChatManager.serverSendMessage($"<b>{@event.Player.Player.name}</b> tomó el Airdrop", Color.white, useRichTextFormatting: true);
                Item item = new Item(59389, true);
                Item item2 = new Item(59388, true);
                @event.Player.Player.inventory.tryAddItemAuto(item2, true, true, true, false);
                @event.Player.Player.inventory.tryAddItemAuto(item, true, true, true, false);
            }
            else
            {
                ChatManager.serverSendMessage($"<b>{@event.Player.Player.name}</b> encontró un Regalo", Color.white, useRichTextFormatting: true);

            }
            if (Points.Points.PointsTrack.ContainsKey(steamId))
            {
                Points.Points.PointsTrack[steamId] = new PlayerData { Points = Points.Points.PointsTrack[steamId].Points + pointsToAdd, PlayerName = @event.Player.Player.channel.owner.playerID.characterName };
            }
            else
            {
                // If the SteamID doesn't exist, add a new entry with the specified points
                Points.Points.PointsTrack.Add(steamId, new PlayerData { Points = Points.Points.PointsTrack[steamId].Points + pointsToAdd, PlayerName = @event.Player.Player.channel.owner.playerID.characterName });
            }
            EffectManager.sendUIEffectText(13, steamId, true, "BoxText (1)", (Points.Points.PointsTrack[steamId].Points + pointsToAdd).ToString());


            return Task.CompletedTask;
        }
         
    }
}
