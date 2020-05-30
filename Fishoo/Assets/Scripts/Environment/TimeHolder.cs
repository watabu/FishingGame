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

        [Tooltip("時刻変化")]
        public float timeSpan;
        [Tooltip("時刻変化量")]
        public int timeDelta;

        /// <summary>
        /// 現在の時刻
        /// </summary>
        [SerializeField, ReadOnly]int m_currentTime;
        public int CurrentTime
        {
            get { return m_currentTime; }
            private set
            {
                m_currentTime = Mathf.Clamp(value, startTime, endTime);
                OnTimeChanged.Invoke(m_currentTime);
                if (m_currentTime == endTime)
                {
                    OnTimeFinished.Invoke(m_currentTime);
                }
            }
        }

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TimeHolderEvent OnTimeChanged = new TimeHolderEvent();
        [SerializeField] private TimeHolderEvent OnTimeFinished = new TimeHolderEvent();

        public void AddOnTimeChanged(UnityAction<int> func) { OnTimeChanged.AddListener(func); }
        public void AddOnTimeFinished(UnityAction<int> func) { OnTimeFinished.AddListener(func); }

        float m_time=0f;

        // Start is called before the first frame update
        void Start()
        {
            AddOnTimeChanged((time) => { timeText.text = GetTimeString(); });
            CurrentTime = startTime;
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