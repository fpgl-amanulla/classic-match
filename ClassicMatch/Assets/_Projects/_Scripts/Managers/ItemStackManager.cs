using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Projects.scripts
{
    [Serializable]
    public class ItemStack
    {
        public List<ItemSelector> ItemSelectorList;
    }

    public class ItemStackManager : MonoBehaviour
    {
        public List<ItemStack> allItemStack = new();
        private void Start()
        {
            Init();
            ActiveItem(0);
        }
        private void Update()
        {
            if (gameObject.transform.childCount <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            foreach (List<ItemSelector> itemList in allItemStack.Select(itemStack => itemStack.ItemSelectorList))
            {
                foreach (ItemSelector item in itemList)
                {
                    item.OnItemClicked += OnItemClicked;
                    item.btnItem.interactable = false;
                }
            }
        }

        private void OnItemClicked(ItemSelector itemSelector)
        {
            int index = GetIndex(itemSelector);
            if (!ShouldActiveNext(index)) return;
            ActiveItem(++index);
        }

        private void ActiveItem(int index)
        {
            if (index > allItemStack.Count - 1)
                return;
            List<ItemSelector> itemSelectorList = allItemStack[index].ItemSelectorList;
            foreach (ItemSelector Item in itemSelectorList)
            {
                Item.btnItem.interactable = true;
            }
        }

        private int GetIndex(ItemSelector itemSelector)
        {
            for (int i = 0; i < allItemStack.Count; i++)
            {
                if (allItemStack[i].ItemSelectorList.Contains(itemSelector))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool ShouldActiveNext(int index)
        {
            List<ItemSelector> itemSelectorList = allItemStack[index].ItemSelectorList;
            return itemSelectorList.All(item => !item.isActive);
        }
    }
}