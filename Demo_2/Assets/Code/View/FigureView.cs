using ColorChessModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureView : MonoBehaviour
{
    public Action<FigureView> EventFigureClicked;

    private BoxCollider _boxCollider;
    private Position _position;

    public void Up()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
        StateBoxCollider(false);
    }

    public void Down()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        StateBoxCollider(true);
    }

    public IEnumerator AnimateMove(List<Vector3> way)
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = way[1];
        
        float progress;

        for (int i = 0; i < way.Count; i++)
        {
            progress = 0f;

            if (i != 0)
            {
                startPosition.x = way[i - 1].x;
                startPosition.z = way[i - 1].z;
                startPosition.y = endPosition.y;
            }

            endPosition.x = way[i].x;
            endPosition.z = way[i].z;

            while (transform.localPosition != endPosition)
            {
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
                progress += (0.35f * 1f);
            }
        }


        yield return new WaitForSeconds(0.02f * Time.deltaTime);

        Relocate(endPosition);

        Down();

         // Coroutine finished after executing AllOffColliders, but method Down turn on collider
         // Need to switch it off manually
        _boxCollider.enabled = false; 
    }

    public void Relocate(Vector3 newPosition)
    {
        Position = new Position(newPosition.x, newPosition.z);
    }

    private void OnMouseUpAsButton()
    {
        EventFigureClicked.Invoke(this);
    }

    public void StateBoxCollider(bool boxCol)
    {
        _boxCollider.enabled = boxCol;
    }
    
    public void SetRotation(int rotate)
    {
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.up);
    }
    public void FindComponents()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public Position Position
    {
        get => _position;
        set
        {
            _position = value;
            transform.localPosition = new Vector3(_position.X, 0, _position.Y);
        }
    }
}
