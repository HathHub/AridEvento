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
    [Command("firework")]
    public class CommandFirework : OpenMod.Core.Commands.Command
    {
        public CommandFirework(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override async Task OnExecuteAsync()
        {
            if (await Context.Parameters.GetAsync<String>(0) == "e")
            {
                await UniTask.SwitchToMainThread();
                for (int i = 0; i < Provider.players.Count; i++)
                {
                    GameObject asd = new GameObject();
                    var fireworkObject = Provider.players[i].player.gameObject.GetOrAddComponent<FireworkBehaviourPlayer>();
                    fireworkObject.Launch();
                     await System.Threading.Tasks.Task.Delay(175);
                    
                }
                 await UniTask.CompletedTask;
                return;
            }
            if (await Context.Parameters.GetAsync<String>(0) == "all")
            {
                SpawnPoints spawnPointsInstance = new SpawnPoints();
                List<Vector3> points = spawnPointsInstance.FireworkSpawnpoints;
                await UniTask.SwitchToMainThread();
                for (int i = 0; i < points.Count; i++)
                {
                    GameObject asd = new GameObject();
                    var fireworkObject = asd.GetOrAddComponent<FireworkBehaviour>();
                    fireworkObject.Fuse = 5;
                    asd.transform.position = points[i];
                    fireworkObject.Launch();
                    await System.Threading.Tasks.Task.Delay(150);
                }
            }
            else if(Context.Parameters.Length > 0)
            {
                UnturnedUser user = await Context.Parameters.GetAsync<UnturnedUser>(0);
                var fireworkObject = user.Player.Player.gameObject.GetOrAddComponent<FireworkBehaviourPlayer>();
                await UniTask.SwitchToMainThread();
                fireworkObject.Launch();
            }
            else
            {
                UnturnedUser user = (UnturnedUser)Context.Actor;
                //var fireworkObject = user.Player.Player.gameObject.GetOrAddComponent<FireworkBehaviour>();
                GameObject asd = new GameObject();
                asd.transform.position = user.Player.Transform.Position.ToUnityVector();
                var fireworkObject = asd.GetOrAddComponent<FireworkBehaviour>();
                // ? ItemManager.dropItem(new Item(363, true), user.Player.Transform.Position.ToUnityVector(), false, false, false);
                //fireworkObject.Fuse = 10;
                //fireworkObject.TrailRate = 10;

                await UniTask.SwitchToMainThread();
                fireworkObject.Launch();
            }
            await UniTask.CompletedTask;
        }
    }
}