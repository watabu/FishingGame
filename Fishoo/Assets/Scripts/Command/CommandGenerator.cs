using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGenerator : MonoBehaviour
{
    [System.Serializable]
    public class CommandData
    {
        public KeyCode key;
        public GameObject prefab;
    }
    [Header("Referances")]
    [SerializeField] private Transform commandParent;
    [Header("Properties")]
    [SerializeField] private GameObject commandPrefab;

    [SerializeField] private List<CommandData> commandsData=new List<CommandData>();

    [Tooltip("コマンドが打ち終わったときにダメージを与える魚")]
    public Fish.FishScripts.CommonFish targetFish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        var obj = Instantiate(commandPrefab, commandParent).GetComponent<CommandScript>();
        List<KeyCode> com = new List<KeyCode>() {KeyCode.A,KeyCode.B,KeyCode.UpArrow,KeyCode.DownArrow };
        obj.SetCommand(com);
        
    }

    public GameObject GetCommandPrefab(KeyCode key)
    {
        foreach (var i in commandsData)
        {
            if (i.key == key) return i.prefab;
        }
        return null;
    }

}
