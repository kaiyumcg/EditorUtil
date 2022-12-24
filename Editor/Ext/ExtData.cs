using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KEditorUtil
{
    public delegate void OnDoAnythingFunc2(int level);
    public delegate void OnDoAnythingFunc3(int level, float raise, float totalNow);
    public delegate void OnDoAnythingFunc4(int level, float raise, float totalNow, float totalPrev);
    public delegate bool WhenToDoFunc();
    public delegate bool WhenToDoFunc<T>(T data1);
    public delegate bool WhenToDoFunc<T1, T2>(T1 data1, T2 data2);
    public delegate bool WhenToDoFunc<T1, T2, T3>(T1 data1, T2 data2, T3 data3);

    public delegate void OnDoAnything();
    public delegate void OnDoAnything<T>(T data1);
    public delegate void OnDoAnything<T1, T2>(T1 data1, T2 data2);
    public delegate void OnDoAnything<T1, T2, T3>(T1 data1, T2 data2, T3 data3);
}