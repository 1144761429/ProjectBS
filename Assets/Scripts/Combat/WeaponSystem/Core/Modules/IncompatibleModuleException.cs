using System;

namespace Combat.WeaponSystem.Core.Modules
{
    public class IncompatibleModuleException : Exception
    {
        public readonly int ID;
        public Type IncompatibleModuleType;
        
        public IncompatibleModuleException(int id, Type incompatibleModuleType, string message) : base(message)
        {
            ID = id;
            IncompatibleModuleType = incompatibleModuleType;
        }
    }
}