using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    public const int LeftMouseButton = 0;

    private int _positionIndex = 0;
    private Camera _mainCamera;
    private LineRenderer _line;
    private Vector3[] _positions;
    private Vector3 _lastHitPosition;

    public event Action<Vector3[]> Finished;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(LeftMouseButton) || Input.GetMouseButtonDown(LeftMouseButton))
        {
            return;
        }

        if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            bool isPositionChanged = _lastHitPosition != hit.point;

            if (isPositionChanged)
            {
                AddPoint(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            StartLine();
        }
        else if (Input.GetMouseButtonUp(LeftMouseButton))
        {
            AddPoint(_lastHitPosition);
            FinishLine();
        }
    }

    private void AddPoint(Vector3 position)
    {
        _line.positionCount++;
        _line.SetPosition(_positionIndex++, position);
        _lastHitPosition = position;
    }

    private void StartLine()
    {
        _line.positionCount = 0;
        _positionIndex = 0;
    }

    private void FinishLine()
    {
        _positions = new Vector3[_line.positionCount];

        for (int i = 0; i < _line.positionCount; i++)
        {
            _positions[i] = _line.GetPosition(i);
        }

        Finished?.Invoke(_positions);
    }
}
