using System;
using System.Collections.Generic;
using ColorChessModel;
using System.Diagnostics;
using UnityEngine;
using System.Text;

static public class TestTrash 
{
    public static Dictionary<string, int> score = new();

    static int Equal = 0;
    public static int test = 0;



    public static int Contain(int[] a)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < a.Length; i++)
        {
            sb.Append(a[i]);

            if (i < a.Length - 1)
            {
                sb.Append(""); // ƒобавл€ем разделитель между значени€ми (пример: "1, 2, 3")
            }
        }

        string result = sb.ToString();


        if (score.ContainsKey(result))
        {
            return score[result];
        }
        else 
        {
            return Int32.MinValue;
        }
    }

    public static void Add(int[] a, int b) 
    {

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < a.Length; i++)
        {
            sb.Append(a[i]);

            if (i < a.Length - 1)
            {
                sb.Append(""); // ƒобавл€ем разделитель между значени€ми (пример: "1, 2, 3")
            }
        }

        string result = sb.ToString();


        if (score.ContainsKey(result))
        {
            Equal++;
        }
        else score.Add(result, b);
 
    }

 
    public static void Refresh() 
    {
        test = 0;
        //score = new();
    }

 

 

    public static void Print() 
    {
        UnityEngine.Debug.Log("Equals: " + Equal);
        UnityEngine.Debug.Log("Counter: " + score.Count);
    }
}
