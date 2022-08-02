using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// This class represents a <b>Item</b>'s Database Manager Singleton.
    /// It holds a Database Scriptable Object which must contain all game items.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        #region Singleton
        
        private static DatabaseManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject); // never destroyed between scene transitions
            }
            else Destroy(this); // already have an instance
        }
        
        #endregion

        [SerializeField] private ItemDatabase db;

        public static Item GetItemByID(short id) => 
            _instance.db.allItems.FirstOrDefault(i => i.Id == id); // Default is null
        
        public static Item GetRandomItem() =>
            _instance.db.allItems[Random.Range(0, _instance.db.allItems.Count())];
        
        public static List<Item> ListAllItems() => _instance.db.allItems;
    }
}