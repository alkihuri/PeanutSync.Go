using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformController : MonoBehaviour
{

    SyncSceneViewManager _syncManager;
    private Vector3 _prevPosition;
    private Vector3 _currentPosition;
    [SerializeField] private List<Vector3> _lastPositions = new List<Vector3>();
    private Vector3 _nextPoint;

    public List<Vector3> LastPositions { get => _lastPositions; set => _lastPositions = value; }

    private void OnEnable()
    {
        StartCoroutine(TransformCheker());
    }

    private void OnDisable()
    {
        StopCoroutine(TransformCheker());
    }

    public void Innit(SyncSceneViewManager syncManager)
    {
        _syncManager = syncManager;
        var trail = gameObject.AddComponent<TrailRenderer>();
    }
    // Update is called once per frame
    IEnumerator TransformCheker()
    {
        _currentPosition = transform.position;
        _prevPosition = _currentPosition;
        while (gameObject.activeInHierarchy)
        {
             
            _currentPosition = transform.position;  

            if (_currentPosition != _prevPosition)
            {
                _syncManager?.OnAnyObjectChanged?.Invoke();
                _currentPosition = transform.localPosition;

            }

            _prevPosition = transform.position;

            for (int i = 0; i < 10; i++)
            {
                if (!LastPositions.Contains(transform.localPosition))
                    LastPositions.Add(transform.localPosition);
                yield return new WaitForSeconds(1 / 10);
                if (LastPositions.Count > 10)
                    LastPositions.RemoveAt(0);
            }
        }


    }


}
