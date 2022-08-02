using Unity.VisualScripting;
using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// An <c>Item</c> represents any item witch can be equipped and dropped in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private new string name = "New Item";
        [SerializeField] private short id; // unique id for each individual item (used to instantiate the item)
        [SerializeField] [TextArea] private string description = "Some Item Description";
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite icon;

        public short Id => id;
        public string Description => description;
        public GameObject Prefab => prefab;
        public Sprite Icon => icon;
        public string Name => name;

        public static short NOTHING_ID = -1; // represents an empty slot
    }
}