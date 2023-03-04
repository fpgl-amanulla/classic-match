using System.Collections;
using _Projects._Scripts.Enum;
using _Projects._Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Projects.scripts
{
    public class TileItem : MonoBehaviour
    {
        public TileItemType type;
        public Image icon;
        [HideInInspector] public bool isActive;
        [HideInInspector] public Button btnItem;
        public UnityAction<TileItem> OnItemClicked;

        private bool _isGameOver = false;

        private void Start()
        {
            btnItem = GetComponent<Button>();
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

            SlotData slotData = SlotsManager.Instance.FindItemSlot((int) type) ??
                                SlotsManager.Instance.GetEmptySlot();
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

            StartCoroutine(CheckForGameOver(0));
        }

        private IEnumerator CheckForGameOver(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            if (SlotsManager.Instance.GetEmptySlot() == null)
            {
                UIManager.Instance.LoadLoosePanel();
            }
            else if (ItemStackManager.Instance.IsGameOver() && !_isGameOver) // Win.......
            {
                _isGameOver = true;
                LevelManager.Instance.LevelComplete();
            }
        }
    }
}