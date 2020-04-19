using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Environment
{

    //朝・夜などの時間を管理する
    //天候による効果(明るさなど)も管理する？
    public class TimeHolder : SingletonMonoBehaviour<TimeHolder>
    {
        [System.Serializable]
        public class TimeHolderEvent : UnityEvent<int> { }

        [Tooltip("開始時刻")]
        public int startTime;
        [Tooltip("終了時刻")]
        public int endTime;

        /// <summary>
        /// 現在の時刻
        /// </summary>
        int m_currentTime;
        public int CurrentTime
        {
            get { return m_currentTime; }
            private set
            {
                m_currentTime = Mathf.Clamp(value, startTime, endTime);
                OnTimeChanged.Invoke(m_currentTime);
            }
        }

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TimeHolderEvent OnTimeChanged;

        public void AddOnTimeChanged(UnityAction<int> func) { OnTimeChanged.AddListener(func); }

        protected override void Awake()
        {
            base.Awake();
            if (OnTimeChanged == null) OnTimeChanged = new TimeHolderEvent();
        }

        // Start is called before the first frame update
        void Start()
        {
            AddOnTimeChanged((time) => { timeText.text = GetTimeString(); });
            CurrentTime = startTime;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddTime(int t)
        {
            CurrentTime += t;
        }
        public void SubTime(int t)
        {
            CurrentTime -= t;
        }

        public string GetTimeString()
        {
            return $"{CurrentTime / 60}:{CurrentTime % 60}";
        }
        public float NormalizedTime { get { return (float)(CurrentTime - startTime) / (float)(endTime - startTime); } }
    }
}