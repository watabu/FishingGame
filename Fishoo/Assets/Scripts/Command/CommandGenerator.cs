using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CommandGenerator : SingletonMonoBehaviour<CommandGenerator>
{
    [System.Serializable]
    public class CommandData
    {
        public KeyCode key;
        public GameObject prefab;
    }
    [Header("Referances")]
    [SerializeField] private Transform commandParent;
    [SerializeField] private Transform commandEffectParent;
    [Header("Properties")]
    [SerializeField] private GameObject commandContainerPrefab;

    [SerializeField] private List<CommandData> commandsData=new List<CommandData>();
    [SerializeField] private List<AudioClip> commandsSE=new List<AudioClip>();
    [SerializeField] private AudioSource audio;
    [SerializeField] private Image comboImage;
    [SerializeField] private Image comboImage2;
    [SerializeField] private List<Sprite> comboSpriteList = new List<Sprite>();

    [ReadOnly]public int MaxCombo = 8;
    [ReadOnly]public int comboCount = 0;
    /// <summary>
    /// コンボが最大まで達したか
    /// </summary>
    bool isFever = false;
    private Animator comboAnimator;
    private void Start()
    {
        SetComboImageAlpha(0);
        comboAnimator = comboImage.GetComponent<Animator>();
    }
    /// <summary>
    /// targetFishのFishMoveDataがもつコマンドのリストから一つ選んで生成する
    /// </summary>
    public void Generate()
    {
        Fish.FishMoveData fishMoveData = FishingGame.FishingGameMgr.Instance.TargetFish.fishMoveData;
        
        var obj = Instantiate(commandContainerPrefab, commandParent).GetComponent<CommandContainerScript>();

        //        List<KeyCode> com = new List<KeyCode>() {KeyCode.A,KeyCode.B,KeyCode.UpArrow,KeyCode.DownArrow };
        List<KeyCode> com = fishMoveData.GetCommands();
        obj.SetCommand(com,commandEffectParent);
        if (isFever)
            FinishCombo();
    }

    public GameObject GetCommandPrefab(KeyCode key)
    {
        foreach (var i in commandsData)
        {
            if (i.key == key) return i.prefab;
        }
        return null;
    }

    

    public void OnCommandKilled()
    {
        comboImage.sprite = comboSpriteList[comboCount];
        comboAnimator.SetTrigger("ComboChanged");
        
        audio.clip = commandsSE[comboCount];
        audio.Play();
         comboCount++;
        if (comboCount >= commandsSE.Count)
        {
            isFever = true;
            comboCount--;
        }
        SetComboImageAlpha(1);
    }

    public void PlayeMistakeSound()
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        audio.PlayOneShot(commandsSE[0]);
        audio.PlayOneShot(commandsSE[2]);
    }

    /// <summary>
    /// ミス等でコンボをリセットする
    /// </summary>
    public void ResetComboCount()
    {
        comboCount = 0;
        comboImage.sprite = null;
        isFever = false;
        SetComboImageAlpha(0);
    }

    /// <summary>
    /// コンボ数の表記を0.3秒残すコンボリセット
    /// </summary>
    public async void FinishCombo()
    {
        comboCount = 0;
        isFever = false;
        await Task.Delay(300);
        SetComboImageAlpha(0);
        comboImage.sprite = null;
    }

    void SetComboImageAlpha(float a)
    {
        var color = comboImage.color;
        color.a = a;
        comboImage.color = color;
        comboImage2.color = color;
    }

}
