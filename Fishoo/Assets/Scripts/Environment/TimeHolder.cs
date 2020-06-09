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
        public int endTime = 999999;

        [Tooltip("時刻変化")]
        public float timeSpan = 0.2f;
        [Tooltip("時刻変化量")]
        public int timeDelta;

        const float defalt_timeSpan = 0.2f;
        /// <summary>
        /// 現在の時刻
        /// </summary>
        [SerializeField, ReadOnly] int m_currentTime;
        [SerializeField] bool Skip;
        public int CurrentTime
        {
            get { return m_currentTime; }
            private set
            {
                m_currentTime = Mathf.Clamp(value, startTime, endTime);
                OnTimeChanged.Invoke(m_currentTime);
                if (m_currentTime == endTime)
                {
                    Debug.Log(m_currentTime);
                    OnTimeFinished.Invoke(m_currentTime);
                }
            }
        }

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TimeHolderEvent OnTimeChanged = new TimeHolderEvent();
        [SerializeField] private TimeHolderEvent OnTimeFinished = new TimeHolderEvent();

        public void AddOnTimeChanged(UnityAction<int> func) { OnTimeChanged.AddListener(func); }
        public void RemoveOnTimeChanged(UnityAction<int> func) { OnTimeChanged.RemoveListener(func); }

        public void AddOnTimeFinished(UnityAction<int> func) { OnTimeFinished.AddListener(func); }

        float m_time = 0f;

        private void Awake()
        {
            endTime = 999999;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (Skip) timeSpan = 0.0005f;
            AddOnTimeChanged((time) => { timeText.text = GetTimeString(); });
            CurrentTime = startTime;
            Debug.Log(startTime);
            InvokeRepeating("TimeChange", 0f, timeSpan);
        }

        void TimeChange() { AddTime(timeDelta); }

        public void AddTime(int t) { CurrentTime += t; }
        public void SubTime(int t) { CurrentTime -= t; }

        public string GetTimeString()
        {
            return string.Format("{0:00}:{1:00}", CurrentTime / 60, CurrentTime % 60);
        }
        public float NormalizedTime { get { return (float)(CurrentTime - startTime) / (float)(endTime - startTime); } }
    }
}