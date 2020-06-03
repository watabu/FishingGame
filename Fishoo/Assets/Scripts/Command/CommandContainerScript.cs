using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 魚を釣るときに打つコマンドの処理
/// </summary>
public class CommandContainerScript : MonoBehaviour
{

    // [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform commandParent;
    int commandsLength;
    [SerializeField]List<KeyCode> m_commands = new List<KeyCode>();
    List<CommandScript> m_commandObjects = new List<CommandScript>();
    int command_index;
    private Transform commandEffectParent;
    bool m_FinishesCommand=false;
    /// <summary>
    /// コマンドをすべて打ち終わったか
    /// </summary>
    public bool FinishesCommand { get { return m_FinishesCommand; } private set { m_FinishesCommand = value; } }

    static CommandGenerator m_generator;
    static CommandGenerator Generator
    {
        get
        {
            if (m_generator == null)
                m_generator = FindObjectOfType<CommandGenerator>();
            return m_generator;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FinishesCommand) return;
        if (Player.InputSystem.GetKeyDown(m_commands[command_index])){//同じ入力があったとき
            OnTrueKeyDown();
        }
        else if(Player.InputSystem.FalseKeyDown(m_commands[command_index]))//正解と反対のキーを押したとき
        {
            OnFalseKeyDown();
        }
    }

    public void SetCommand(List<KeyCode> commands,Transform effectParent)
    {
        command_index = 0;
        m_commands.Clear();
        m_commands = commands;
        commandsLength = commands.Count;
        foreach (var i in commands)
        {
            m_commandObjects.Add(Instantiate(Generator.GetCommandPrefab(i), commandParent).GetComponent<CommandScript>());
            m_commandObjects[m_commandObjects.Count - 1].effectParent = effectParent;
        }
        FishingGame.FishingGameMgr.Instance.TargetFish.fishMove.ReceiveNextKey(m_commands[0]);
    }
    void OnTrueKeyDown()
    {
        if (FinishesCommand) return;

        Vector3 EffectPoint = FishingGame.FishingGameMgr.Instance.TargetFish.transform.position;
        m_commandObjects[command_index].Kill(m_generator.comboCount,m_generator.MaxCombo,EffectPoint );
        CommandGenerator.Instance.OnCommandKilled();
        command_index++;
        //text.text= text.text.Remove(0, 1);
        
        //今釣っている魚にダメージを与える
        if(command_index == m_commands.Count)
//        if (m_commands.Count == 0)
        {
            FinishesCommand = true;
            OnFinish();
            return;
        }
        if (FishingGame.FishingGameMgr.Instance.TargetFish != null)
        {
            float damage = 0.25f;
//            Debug.Log(damage);
            FishingGame.FishingGameMgr.Instance.TargetFish.Damaged(damage);
            FishingGame.FishingGameMgr.Instance.TargetFish.fishMove.ReceiveNextKey(m_commands[command_index]);
        }
    }
     void OnFalseKeyDown()
    {
        m_generator.PlayeMistakeSound();
        m_generator.ResetComboCount();
    }

    void OnFinish()
    {
        //今釣っている魚にダメージを与える
        if (FishingGame.FishingGameMgr.Instance.TargetFish != null)
        {
            float damage;
            Debug.Log(m_generator.comboCount);
            damage = m_generator.comboCount * m_generator.comboCount /10;
            Debug.Log(damage);
            FishingGame.FishingGameMgr.Instance.TargetFish.Damaged(damage);
        }
        //次のコマンドを生成する許可をする
        FishingGame.FishingGameMgr.Instance.canAttack = true;
            
        Destroy(gameObject);
    }
}


/*
 
    public void SetCommand(List<KeyCode> commands)
    {
        m_commands.Clear();
      //  text.text = "";
        m_commands = commands;
        foreach (var i in commands)
        {
            string c = "";
            switch (i)
            {
                case KeyCode.A:
                    c = "A";
                    break;
                case KeyCode.B:
                    c = "B";
                    break;
                case KeyCode.X:
                    c = "X";
                    break;
                case KeyCode.Y:
                    c = "Y";
                    break;
                case KeyCode.UpArrow:
                    c = "↑";
                    break;
                case KeyCode.DownArrow:
                    c = "↓";
                    break;
                case KeyCode.LeftArrow:
                    c = "←";
                    break;
                case KeyCode.RightArrow:
                    c = "→";
                    break;
                default:
                    break;
            }
         //   text.text += c;
        }
    }*/
