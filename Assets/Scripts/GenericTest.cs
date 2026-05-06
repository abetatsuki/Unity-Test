using System;
using System.Collections.Generic;
using UnityEngine;


public class GenericTest<T> where T : Behaviour
{
    int a;
    int b;
    public GenericTest(T a)
    {
        Value = a;
    }
    public T Value;

    List<int> test = new List<int>(1);

    private void Test()
    {

        Swap(ref a, ref b);
    }

    private void Swap(ref T a, ref T b)
    {

    }

    private void Swap(ref int a, ref int b)
    {

    }

}
