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
    [SerializeField] private Transform commandEffectParent;
    [Header("Properties")]
    [SerializeField] private GameObject commandContainerPrefab;

    [SerializeField] private List<CommandData> commandsData=new List<CommandData>();

    [Tooltip("釣りゲームMgr")] public FishingGame.FishingGameMgr fishingGameMgr;
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

    /// <summary>
    /// targetFishのFishMoveDataがもつコマンドのリストから一つ選んで生成する
    /// </summary>
    public void Generate()
    {
        Fish.FishMoveData fishMoveData = targetFish.fishMoveData;
        
        var obj = Instantiate(commandContainerPrefab, commandParent).GetComponent<CommandContainerScript>();
        //        List<KeyCode> com = new List<KeyCode>() {KeyCode.A,KeyCode.B,KeyCode.UpArrow,KeyCode.DownArrow };
        List<KeyCode> com = fishMoveData.GetCommands();

        obj.SetCommand(com,commandEffectParent);
        
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
