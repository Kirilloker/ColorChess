using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void Start()
    {
        Promt_create();
    }
    public void Promt_create() 
    {
        // Создается подсказка над клеткой

        this.name = "Promt " + this.transform.localPosition.x + "_" + this.transform.localPosition.z;
        this.transform.localScale = new Vector3(0.25f, 0.1f, 0.25f);
        meshRenderer = this.GetComponent<MeshRenderer>();
        hide();
    }

    public void show(bool _figure_in_cell = false) 
    {
        // Включается подсказка

        if (_figure_in_cell) { this.transform.localScale = new Vector3(0.95f, 0.05f, 0.95f); }

        meshRenderer.enabled = true;
    }

    public void hide() 
    {
        // Выключается подсказка
        this.transform.localScale = new Vector3(0.25f, 0.1f, 0.25f);
        meshRenderer.enabled = false;
    }

    public void Promt_destroy() 
    {
        // Удаляется подсказка над клеткой 
        GameObject.Destroy(gameObject);
    }
}