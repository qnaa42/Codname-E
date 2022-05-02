using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Inventory : MonoBehaviour
    {
        
        public static List<GameObject> _globalInventoryList = new List<GameObject>();

        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame


        public void UpdateInventory()
        {
            var _inventoryObject = GameManager.instance.uiManager.inventory;
            var inventorySizeNow = _inventoryObject.transform.childCount;
            var inventorySizeNew = _globalInventoryList.Count;
                for (int i = 0; i < 18; i++)
                {
                    if (i < inventorySizeNew)
                    {
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = _globalInventoryList[i].gameObject.GetComponent<PickableObject>().icon;
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<InventorySlot>().storedItem = _globalInventoryList[i].gameObject;
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<InventorySlot>().itemIdInInventory = i;
                    }
                    else
                    {
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<InventorySlot>().storedItem = null;
                        _inventoryObject.transform.GetChild(i).GetChild(0).GetComponent<InventorySlot>().itemIdInInventory = 0;
                    }
                }

        }
        public void AddToInventory(GameObject anObject)
        {
            if (anObject.CompareTag("Pickable"))
            {
                GameManager.instance.uiManager.inventoryInfo.SetActive(false);
            }
            _globalInventoryList.Add(anObject);
            UpdateInventory();
            anObject.transform.parent = GameManager.instance.cameraReferencePoint.transform;
            anObject.transform.gameObject.SetActive(false);
        }

    }
}

