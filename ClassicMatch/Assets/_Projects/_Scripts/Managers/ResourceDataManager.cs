using UnityEngine.Events;

namespace _Projects._Scripts.Managers
{
    public class ResourceDataManager
    {
        private static ResourceDataManager instance;
        public static ResourceDataManager Instance => instance ??= new ResourceDataManager();

        public UnityAction onStartCountUpdate;

        private int startCount;

        public int GetStartCount() => startCount;

        public void AddStarCount(int amount)
        {
            startCount += amount;
            onStartCountUpdate?.Invoke();
        }

        public void ResetStarCount() => startCount = 0;
    }
}