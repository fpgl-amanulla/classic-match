using System;
using _Projects._Scripts.Managers;
using _Projects.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Projects.scripts
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Button btnPlay;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject loosePanel;

        [SerializeField] private TextMeshProUGUI startCountText;

        [SerializeField] private GameObject startPrefab;
        [SerializeField] private RectTransform starFlyDesTr;

        private void Awake()
        {
            btnPlay.image.rectTransform.DOScale(Vector3.one * .9f, .8f).SetLoops(-1, LoopType.Yoyo);

            btnPlay.onClick.AddListener(() =>
            {
                gamePanel.SetActive(true);
                startPanel.SetActive(false);
                LevelManager.Instance.LoadLvlPrefab();
            });

            UpdateStartCount();
            ResourceDataManager.Instance.onStartCountUpdate += UpdateStartCount;
        }

        private void OnDestroy()
        {
            ResourceDataManager.Instance.onStartCountUpdate -= UpdateStartCount;
        }

        private void UpdateStartCount() => startCountText.text = ResourceDataManager.Instance.GetStartCount() + "";

        public void LoadWinPanel()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
        }

        public void LoadLoosePanel()
        {
            gamePanel.SetActive(false);
            loosePanel.SetActive(true);
        }

        public void ShowStarFly(RectTransform slotReRectTransform)
        {
            GameObject star = Instantiate(startPrefab, slotReRectTransform.position, Quaternion.identity,
                this.transform);
            RectTransform rectTransform = star.GetComponent<RectTransform>();
            rectTransform.DOMove(starFlyDesTr.position, 1.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                ResourceDataManager.Instance.AddStarCount(1);
                Destroy(star);
            });
        }
    }
}