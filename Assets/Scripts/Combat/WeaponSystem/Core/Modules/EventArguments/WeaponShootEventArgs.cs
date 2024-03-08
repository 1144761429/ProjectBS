using System;
using UnityEngine;

namespace Combat.WeaponSystem.Core.Modules.EventArguments
{
    public class WeaponShootEventArgs : EventArgs
    {
        public readonly int ID;
        public readonly GameObject Wielder;
        
        public WeaponShootEventArgs(int id, GameObject wielder)
        {
            ID = id;
            Wielder = wielder;
        }
    }
}