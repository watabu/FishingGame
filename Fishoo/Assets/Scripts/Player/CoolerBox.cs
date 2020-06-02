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
        [SerializeField, ReadOnly]
        List<Fish.Behavior.CommonFish> m_caughtFishList = new List<Fish.Behavior.CommonFish>();
        public List<Fish.Behavior.CommonFish> GetFishList { get { return m_caughtFishList; } }

        // Start is called before the first frame update
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        public void Add(Fish.Behavior.CommonFish fish)
        {
            m_caughtFishList.Add(fish);
        }
        

    }
}
