using System;
using System.Collections.Generic;

using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventoMX.Points.Kits
{
    public class Kits
    {
        public static Dictionary<CSteamID, String> KitsSelected = new Dictionary<CSteamID, String>();
        public static List<String> KitList { get; set; } = new List<String>
        {
            "NEMESIS",
            "DRTP",
            "PAINTBALL"
        };
    }
}
