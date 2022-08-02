using Mirror;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public enum EquippedItem : byte
    {
        Nothing,
        Item
    }
    
    public class EquipHand : NetworkBehaviour
    {
        [SerializeField] private GameObject sceneObjectPrefab;
        
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject itemPrefab;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public EquippedItem equippedItem;

        void OnChangeEquipment(EquippedItem oldEquipmentItem, EquippedItem newEquipmentItem)
        {
            Debug.Log(oldEquipmentItem);
            StartCoroutine(ChangeEquipment(newEquipmentItem));
        }
        
        IEnumerator ChangeEquipment(EquippedItem newEquippedItem)
        {
            while (hand.transform.childCount > 0)
            {
                Destroy(hand.transform.GetChild(0).gameObject);
                yield return null;
            }

            switch (newEquippedItem)
            {
                case EquippedItem.Item:
                    Instantiate(itemPrefab, hand.transform);
                    break;
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (Input.GetKeyDown(KeyCode.N) && equippedItem != EquippedItem.Nothing)
                CmdChangeEquippedItem(EquippedItem.Nothing);
            if (Input.GetKeyDown(KeyCode.E) && equippedItem != EquippedItem.Item)
                CmdChangeEquippedItem(EquippedItem.Item);

            if (Input.GetKeyDown(KeyCode.G) && equippedItem != EquippedItem.Nothing)
                CmdDropItem();
        }

        [Command]
        void CmdChangeEquippedItem(EquippedItem selectedItem)
        {
            equippedItem = selectedItem;
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
            sceneObject.SetEquippedItem(equippedItem);

            // set the SyncVar on the scene object for clients
            sceneObject.equippedItem = equippedItem;

            // set the player's SyncVar to nothing so clients will destroy the equipped child item
            equippedItem = EquippedItem.Nothing;

            // Spawn the scene object on the network for all to see
            NetworkServer.Spawn((GameObject)newSceneObject);
        }
    }
}
