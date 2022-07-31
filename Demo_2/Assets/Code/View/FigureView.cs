using ColorChessModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureView : MonoBehaviour
{
    private FigureType type;
    private int numberPlayer;
    
    private FigureController figureController;

    private BoxCollider boxCollider;

    public void FindComponents()
    {
        boxCollider = this.GetComponent<BoxCollider>();
    }

    public void Up()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0.5f, this.transform.localPosition.z);
        StateBoxColodier(false);
    }

    public void Down()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, this.transform.localPosition.z);
        StateBoxColodier(true);
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

        Move(endPosition);

        Down();
        boxCollider.enabled = false; // Сам не понимаю из-за чего этот баг
    }

    public void Move(Vector3 endPos)
    {
        Pos = new Position(endPos.x, endPos.z);
    }

    private void OnMouseUpAsButton()
    {
        figureController.OnClicked(this);
    }

    public void StateBoxColodier(bool boxCol)
    {
        boxCollider.enabled = boxCol;
    }
    
    public void SetRotation(int rotate)
    {
        this.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.up);
    }

    public void SetType(FigureType _type)
    {
        type = _type;
    }

    public void SetNumberPlayer(int _numberPlayer)
    {
        numberPlayer = _numberPlayer;
    }

    public void SetFigureController(FigureController _figureController)
    {
        figureController = _figureController;
    }

    public Position Pos { 
        get 
        {
            //return new Position(transform.localPosition.x, transform.localPosition.z);
            return pos;
        } 
        set 
        {
            pos = value;
            this.transform.localPosition = new Vector3(value.X, 0, value.Y); 
        }
    }

    private Position pos;

    public FigureType Type { get { return type; } }

    public int Number { get { return numberPlayer; } }
}
