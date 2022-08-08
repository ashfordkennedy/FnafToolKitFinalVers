using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class JumpBob
{
    public float BobDuration;
    public float BobAmount;

    private float _offset = 0f;

    public float Offset()
    {
        return _offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator DoBobCycle()
    {
        // make the camera move down slightly
        float t = 0f;
        while (t < BobDuration)
        {
            _offset = Mathf.Lerp(0f, BobAmount, t / BobDuration);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // make it move back to neutral
        t = 0f;
        while (t < BobDuration)
        {
            _offset = Mathf.Lerp(BobAmount, 0f, t / BobDuration);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        _offset = 0f;
    }
}
