using System;
using Mirror;
using System.Collections;
using Game.Inventory;
using UnityEngine;

namespace Game.Core
{
    public class EquipHand : NetworkBehaviour
    {
        [SerializeField] private GameObject sceneObjectPrefab;
        [SerializeField] private GameObject hand;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public short equippedItemID = Item.NOTHING_ID; // -1 stands for no item 

        void OnChangeEquipment(short oldEquipmentItemID, short newEquipmentItemID)
        {
            StartCoroutine(ChangeEquipment(newEquipmentItemID));
        }
        
        IEnumerator ChangeEquipment(short newEquippedItemID)
        {
            while (hand.transform.childCount > 0)
            {
                Destroy(hand.transform.GetChild(0).gameObject);
                yield return null;
            }

            if (newEquippedItemID != Item.NOTHING_ID)
            {
                Item item = DatabaseManager.GetItemByID(newEquippedItemID);

                if (item)
                    Instantiate(item.Prefab, hand.transform);
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            
            if (Input.GetKeyDown(KeyCode.E) && equippedItemID == Item.NOTHING_ID)
                CmdChangeEquippedItem(DatabaseManager.GetRandomItem().Id);

            if (Input.GetKeyDown(KeyCode.G) && equippedItemID != Item.NOTHING_ID)
                CmdDropItem();
        }

        [Command]
        void CmdChangeEquippedItem(short selectedItemId)
        {
            equippedItemID = selectedItemId;
        }

        [Command]
        void CmdDropItem()
        {
            // Instantiate the scene object on the server
            Vector3 pos = hand.transform.position;
            Quaternion rot = hand.transform.rotation;
            GameObject newSceneObject =Instantiate(sceneObjectPrefab, pos, rot);

            // set the RigidBody as non-kinematic on the server only (isKinematic = true in prefab)
            newSceneObject.GetComponent<Rigidbody>().isKinematic = false;

            SceneObject sceneObject = newSceneObject.GetComponent<SceneObject>();

            // set the child object on the server
            sceneObject.SetEquippedItem(equippedItemID);

            // set the SyncVar on the scene object for clients
            sceneObject.equippedItemID = equippedItemID;

            // set the player's SyncVar to nothing so clients will destroy the equipped child item
            equippedItemID = Item.NOTHING_ID;

            // Spawn the scene object on the network for all to see
            NetworkServer.Spawn((GameObject)newSceneObject);
        }
    }
}
