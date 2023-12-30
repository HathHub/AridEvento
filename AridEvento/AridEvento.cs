using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Cysharp.Threading.Tasks;
using EventoMX.Behaviours.Presents;
using HarmonyLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("EventoMX", DisplayName = "My OpenMod Plugin")]

namespace EventoMX
{
    public class EventoMX : OpenModUnturnedPlugin
    {
        private Harmony harmony;
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;
        private  readonly ILogger<EventoMX> m_Logger;
        private static System.Timers.Timer timer;

        public static List<Vector3>  Coordinates = new List<Vector3>
        {
            new Vector3(980.60f, 0.10f, 452.20f),
            new Vector3(966.85f, 0.10f, 441.28f),
            new Vector3(978.80f, 0.10f, 426.40f),
            new Vector3(972.60f, 0.30f, 413.03f),
            new Vector3(916.25f, 0.33f, 415.04f),
            new Vector3(843.53f, 0.36f, 419.58f),
            new Vector3(789.41f, 0.30f, 382.56f),
            new Vector3(784.05f, 0.30f, 329.45f),
            new Vector3(779.47f, 0.30f, 281.15f),
            new Vector3(775.33f, 0.30f, 259.15f),
            new Vector3(772.53f, 0.30f, 246.29f),
            new Vector3(769.19f, 0.30f, 238.13f),
            new Vector3(772.93f, 0.30f, 217.65f),
            new Vector3(761.82f, 0.30f, 194.23f),
            new Vector3(750.33f, 0.30f, 171.68f),
            new Vector3(748.51f, 0.30f, 160.02f),
            new Vector3(756.15f, 0.30f, 136.56f),
            new Vector3(759.71f, 0.30f, 119.60f),
            new Vector3(752.16f, 0.30f, 102.91f),
            new Vector3(752.86f, 0.30f, 93.87f),
            new Vector3(755.67f, 0.30f, 77.12f),
            new Vector3(763.91f, 0.30f, 55.48f),
            new Vector3(762.05f, 0.30f, 48.10f),
            new Vector3(765.67f, 0.30f, 42.37f),
            new Vector3(778.76f, 0.30f, 31.33f),
            new Vector3(790.40f, 0.30f, 26.26f),
            new Vector3(800.79f, 0.30f, 22.21f),
            new Vector3(809.43f, 0.30f, 21.77f),
            new Vector3(823.51f, 0.30f, 18.22f),
            new Vector3(838.47f, 0.30f, 17.42f)
        };

    public EventoMX(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<EventoMX> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            harmony = new Harmony("HandCuff_Oasis");
            harmony.PatchAll();
            m_Logger.LogInformation("Hello World!");

            double interval = 5 * 1000;
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            await UniTask.SwitchToMainThread();
            timer.Enabled = true;

        }

        [Obsolete]
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            int randomIndex = new System.Random().Next(0, Coordinates.Count);
            Vector3 randomCoordinate = Coordinates[randomIndex];

            var barricade = new Barricade(GetRandombarricade());
            Present.SpawnPresent(randomCoordinate, barricade);
        }
        private static ushort GetRandombarricade()
        {
            if (PresentTypes.PresentTypesList.Count == 0)
            {
                return 0;
            }
            int randomIndex = new System.Random().Next(PresentTypes.PresentTypesList.Count);
            return PresentTypes.PresentTypesList[randomIndex];
        }
        protected override async UniTask OnUnloadAsync()
        {
            timer.Stop();
            // await UniTask.SwitchToMainThread(); uncomment if you have to access Unturned or UnityEngine APIs
            m_Logger.LogInformation(m_StringLocalizer["plugin_events:plugin_stop"]);
            harmony.UnpatchAll("HandCuff_Oasis");
        }
    }
}
