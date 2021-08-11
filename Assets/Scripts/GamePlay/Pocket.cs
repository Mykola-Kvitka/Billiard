using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pocket : MonoBehaviour
{
    private float _points;
    [SerializeField] HUD hud;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("GameBall"))
        {
           float points = collision.gameObject.GetComponent<GameBall>().GetPoints();
           hud.AddScore(points);
           Destroy(collision.gameObject);
        }
    }
}
