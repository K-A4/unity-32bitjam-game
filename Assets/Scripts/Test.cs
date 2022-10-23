using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyEnumeratorExt
{
    public static Enum GetEnumerator(this Enum enumerator)
    {
        return enumerator;
    }
}

public class Enum
{
    private float[] v3;
    public object Current => v3[position];
    private int position = -1;

    public Enum(float[] v3)
    {
        this.v3 = v3;
    }

    public bool MoveNext()
    {
        position++;
        return position < v3.Length;
    }
}

public class Test : MonoBehaviour
{
    private void Start()
    {
        Enum container = new Enum(new float[3] { 1.0f, 2.0f, 3.0f });

        foreach (var item in container)
        {
            print(item);
        }
    }

    private void Update()
    {
        
    }
}
