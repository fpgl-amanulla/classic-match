using System.Collections;
using System.Collections.Generic;
using _Projects._Scripts.Core;
using _Projects._Scripts.ScriptableObject;
using Coffee.UIExtensions;
using UnityEngine;

namespace _Projects.scripts
{
    [System.Serializable]
    public class LevelManager : Singleton<LevelManager>
    {
        public List<GameObject> LevelPrefabs = new();
        public int levelNo;
        public GameObject levelParent;
        public UIParticle victoryEffect;

        public List<TileItemSO> TileItemSoList = new List<TileItemSO>();


        public void LoadLvlPrefab()
        {
            levelNo = PlayerPrefs.GetInt("LevelCountKey", 0);
            GameObject lvlPrefab = Instantiate(LevelPrefabs[levelNo], levelParent.transform.position,
                Quaternion.identity, levelParent.transform);
            SlotsManager.Instance.itemsHolder = lvlPrefab;
        }

        public void LevelComplete()
        {
            if (levelNo + 1 >= LevelPrefabs.Count)
                PlayerPrefs.SetInt("LevelCountKey", 0);
            else
                PlayerPrefs.SetInt("LevelCountKey", levelNo + 1);

            levelNo++;

            StartCoroutine(LoadWinPanel(1.0f));
        }

        public IEnumerator LoadWinPanel(float delayTime)
        {
            yield return new WaitForSeconds(1.25f);
            victoryEffect.Play();
            yield return new WaitForSeconds(delayTime);
            UIManager.Instance.LoadWinPanel();
        }
    }
}