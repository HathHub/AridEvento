using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Cysharp.Threading.Tasks;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventoMX.Behaviours.Presents
{
    public class Present
    {
        public void SpawnPresent(Vector3 position, Barricade barricade)
        {
            var vector = new Vector3(position.x, position.y + 0.5f, position.z);
            Transform barr = BarricadeManager.dropNonPlantedBarricade(barricade, vector, Quaternion.Euler(new Vector3(270f, 0f, 180f)), (ulong)CSteamID.Nil, (ulong)CSteamID.Nil);
            var eff2 = new TriggerEffectParameters(120)
            {
                position = vector,
                relevantDistance = 10
            };
            EffectManager.triggerEffect(eff2);
        }
    }
}