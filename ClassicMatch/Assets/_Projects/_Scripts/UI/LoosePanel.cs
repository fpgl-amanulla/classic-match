using _Projects._Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Projects._Scripts.UI
{
    public class LoosePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI startCountText;

        [SerializeField] private Button retryButton;

        private void Start()
        {
            startCountText.text = ResourceDataManager.Instance.GetStartCount() + "";
            retryButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
    }
}