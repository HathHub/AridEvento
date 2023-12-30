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
        public class PlayerData
        {
            public int Points { get; set; }
            public string PlayerName { get; set; }
        }

        public static Dictionary<CSteamID, PlayerData> PointsTrack = new Dictionary<CSteamID, PlayerData>();

    }
}
