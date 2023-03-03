using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Projects.scripts
{
    public class ItemSelector : MonoBehaviour
    {
        public int type;
        public Button btnItem;
        public bool isActive;

        public UnityAction<ItemSelector> OnItemClicked;

        private void Start()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 originPos = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 800);

            float duration = Random.Range(.35f, .75f);
            rectTransform.DOLocalMove(originPos, duration).OnComplete(() =>
            {
                isActive = true;
                btnItem.onClick.AddListener(ItemClickCallBack);
            });
        }

        private void ItemClickCallBack()
        {
            if (!isActive) return;
            SlotData slotData = SlotsManager.Instance.FindItemSlot(type) ?? SlotsManager.Instance.GetEmptySlot();
            if (slotData != null)
            {
                SlotsManager.Instance.UpdateSlot(slotData.index, this.gameObject);
                isActive = false;
                Destroy(btnItem.GetComponent<Button>());
                OnItemClicked?.Invoke(this);

                btnItem.GetComponent<RectTransform>()
                    .DOMove(slotData.rectTransform.position, .4f).OnComplete(delegate
                    {
                        SlotsManager.Instance.MatchCheck();

                        if (SlotsManager.Instance.IsItemsRemainingCheck()) return;
                        
                        //loose no more items game end
                        if (SlotsManager.Instance.GetEmptySlot() == null)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        }
                        else // Win.......
                        {
                            LevelManager.Instance.LevelComplete();
                        }
                    });
            }
            else //game end no slot
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}