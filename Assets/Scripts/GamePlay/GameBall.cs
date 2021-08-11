using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    LineRenderer _line;

    [SerializeField]private float _points = 20;

    private float _timer = 0;
    private float _duration = 0.05f;
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _timer = _duration;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
            Hide();

    }
    void Hide()
    {
        _timer = _duration;

        _line.positionCount = 0;
    }

    public float GetPoints()
    {
        return _points;
    }
}
