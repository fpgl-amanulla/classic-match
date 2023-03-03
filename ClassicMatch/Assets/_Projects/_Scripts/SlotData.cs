using UnityEngine;

namespace _Projects.scripts
{
    [System.Serializable]
    public class SlotData
    {
        public SlotData(int index, RectTransform rectTransform, bool isEmpty, GameObject item)
        {
            this.index = index;
            this.rectTransform = rectTransform;
            this.isEmpty = isEmpty;
            this.item = item;
        }

        public int index;
        public RectTransform rectTransform;
        public bool isEmpty;
        public GameObject item;
    }
}