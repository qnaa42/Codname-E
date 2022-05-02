using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InventorySlot : Inventory
    {
        public GameObject storedItem;
        public int itemIdInInventory;

        public void GetItemFromSlot()
        {
            if (storedItem != null)
            {
                storedItem.gameObject.GetComponent<MeshRenderer>().enabled = false;
                storedItem.SetActive(true);
                GameManager.instance.pickupScript.PickUpObject(storedItem);
                storedItem.gameObject.GetComponent<MeshRenderer>().enabled = true;
                storedItem = null;
                _globalInventoryList.RemoveAt(itemIdInInventory);
                UpdateInventory();
            }
        }


    }
}
