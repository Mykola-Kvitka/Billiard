using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private float _score = 0;
    private void Awake()
    {
        int numbeOfThings = FindObjectsOfType<HUD>().Length;
        if(numbeOfThings > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(float point)
    {
        _score += point;
        scoreText.text = _score.ToString();
    }

}
