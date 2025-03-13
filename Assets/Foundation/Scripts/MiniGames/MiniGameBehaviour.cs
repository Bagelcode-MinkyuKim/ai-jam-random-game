using System.Collections;
using UnityEngine;

public class MiniGameBehaviour : MonoBehaviour
{
    private bool flag = true;

    protected void Exit()
    {
        flag = false;
    }
    public IEnumerator Run2()
    {
        while (flag)
        {
            yield return null;
        }
    }
    public IEnumerator Run2A()
    {
        while (flag)
        {
            yield return null;
        }
    }
    public IEnumerator Run()
    {
        while (flag)
        {
            yield return null;
        }
    }
}