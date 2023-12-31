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
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using UnityEngine;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Eventos
{
    public class UserConnectingEventListener : IEventListener<IUserConnectingEvent>
    {
        private readonly ILogger<UserConnectingEventListener> m_Logger;
        public UserConnectingEventListener(ILogger<UserConnectingEventListener> logger)
        {
            m_Logger = logger;
        }

        public Task HandleEventAsync(object sender, IUserConnectingEvent @event)
        {
            SteamPending _pendingPlayer = Provider.pending.FirstOrDefault(x => x.playerID.steamID.m_SteamID == ulong.Parse(@event.User.Id));
            // Appareal
            {
                /* var hair = typeof(SteamPending).GetField("_hair", BindingFlags.NonPublic | BindingFlags.Instance);
                hair.SetValue(_pendingPlayer, 0); 

                var beard = typeof(SteamPending).GetField("_beard", BindingFlags.NonPublic | BindingFlags.Instance);
                beard.SetValue(_pendingPlayer, 8);

                var skinColor = typeof(SteamPending).GetField("_skin", BindingFlags.NonPublic | BindingFlags.Instance);

                skinColor.SetValue(_pendingPlayer, Color.white);

                var hairColor = typeof(SteamPending).GetField("_color", BindingFlags.NonPublic | BindingFlags.Instance);
                // Convert UnityEngine.Color to Color32
                hairColor.SetValue(_pendingPlayer, Color.white); */

            }
            // Clothes
            {
                _pendingPlayer.hatItem = 63501;
                _pendingPlayer.shirtItem = 63601;
                _pendingPlayer.pantsItem = 63701;
                _pendingPlayer.backpackItem = 94600;
                _pendingPlayer.vestItem = 64801;
            }
            return Task.CompletedTask;
        }
    }
}