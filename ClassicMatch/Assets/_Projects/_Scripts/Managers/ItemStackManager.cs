using System;
using System.Collections.Generic;
using System.Linq;
using _Projects.Scripts;
using UnityEngine;
using Random = System.Random;

namespace _Projects.scripts
{
    [Serializable]
    public class ItemStack
    {
        public List<ItemSelector> ItemSelectorList;
    }

    public class ItemStackManager : Singleton<ItemStackManager>
    {
        public List<ItemStack> allItemStack = new();

        public List<ItemSelector> ItemSelectorList = new List<ItemSelector>();

        private void Start()
        {
            Init();
            ActiveItem(0);
        }

        private void Update()
        {
            if (gameObject.transform.childCount <= 0)
            {
                //Destroy(gameObject);
            }
        }

        private void Init()
        {
            List<ItemSelector> allItemSelector = new List<ItemSelector>();
            for (int i = 0; i < allItemStack.Count; i++)
            {
                for (int j = 0; j < allItemStack[i].ItemSelectorList.Count; j++)
                {
                    allItemSelector.Add(allItemStack[i].ItemSelectorList[j]);
                }
            }

            for (int j = 0; j < allItemSelector.Count / 3; j++)
            {
                List<ItemSelector> itemSelectors = GetRandomThree(ref allItemSelector);
                ItemSelector itemSelector = ItemSelectorList[UnityEngine.Random.Range(0, ItemSelectorList.Count)];
                for (int i = 0; i < itemSelectors.Count; i++)
                {
                    itemSelectors[i].type = itemSelector.type;
                    itemSelectors[i].icon.sprite = itemSelector.icon.sprite;
                }
            }

            foreach (List<ItemSelector> itemList in allItemStack.Select(itemStack => itemStack.ItemSelectorList))
            {
                foreach (ItemSelector item in itemList)
                {
                    item.OnItemClicked += OnItemClicked;
                    item.btnItem.interactable = false;
                }
            }
        }

        private List<ItemSelector> GetRandomThree(ref List<ItemSelector> itemSelectors)
        {
            Random rand = new Random();
            List<ItemSelector> selectors = itemSelectors.OrderBy(x => rand.Next()).Take(3).ToList();
            for (int i = 0; i < selectors.Count; i++)
            {
                itemSelectors.Remove(selectors[i]);
            }

            return selectors;
        }

        public bool IsGameOver()
        {
            for (int i = 0; i < allItemStack.Count; i++)
            {
                for (int j = 0; j < allItemStack[i].ItemSelectorList.Count; j++)
                {
                    if (allItemStack[i].ItemSelectorList[j].btnItem != null) return false;
                }
            }

            return true;
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