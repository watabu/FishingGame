using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.UI;

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
        const float countDownSpan = 1f;
        public enum Phase{
            nomarl,
            phase1,//カウントダウンの表示
            phase2//音による警告
        }
        Phase phase;
        /// <summary>
        /// 現在の時刻
        /// </summary>
        [SerializeField, ReadOnly] int m_currentTime;
        [SerializeField] bool Skip;
        private int countDownTime1 = 60;
        private int countDownTime2 = 20;
        public int CurrentTime
        {
            get { return m_currentTime; }
            private set
            {
                m_currentTime = Mathf.Clamp(value, startTime, endTime);
                OnTimeChanged.Invoke(m_currentTime);
                if(phase == Phase.nomarl&& m_currentTime +countDownTime1 > endTime)
                {
                    phase = Phase.phase1;
                    AddOnTimeChanged((time) => { countDownText.text = "残" + GetTimeString(endTime- m_currentTime); });
                    Appear();
                }
                if(phase == Phase.phase1 && m_currentTime +countDownTime2 > endTime)
                {
                    phase = Phase.phase2;
                    Debug.Log("phase2");
                    InvokeRepeating("CountDown", 0, countDownSpan);
                }
                if (phase == Phase.phase2 && m_currentTime == endTime)
                {
                    OnTimeFinished.Invoke(m_currentTime);
                }
            }
        }

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private AudioSource countDownAudio;
        [SerializeField] private TimeHolderEvent OnTimeChanged = new TimeHolderEvent();
        [SerializeField] private TimeHolderEvent OnTimeFinished = new TimeHolderEvent();

        public void AddOnTimeChanged(UnityAction<int> func) { OnTimeChanged.AddListener(func); }
        public void RemoveOnTimeChanged(UnityAction<int> func) { OnTimeChanged.RemoveListener(func); }

        public void AddOnTimeFinished(UnityAction<int> func) { OnTimeFinished.AddListener(func); }

        float m_time = 0f;

        private void Awake()
        {
            endTime = 999999;
            phase = Phase.nomarl;
            var color = countDownText.GetComponentInParent<Image>().color;
            color.a = 0;
            countDownText.GetComponentInParent<Image>().color = color;
            var textColor = countDownText.color;
            textColor.a = color.a;
            countDownText.color = textColor;

        }
        // Start is called before the first frame update
        void Start()
        {
            if (Skip) timeSpan = 0.01f;
            AddOnTimeChanged((time) => { timeText.text = GetTimeString(); });
            CurrentTime = startTime;
            timeText.text = GetTimeString();
        }

        /// <summary>
        /// 時間の進める
        /// </summary>
        public void StartTimer()
        {
            InvokeRepeating("TimeChange", 0f, timeSpan);
        }

        void TimeChange() { AddTime(timeDelta); }

        /// <summary>
        /// カウントダウンの音を再生するなど一定時間毎に処理すること
        /// </summary>
        void CountDown()
        {
            countDownAudio.Play();
        }

        async void Appear()
        {
            var color = countDownText.GetComponentInParent<Image>().color;
            color.a = 0;
            while( color.a < 0.5)
            {
                if (countDownText == null) return;
                color.a += 0.05f;
                countDownText.GetComponentInParent<Image>().color = color;
                var textColor = countDownText.color;
                textColor.a = color.a*2;
                countDownText.color = textColor;
                await Task.Delay(20);
            }

        }

        public void AddTime(int t) { CurrentTime += t; }
        public void SubTime(int t) { CurrentTime -= t; }

        public string GetTimeString()
        {
            return string.Format("{0:00}:{1:00}", CurrentTime / 60, CurrentTime % 60);
        }
        public float NormalizedTime { get { return (float)(CurrentTime - startTime) / (float)(endTime - startTime); } }

        public string GetTimeString(int currentTime)
        {
            return string.Format("{0:00}:{1:00}", currentTime / 60, currentTime % 60);
        }
    }
}