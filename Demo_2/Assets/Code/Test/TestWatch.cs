
using System.Diagnostics;

static public class TestWatch
{
    static Stopwatch watch1 = new();
    static Stopwatch watch2 = new();
    static Stopwatch watch3 = new();
    static Stopwatch watch4 = new();
    static Stopwatch watch5 = new();
    static Stopwatch watch6 = new();
    public static void Start1() 
    {
        watch1.Start();
    }

    public static void Start2()
    {
        watch2.Start();
    }

    public static void Start3()
    {
        watch3.Start();
    }

    public static void Start4()
    {
        watch4.Start();
    }

    public static void Start5()
    {
        watch5.Start();
    }

    public static void Start6()
    {
        watch6.Start();
    }

    public static void Stop5()
    {
        watch5.Stop();
    }
    public static void Stop6()
    {
        watch6.Stop();
    }

    public static void Stop1()
    {
        watch1.Stop();
    }

    public static void Stop2()
    {
        watch2.Stop();
    }
    public static void Stop3()
    {
        watch3.Stop();
    }
    public static void Stop4()
    {
        watch4.Stop();
    }


    public static void Reset() 
    {
        watch1.Reset();
        watch2.Reset();
        watch3.Reset();
        watch4.Reset();
        watch5.Reset();
        watch6.Reset();
    }



    public static void Print() 
    {
        UnityEngine.Debug.Log("Wathc 1 :" + watch1.Elapsed.TotalMilliseconds);
        UnityEngine.Debug.Log("Wathc 2 :" + watch2.Elapsed.TotalMilliseconds);
        UnityEngine.Debug.Log("Wathc 3 :" + watch3.Elapsed.TotalMilliseconds);
        UnityEngine.Debug.Log("Wathc 4 :" + watch4.Elapsed.TotalMilliseconds);
        UnityEngine.Debug.Log("Wathc 5 :" + watch5.Elapsed.TotalMilliseconds);
        UnityEngine.Debug.Log("Wathc 6 :" + watch6.Elapsed.TotalMilliseconds);
    }
}

