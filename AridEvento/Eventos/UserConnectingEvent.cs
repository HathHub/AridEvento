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
            #region Appareal
            {
                m_Logger.LogInformation(_pendingPlayer.beard.ToString());
                m_Logger.LogInformation(_pendingPlayer.hair.ToString());
                m_Logger.LogInformation(_pendingPlayer.face.ToString());

                //_pendingPlayer.beard = 0;
            }
            #endregion
            #region Clothes
            {
                _pendingPlayer.hatItem = 63501;
                _pendingPlayer.shirtItem = 63601;
                _pendingPlayer.pantsItem = 63701;
                _pendingPlayer.backpackItem = 94600;
                _pendingPlayer.maskItem = 0;
                _pendingPlayer.vestItem = 0;
                _pendingPlayer.skinItems.ToList().Add(63601);
            }
            #endregion
            return Task.CompletedTask;
        }
    }
}