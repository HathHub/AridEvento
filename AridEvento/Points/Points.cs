using System;
using System.Collections.Generic;

using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventoMX.Points
{
    public class Points
    {
        public static Dictionary<CSteamID, int> PointsTrack = new Dictionary<CSteamID, int>();
    }
}
