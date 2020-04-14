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
    List<KeyCode> m_commands = new List<KeyCode>();
    List<CommandScript> m_commandObjects = new List<CommandScript>();

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
        if (Input.GetKeyDown(m_commands[0])){//先頭の文字と同じ入力があったとき
            OnTrueKeyDown();
        }
        else if(Input.anyKeyDown)//間違えた文字を打ったとき
        {
            OnFalseKeyDown();
        }
    }

    public void SetCommand(List<KeyCode> commands,Transform effectParent)
    {
        m_commands.Clear();
        m_commands = commands;
        foreach (var i in commands)
        {
            m_commandObjects.Add(Instantiate(Generator.GetCommandPrefab(i), commandParent).GetComponent<CommandScript>());
            m_commandObjects[m_commandObjects.Count - 1].effectParent = effectParent;
        }
    }
    void OnTrueKeyDown()
    {
        if (FinishesCommand) return;
        m_commands.RemoveAt(0);
        m_commandObjects[0].Kill();
        m_commandObjects[0].transform.SetParent(transform.parent);
        m_commandObjects.RemoveAt(0);
        //text.text= text.text.Remove(0, 1);
        if (m_commands.Count == 0)
        {
            FinishesCommand = true;
            OnFinish();
        }
    }
     void OnFalseKeyDown()
    {

    }

    void OnFinish()
    {
        if (Generator.targetFish == null) return;
        float damage = 3;
        Generator.targetFish.Damaged(damage);
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
