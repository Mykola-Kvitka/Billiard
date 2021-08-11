using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteBall : MonoBehaviour
{
    [SerializeField] private Transform stick;

    private LineRenderer _line;
    private LineRenderer _line2;
    private LineRenderer _line3;
    
    private bool _follow = true;
    private float _ballRadius = 0;
    private LayerMask _layerMaskBallsAndWalls = 1 << 6 | 1 << 7;
    private LayerMask _layerMaskWalls = 1 << 7;

    private RaycastHit2D _hit;
    private float _dist = 0;
    private float _rayCastDistance = 99;
    private float _minDist = -1;
    private float _maxDist = -5;
    private float _forceMultiplier = 8f;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line2 = stick.GetComponent<LineRenderer>();
        _line.positionCount = 3;
        _line2.positionCount = 40;


        _ballRadius = gameObject.GetComponent<CircleCollider2D>().radius;
        _minDist = -(_ballRadius + _ballRadius / 2);
        _dist = Mathf.Clamp(_maxDist / 2, _maxDist, _minDist);

    }

    private void Update()
    {

        Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        
        if (_follow)
        {
            StickRotation(mPos);

            // raycast от шара вперед
            _hit = Physics2D.CircleCast(gameObject.transform.position, _ballRadius, stick.up, _rayCastDistance, _layerMaskBallsAndWalls);
            // попадание в шар
            if (_hit.collider != null)
            {
                DrawBallTrajectory();

            }
            else
            { // 
              // raycast на стены
                BallTrajectoryToWalls();

            }
        }


        if (_follow && Input.GetButtonUp("Fire1"))
        {
            Strike();
        }

        if (!_follow)
        {
            if (gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.015f)
            {
                _follow = true;
            }
        }

    }

    private void StickRotation(Vector3 mPos)
    {
        //Поворот кия вокруг шара
        Vector3 temp = (stick.position - gameObject.transform.position).normalized;
        stick.rotation = Quaternion.LookRotation(Vector3.forward, mPos - gameObject.transform.position);
        stick.position = gameObject.transform.position;
        stick.localPosition += stick.up * _dist;
    }

    private void BallTrajectoryToWalls()
    {
        _hit = Physics2D.CircleCast(stick.position, _ballRadius, stick.up, _rayCastDistance, _layerMaskWalls);
        if (_hit.collider != null)
        {
            Vector3 reflectDir2 = Vector3.Reflect((new Vector3(_hit.centroid.x, _hit.centroid.y, 0) - stick.position).normalized, _hit.normal);
            DrawLine(gameObject.transform.position, _hit.centroid, reflectDir2);
        }
    }

    private void DrawBallTrajectory()
    {
        Vector3 reflectDir = Vector3.Reflect((_hit.transform.position - new Vector3(_hit.centroid.x, _hit.centroid.y, 0) - gameObject.transform.position).normalized, _hit.normal);
        DrawLine(gameObject.transform.position, _hit.centroid, reflectDir);

        if (_hit.collider.CompareTag("GameBall"))
        {
            Vector3 targetDir = (_hit.transform.position - new Vector3(_hit.centroid.x, _hit.centroid.y, 0)).normalized * 10;
            _line3 = _hit.collider.gameObject.GetComponent<LineRenderer>();
            DrawRay(_hit.centroid, targetDir);
        }
    }

    private void Strike()
    {
        Vector3 pos;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.0f));
        }
        else
        {
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
        }

        Vector3 forceDir = -(gameObject.transform.position - pos).normalized * _forceMultiplier;
        gameObject.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
        _follow = false;
    }

    //Траэктория игрового шара
    private void DrawRay(Vector2 centroid, Vector3 targetDir)
    {

        _line3.positionCount = 2;
        _line3.SetPosition(0, centroid);
        _line3.SetPosition(1, targetDir);
    }

    //Траэтория шара после удара 
    private void DrawLine(Vector3 start, Vector3 end, Vector3 reflect)
    {

        float deltaTheta = (2f * Mathf.PI) / 40;
        float theta = 0f;
        float radius = _ballRadius * 0.5f;
        for (int i = 0; i < 40; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
            _line2.SetPosition(i, pos + end);
            theta += deltaTheta;
        }

        _line.SetPosition(0, start);
        _line.SetPosition(1, end);
        _line.SetPosition(2, reflect + end);

    }



}



