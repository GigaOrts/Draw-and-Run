using UnityEngine;

public class Placer : MonoBehaviour
{
    [SerializeField] private LineDrawer _lineDrawer;
    [SerializeField] private Transform _entitiesParent;

    private Transform[] _entities;

    private void Awake()
    {
        _entities = new Transform[_entitiesParent.childCount];

        for (int i = 0; i < _entities.Length; i++)
        {
            _entities[i] = _entitiesParent.GetChild(i);
        }
    }

    private void OnEnable()
    {
        _lineDrawer.Finished += OnLineDrawerFinished;
    }
    
    private void OnDisable()
    {
        _lineDrawer.Finished -= OnLineDrawerFinished;
    }

    private void OnLineDrawerFinished(Vector3[] positions)
    {
        foreach (var position in positions)
        {
            position.Set(position.x, 0f, position.z);
        }
        
        int pointerStep = (int)Mathf.Floor((float)positions.Length / (_entities.Length + 1));
        int defaultPointerStep = 2;

        if (pointerStep <= defaultPointerStep)
        {
            PlaceRepetitive(positions, defaultPointerStep);
        }
        else
        {
            PlaceEqually(positions, pointerStep);
        }
    }

    private void PlaceEqually(Vector3[] positions, int pointerStep)
    {
        int currentPointer = pointerStep;

        for (int i = 0; i < _entities.Length; i++)
        {
            _entities[i].position = positions[currentPointer];
            currentPointer += pointerStep;
        }
    }

    private void PlaceRepetitive(Vector3[] positions, int defaultPointerStep)
    {
        int pointerStep = defaultPointerStep;
        int entityPointer = 0;
        int positionPointer = 0;

        while (entityPointer < _entities.Length)
        {
            if (positionPointer >= positions.Length - 1)
                positionPointer = 0;

            _entities[entityPointer].position = positions[positionPointer];
            positionPointer += pointerStep;

            entityPointer++;
        }
    }
}
