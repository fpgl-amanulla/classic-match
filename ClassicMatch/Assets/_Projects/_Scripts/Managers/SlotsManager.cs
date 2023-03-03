using System.Collections;
using System.Collections.Generic;
using _Projects.Scripts;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;

namespace _Projects.scripts
{
    public class SlotsManager : Singleton<SlotsManager>
    {
        public List<RectTransform> slotReRectTransformList = new();
        [HideInInspector] public List<SlotData> allSlot = new();
        public GameObject itemsHolder;

        private int CurrentSlot { get; set; }

        public void Start()
        {
            CurrentSlot = 0;
            InitSlotData();
        }

        private void InitSlotData()
        {
            for (int i = 0; i < slotReRectTransformList.Count; i++)
            {
                allSlot.Add(new SlotData(i, slotReRectTransformList[i], true, null));
            }
        }

        public SlotData GetEmptySlot()
        {
            for (int i = 0; i < allSlot.Count; i++)
            {
                if (!allSlot[i].isEmpty) continue;
                CurrentSlot = i;
                return allSlot[i];
            }

            //no slot game end return null
            return null;
        }

        public SlotData FindItemSlot(int type)
        {
            int maxPos = -1;
            for (int i = 0; i < allSlot.Count; i++)
            {
                if (allSlot[i].isEmpty) continue;
                if (allSlot[i].item.transform.GetComponent<ItemSelector>().type == type)
                {
                    maxPos = i + 1;
                }
            }

            if (maxPos > allSlot.Count - 1) return null;
            return maxPos == -1 ? null : allSlot[maxPos];
        }

        public void MatchCheck()
        {
            int matchCounter = 0;
            int typePrev = 0;
            for (int i = 0; i < allSlot.Count; i++)
            {
                if (allSlot[i].isEmpty) continue;

                int typeCurrent = allSlot[i].item.transform.GetComponent<ItemSelector>().type;
                if (typePrev != 0 && typePrev == typeCurrent)
                {
                    matchCounter++;
                }
                else
                {
                    typePrev = typeCurrent;
                    matchCounter = 0;
                }

                //match
                if (matchCounter < 2) continue;
                for (int y = i; y > i - 3; y--)
                {
                    allSlot[y].isEmpty = true;
                    GameObject item = allSlot[y].item.gameObject;
                    UIParticle particleEffect =
                        slotReRectTransformList[y].gameObject.GetComponentInChildren<UIParticle>();
                    particleEffect.Play();
                    item.transform.DOScale(0, .3f).OnComplete(delegate
                    {
                        Destroy(item.gameObject);
                        //allSlot[y].item = null;
                    });
                }

                StartCoroutine(LeftShiftItems(i, 3, .2f));
                CurrentSlot -= 3;
                break;
            }
        }

        private IEnumerator LeftShiftItems(int startIndex, int shiftAmm, float delayTime = 0)
        {
            yield return new WaitForSeconds(delayTime);
            for (int i = startIndex; i < allSlot.Count; i++)
            {
                if (allSlot[i].isEmpty) continue;
                GameObject item = allSlot[i].item;
                allSlot[i - shiftAmm].item = item;
                allSlot[i - shiftAmm].isEmpty = false;
                allSlot[i].item = null;
                allSlot[i].isEmpty = true;
                item.GetComponent<RectTransform>()
                    .DOMove(allSlot[i - shiftAmm].rectTransform.position, .4f);
            }
        }

        private void RightShiftItems(int startIndex, int shiftAmm)
        {
            for (int i = allSlot.Count - 1; i >= startIndex; i--)
            {
                if (allSlot[i].isEmpty) continue;
                GameObject item = allSlot[i].item;
                allSlot[i + shiftAmm].item = item;
                allSlot[i + shiftAmm].isEmpty = false;
                allSlot[i].item = null;
                allSlot[i].isEmpty = true;
                item.GetComponent<RectTransform>()
                    .DOMove(allSlot[i + shiftAmm].rectTransform.position, .4f);
            }
        }

        public bool IsItemsRemainingCheck()
        {
            return itemsHolder.transform.childCount >= 1;
        }

        public void UpdateSlot(int slotDataIndex, GameObject item)
        {
            RightShiftItems(slotDataIndex, 1);
            allSlot[slotDataIndex].isEmpty = false;
            allSlot[slotDataIndex].item = item;
            item.transform.parent = this.transform;
            CurrentSlot += 1;
        }
    }
}