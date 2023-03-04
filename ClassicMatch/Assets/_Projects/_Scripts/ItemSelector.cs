using System.Collections;
using System.Collections.Generic;
using _Projects._Scripts.Managers;
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
        public Image icon;
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

            SoundManager.Instance.PlaySFX(SoundManager.Instance.clickSFX);

            SlotData slotData = SlotsManager.Instance.FindItemSlot(type) ?? SlotsManager.Instance.GetEmptySlot();
            if (slotData != null)
            {
                SlotsManager.Instance.UpdateSlot(slotData.index, this.gameObject);
                isActive = false;
                Destroy(btnItem.GetComponent<Button>());
                OnItemClicked?.Invoke(this);

                btnItem.GetComponent<RectTransform>().DOSizeDelta(new Vector2(65, 75), .4f);
                btnItem.GetComponent<RectTransform>()
                    .DOMove(slotData.rectTransform.position, .4f).OnComplete(delegate
                    {
                        SlotsManager.Instance.MatchCheck();
                    });
            }
            else //game end no slot
            {
                UIManager.Instance.LoadLoosePanel();
            }

            StartCoroutine(CheckForGameOver(.5f));
        }

        private IEnumerator CheckForGameOver(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            if (SlotsManager.Instance.GetEmptySlot() == null)
            {
                UIManager.Instance.LoadLoosePanel();
            }
            else if (ItemStackManager.Instance.IsGameOver()) // Win.......
            {
                LevelManager.Instance.LevelComplete();
            }
        }
    }
}