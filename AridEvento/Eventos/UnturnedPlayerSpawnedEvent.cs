using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventoMX.KitModel;
using EventoMX.Kits;
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

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

// ... other using statements ...

namespace EventoMX.Eventos
{
    public class UnturnedPlayerRevivedEventListener : IEventListener<UnturnedPlayerSpawnedEvent>
    {
        private readonly ILogger<UserConnectingEventListener> m_Logger;
        public Kits.Kits KitList { get; set; } = new Kits.Kits();


        private readonly Random m_Random = new Random();

        public UnturnedPlayerRevivedEventListener(ILogger<UserConnectingEventListener> logger)
        {
            m_Logger = logger;
            KitList = new Kits.Kits();
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
            Kit randomKit = GetRandomKit();
            m_Logger.LogInformation(KitList.KitList.Count.ToString());
            foreach (CItem item in randomKit.items)
            {
                player.inventory.tryAddItem(new Item(item.id, true, 100), item.x, item.y, item.page, item.rot);
            }
            foreach (Binding binding in randomKit.bindings)
            {
                ItemAsset Asset = Assets.find(EAssetType.ITEM, binding.id) as ItemAsset;
                player.equipment.ServerBindItemHotkey(binding.key, Asset, 2, 4, 5);
            }
            return Task.CompletedTask;
        }

        private Kit GetRandomKit()
        {
            if (KitList.KitList.Count == 0)
            {
                // Handle the case where there are no kits available
                return null;
            }

            int randomIndex = m_Random.Next(KitList.KitList.Count);
            return KitList.KitList[randomIndex];
        }
    }
}
