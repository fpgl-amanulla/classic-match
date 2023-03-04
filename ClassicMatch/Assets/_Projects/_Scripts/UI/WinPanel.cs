using System;
using _Projects._Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Projects._Scripts.UI
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI startCountText;
        [SerializeField] private Button nextButton;

        private void Start()
        {
            startCountText.text = ResourceDataManager.Instance.GetStartCount() + "";
            nextButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
        }
    }
}