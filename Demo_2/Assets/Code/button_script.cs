using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button_script : MonoBehaviour
{
   public Image but;
    private void Awake()
    {
        but = this.GetComponent<Image>();
    }
    public void disable_button()
    {
        //ColorBlock cb = ColorBlock.defaultColorBlock;
        //cb.normalColor = UnityEngine.Color.black;
        but.color = UnityEngine.Color.black;
    }

    public void enable_button()
    {
        //ColorBlock cb = ColorBlock.defaultColorBlock;
        //cb.normalColor = UnityEngine.Color.green;
        but.color = UnityEngine.Color.green;
    }

    public void change_color() 
    {

    }
}

