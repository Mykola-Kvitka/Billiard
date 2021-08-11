using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Перезапус сцены когда шаров не осталось на карте
/// </summary>
public class PlayAgain : MonoBehaviour
{
    private int _childCount;

    void Update()
    {
        _childCount =  gameObject.transform.childCount;
        if(_childCount == 0)
        {
            Invoke("LoadAgain", 0.2f);
        }
    }

    private void LoadAgain()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
