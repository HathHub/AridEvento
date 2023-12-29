using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventoMX.KitModel
{
    public class Binding
    {
        public byte key;
        public ushort id;
    }
    public class Kit
    {
        public List<CItem> items;
        public List<Binding> bindings;
    }
    public class CItem
    {
        public ushort id { get; set; }
        public byte y { get; set; }
        public byte x { get; set; }
        public byte page { get; set; }
        public byte rot { get; set; }
    }
}