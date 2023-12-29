using System.Collections.Generic;
using EventoMX.KitModel;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventoMX.Kits
{
    public class Kits
    {
        public List<Kit> KitList { get; set; } = new List<Kit>
        {
            new Kit
            {
                items = new List<CItem>
                {
                },
                bindings = new List<Binding>
                {
                    new Binding
                    {
                        key = 3,
                        id = 15
                    },
                }
            },
        };
    }
}
