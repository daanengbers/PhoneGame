using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public GameObject[] MergedPrefabs;

    public bool IsDragging;

    public bool CanMerge;

    public Vector3 LastPosition;

    private Collider2D _collider;

    private DragController _dragController;

    private float _movementTime = 15f;

    private System.Nullable<Vector3> _movementDestination;

    private string mergedString;

    public string mergeString;


    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _dragController = FindObjectOfType<DragController>();
    }

    private void FixedUpdate()
    {
        if (_movementDestination.HasValue)
        {
            if (IsDragging)
            {
                _movementDestination = null;
                return;
            }

            if(transform.position == _movementDestination)
            {
                gameObject.layer = Layers.Default;
                _movementDestination = null;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _movementDestination.Value, _movementTime * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Draggable colliderDraggable = other.GetComponent<Draggable>();
        
        

        if(colliderDraggable != null && _dragController.LastDragged.gameObject == gameObject)
        {
            /*ColliderDistance2D colliderDistance2D = other.Distance(_collider);
            Vector3 diff = new Vector3(colliderDistance2D.normal.x, colliderDistance2D.normal.y) * colliderDistance2D.distance;
            transform.position -= diff;*/
            if (CanMerge == true && colliderDraggable.CanMerge == true)
            {             
                string otherMergeString = colliderDraggable.mergeString;
                mergedString = mergeString + otherMergeString;

                Vector2 mergePosition = colliderDraggable.transform.position;

                if (mergedString == "KK")
                {
                    Instantiate(MergedPrefabs[0], new Vector2(mergePosition.x, mergePosition.y), Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    Debug.Log("Great Succes");
                }
            }

            transform.position = LastPosition;
        }

     

        if (other.CompareTag("DropValid"))
        {
            _movementDestination = other.transform.position;
        }
        else if (other.CompareTag("DropInvalid"))
        {
            _movementDestination = LastPosition;
        }
    }
    
  
        
    }

