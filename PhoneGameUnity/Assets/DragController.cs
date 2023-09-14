using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public Draggable LastDragged => _LastDragged;

    private bool _isDragActive = false;

    private Vector2 _ScreenPosition;

    private Vector3 _WorldPosition;

    private Draggable _LastDragged;

    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if(controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isDragActive && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Drop();
            return;
        }

        if(Input.touchCount > 0)
        {
            _ScreenPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            _ScreenPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else
        {
            return;
        }

        _WorldPosition = Camera.main.ScreenToWorldPoint(_ScreenPosition);

        if (_isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_WorldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if(draggable != null)
                {
                    _LastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag()
    {
        LastDragged.LastPosition = LastDragged.transform.position;
        UpdateDragStatus(true);
    }

    void Drag()
    {
        _LastDragged.transform.position = new Vector2(_WorldPosition.x, _WorldPosition.y);
    }

    void Drop()
    {
        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        _isDragActive = LastDragged.IsDragging = isDragging;
        _LastDragged.gameObject.layer = isDragging ? Layers.Dragging : Layers.Default; 
    }
}
