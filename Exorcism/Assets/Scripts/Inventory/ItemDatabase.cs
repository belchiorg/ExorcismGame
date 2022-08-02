using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// This is an Scriptable Object that can be created in the editor.
    /// You can create some Item Databases to use in the <b>Database Manager</b>
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Database", menuName = "ScriptableObjects/Inventory/Item Database")]
    public class ItemDatabase: ScriptableObject
    {
        public List<Item> allItems;
    }
}