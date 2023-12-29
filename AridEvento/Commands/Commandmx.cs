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
using static UnityEngine.UI.Selectable;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

namespace EventoMX.Commands
{
    [Command("mx")]
    public class CommandMx : OpenMod.Core.Commands.Command
    {
        public CommandMx(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;
            List<Vector3> allVectors = new List<Vector3>
        {
            // Points
            new Vector3(-5f, -1f),
            new Vector3(-5.4f, -1.2f),
            new Vector3(-5.3f, -1.15f),
            new Vector3(-5.2f, -1.3f),
            new Vector3(-5.1f, -1.45f),
            new Vector3(-5f, -1.4f),
            new Vector3(-4.8f, -1f),
            new Vector3(-4.6f, -1f),
            new Vector3(-4.4f, -1f),
            new Vector3(-4.6f, -1.2f),
            new Vector3(-4.6f, -1.4f),
            new Vector3(-4.6f, -1.6f),
            new Vector3(-5.8f, -2f),
            new Vector3(-5.8f, -2.2f),

            // Line Segments
            new Vector3(-5.4f, -1.6f),
            new Vector3(-5.4f, -1.4f),
            new Vector3(-5.4f, -1.2f),
            new Vector3(-5f, -1.6f),
            new Vector3(-5f, -1.2f),
            new Vector3(-5f, -1.6f),
            new Vector3(-5f, -1f),

            // Intersection
            new Vector3(-5.0677286388647f, -2.198407f),
            new Vector3(-4.9333759047731f, -2.20096f),
            new Vector3(-5.1330180852235f, -2.49952f),
            new Vector3(-4.8658610102057f, -2.501208f),
            new Vector3(-4.9316239961975f, -2.402564f),
            new Vector3(-5.0664194831454f, -2.39962f),
            new Vector3(-5.7f, -2.1f),
            new Vector3(-5.5f, -2.1f),
        };
            await UniTask.SwitchToMainThread();
            foreach (Vector3 vector in allVectors)
            {
                Vector3 transformedVector = vector + user.Player.Player.transform.position;

                // Use or store the transformedVector as needed
                EffectManager.sendEffect(124, EffectManager.INSANE, transformedVector);
            }
            await UniTask.CompletedTask;
        }
    }
}