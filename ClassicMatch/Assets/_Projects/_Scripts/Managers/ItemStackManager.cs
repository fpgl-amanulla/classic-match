using System;
using System.Collections.Generic;
using System.Linq;
using _Projects._Scripts.Core;
using _Projects._Scripts.ScriptableObject;
using Random = System.Random;

namespace _Projects.scripts
{
    [Serializable]
    public class ItemStack
    {
        public List<TileItem> TileItemList;
    }

    public class ItemStackManager : Singleton<ItemStackManager>
    {
        public List<ItemStack> allItemStack = new();

        //public List<TileItem> TileItemPrefabList = new List<TileItem>();

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
            InitLevel();

            foreach (List<TileItem> itemList in allItemStack.Select(itemStack => itemStack.TileItemList))
            {
                foreach (TileItem item in itemList)
                {
                    item.OnItemClicked += OnItemClicked;
                    item.btnItem.interactable = false;
                }
            }
        }

        private void InitLevel()
        {
            List<TileItemSO> tileItemSoList = LevelManager.Instance.TileItemSoList;
            List<TileItem> allItemSelector = new List<TileItem>();
            for (int i = 0; i < allItemStack.Count; i++)
            {
                for (int j = 0; j < allItemStack[i].TileItemList.Count; j++)
                {
                    allItemSelector.Add(allItemStack[i].TileItemList[j]);
                }
            }

            for (int j = 0; j < allItemSelector.Count / 3; j++)
            {
                List<TileItem> itemSelectors = GetRandomThree(ref allItemSelector);
                TileItemSO tileItemSO = tileItemSoList[UnityEngine.Random.Range(0, tileItemSoList.Count)];
                for (int i = 0; i < itemSelectors.Count; i++)
                {
                    itemSelectors[i].type = tileItemSO.type;
                    itemSelectors[i].icon.sprite = tileItemSO.iconSprite;
                }
            }
        }

        private List<TileItem> GetRandomThree(ref List<TileItem> itemSelectors)
        {
            Random rand = new Random();
            List<TileItem> selectors = itemSelectors.OrderBy(x => rand.Next()).Take(3).ToList();
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
                for (int j = 0; j < allItemStack[i].TileItemList.Count; j++)
                {
                    if (allItemStack[i].TileItemList[j].btnItem != null) return false;
                }
            }

            return true;
        }

        private void OnItemClicked(TileItem tileItem)
        {
            int index = GetIndex(tileItem);
            if (!ShouldActiveNext(index)) return;
            ActiveItem(++index);
        }

        private void ActiveItem(int index)
        {
            if (index > allItemStack.Count - 1)
                return;
            List<TileItem> itemSelectorList = allItemStack[index].TileItemList;
            foreach (TileItem Item in itemSelectorList)
            {
                Item.btnItem.interactable = true;
            }
        }

        private int GetIndex(TileItem tileItem)
        {
            for (int i = 0; i < allItemStack.Count; i++)
            {
                if (allItemStack[i].TileItemList.Contains(tileItem))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool ShouldActiveNext(int index)
        {
            List<TileItem> itemSelectorList = allItemStack[index].TileItemList;
            return itemSelectorList.All(item => !item.isActive);
        }
    }
}