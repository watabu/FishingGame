using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キー入力をGameMgrで一元管理するためのインターフェース
/// ポーズ中にキー入力を受け付けないとか楽になる
/// </summary>
public interface IInputUpdatable 
{
    void InputUpdate();
}
