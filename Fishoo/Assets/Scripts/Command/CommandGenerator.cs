using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    public GameObject GetCommandPrefab(KeyCode key)
    {
        foreach (var i in commandsData)
        {
            if (i.key == key) return i.prefab;
        }
        return null;
    }

    int comboCount = 0;
    public void OnCommandKilled()
    {
        audio.clip = commandsSE[comboCount];
        audio.Play();
         comboCount++;
        if (comboCount >= commandsSE.Count) comboCount--;
    }
}
