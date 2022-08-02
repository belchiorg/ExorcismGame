using System.Collections;
using Mirror;
using UnityEngine;

namespace Game.Core
{
    public class SceneObject : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnChangeEquipment))]
        public EquippedItem equippedItem;

        public GameObject itemPrefab;

        void OnChangeEquipment(EquippedItem oldEquippedItem, EquippedItem newEquippedItem)
        {
            StartCoroutine(ChangeEquipment(newEquippedItem));
        }

        // Since Destroy is delayed to the end of the current frame, we use a coroutine
        // to clear out any child objects before instantiating the new one
        IEnumerator ChangeEquipment(EquippedItem newEquippedItem)
        {
            while (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
                yield return null;
            }

            // Use the new value, not the SyncVar property value
            SetEquippedItem(newEquippedItem);
        }

        // SetEquippedItem is called on the client from OnChangeEquipment (above),
        // and on the server from CmdDropItem in the PlayerEquip script.
        public void SetEquippedItem(EquippedItem newEquippedItem)
        {
            switch (newEquippedItem)
            {
                case EquippedItem.Item:
                    Instantiate(itemPrefab, transform);
                    break;
            }
        }
    }
}
