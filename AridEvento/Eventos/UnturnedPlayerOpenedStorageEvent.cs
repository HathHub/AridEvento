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

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

// ... other using statements ...

namespace EventoMX.Eventos
{
    public class UnturnedPlayerOpenedStorageEventListener : IEventListener<UnturnedPlayerOpenedStorageEvent>
    {
        private readonly ILogger<UnturnedPlayerOpenedStorageEventListener> m_Logger;
        Points.Points pointsInstance = new Points.Points();

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
            if(barricade.asset.id == 59708)
            {
                ChatManager.serverSendMessage($"<b>{@event.Player.Player.name}</b> tomó el Airdrop", Color.white, useRichTextFormatting: true);
            }
            else
            {
                ChatManager.serverSendMessage($"<b>{@event.Player.Player.name}</b> encontró un Regalo", Color.white, useRichTextFormatting: true);
            }
            
            int pointsToAdd = 2;
            if (Points.Points.PointsTrack.ContainsKey(steamId))
            {
                // If the SteamID already exists, add points to the existing total
                Points.Points.PointsTrack[steamId] += pointsToAdd;
            }
            else
            {
                // If the SteamID doesn't exist, add a new entry with the specified points
                Points.Points.PointsTrack.Add(steamId, pointsToAdd);
            }
            return Task.CompletedTask;
        }
         
    }
}
