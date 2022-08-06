/*using System.Collections;
using Game.Inventory;
using Mirror;
using UnityEngine;

namespace Game.Core
{
    public class SceneObject : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnChangeEquipment))]
        public short equippedItemID;

        void OnChangeEquipment(short oldEquippedItemID, short newEquippedItemID)
        {
            StartCoroutine(ChangeEquipment(newEquippedItemID));
        }

        // Since Destroy is delayed to the end of the current frame, we use a coroutine
        // to clear out any child objects before instantiating the new one
        private IEnumerator ChangeEquipment(short newEquippedItemID)
        {
            while (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
                yield return null;
            }

            // Use the new value, not the SyncVar property value
            SetEquippedItem(newEquippedItemID);
        }

        // SetEquippedItem is called on the client from OnChangeEquipment (above),
        // and on the server from CmdDropItem in the PlayerEquip script.
        public void SetEquippedItem(short newEquippedItemID)
        {
            if (newEquippedItemID >= 0)
            {
                Item found = DatabaseManager.GetItemByID(newEquippedItemID);
                
                if (found)
                    Instantiate(found.Prefab, transform);
                else
                    Debug.LogError($"Item with id {newEquippedItemID} was not found");
            }
            else
                Debug.LogError($"Invalid item id: {newEquippedItemID}");
            
        }
    }
}
*/