using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// ボタンを押すと特定のシーンに移動できるようにするクラス
/// </summary>
public class StageSelectMgr : MonoBehaviour
{
    [System.Serializable]
    public class PlaceAndData
    {
        public string sceneName;
        public PlaceData place;
        public StageSelectButton button;
    }

    [SerializeField] private PlaceAndData[] scenesData;
    [Header("References")]
    [SerializeField] private Transform buttonsParent;


    // Start is called before the first frame update
    void Start()
    {
        foreach(var i in scenesData)
        {
            i.button.Initialize(i.place.description,()=> { });
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Jump(int ID)
    {
        SceneManager.LoadScene(scenesData[ID].sceneName);
    }
}
