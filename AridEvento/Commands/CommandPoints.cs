using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventoMX.Behaviors;
using EventoMX.Behaviours.Presents;
using EventoMX.FireworkData;
using EventoMX.Points.Kits;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Core.Users.Events;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using UnityEngine;
using static SDG.Unturned.WeatherAsset;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Commands
{
    [Command("points")]
    public class CommandPoints : OpenMod.Core.Commands.Command
    {
        Points.Points pointsInstance = new Points.Points();
        public CommandPoints(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;
            int points = Points.Points.PointsTrack.TryGetValue(user.SteamId, out int value) ? value : 0;
            await Context.Actor.PrintMessageAsync($"{points}");
            await UniTask.CompletedTask;
        }
    }
}