using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using Steamworks;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("EventoMX", DisplayName = "My OpenMod Plugin")]

namespace EventoMX
{
    public class EventoMX : OpenModUnturnedPlugin
    {
        private Harmony harmony;
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ILogger<EventoMX> m_Logger;

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

        }
        protected override async UniTask OnUnloadAsync()
        {
            // await UniTask.SwitchToMainThread(); uncomment if you have to access Unturned or UnityEngine APIs
            m_Logger.LogInformation(m_StringLocalizer["plugin_events:plugin_stop"]);
            harmony.UnpatchAll("HandCuff_Oasis");
        }
    }
}
