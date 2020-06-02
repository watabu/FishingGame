using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 釣った魚を格納する
    /// </summary>
    public class CoolerBox : MonoBehaviour
    {
        [SerializeField]
        List<Fish.FishInfo> m_caughtFishList = new List<Fish.FishInfo>();

        [Tooltip("デバッグ用")]public SaveData saveData;
        public List<Fish.FishInfo> GetFishList { get { return m_caughtFishList; } }

        // Start is called before the first frame update
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            saveData.DebugAddFishes();
        }
        public void Add(Fish.Behavior.CommonFish fish)
        {
            m_caughtFishList.Add(fish.fishInfo);
        }
        

    }
}
