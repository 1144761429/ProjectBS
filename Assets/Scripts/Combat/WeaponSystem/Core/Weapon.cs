using System.Collections.Generic;
using Combat.Core;
using UnityEngine;

namespace Combat.WeaponSystem.Core
{
    /// <summary>
    /// <para>
    /// An abstract class that represents a weapon.
    /// </para>
    ///
    /// <para>
    /// A <see cref="Weapon"/> is made of different modules.
    /// <example>
    /// <list type="number">
    /// <item>A gun = shoot module + ammunition module + aim module </item> TODO: make these three <see cref=""/>
    /// <item>A knife = melee attack module </item> TODO: make these three <see cref=""/>
    /// </list>
    /// </example>
    /// </para>
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        #region Fields and Properties

        /// <summary>
        /// <para>
        /// The ID of this <see cref="Weapon"/> in the database.
        /// </para>
        ///
        /// <para>
        /// 0 means no <see cref="Weapon"/> from the database needs to initialize and store into this component.
        /// </para>
        /// </summary>
        [field: SerializeField]
        public int DatabaseID { get; private set; }

        public GameObject Wielder { get; private set; }
        
        /// <summary>
        /// Maps a <see cref="EWeaponModuleType"/> to a <see cref="WeaponModule"/>.
        /// </summary>
        protected readonly Dictionary<EWeaponModuleType, WeaponModule> Modules =
            new Dictionary<EWeaponModuleType, WeaponModule>();
        
        #endregion
        
        /// <summary>
        /// <para>
        /// Initialize the <see cref="WeaponModule"/>s for this <see cref="Weapon"/> according to the specific type
        /// of this <see cref="Weapon"/>. 
        /// </para>
        /// 
        /// <para>
        /// This will remove all the existing <see cref="WeaponModule"/> and then initialize the new
        /// <see cref="WeaponModule"/>s.
        /// </para>
        /// </summary>
        public virtual void InitModules()
        {
            Modules.Clear();
            
            if (DatabaseID == 0)
            {
                return;
            }
        }
    }
}
