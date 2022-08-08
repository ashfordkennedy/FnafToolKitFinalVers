using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeadBob
{
    public float HorizontalRange = 0.3f;
    public float VerticalRange = 0.3f;
    public AnimationCurve BobCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),  new Keyframe(1f, 0f), new Keyframe(1.5f, -1f), new Keyframe(2f, 0f));

    public float VerticalHorizontalRatio = 1f;


    private float _cyclePositionX;
    private float _cyclePositionY;
    private float _bobInterval;
    private Vector3 _originalCameraPosition;
    private float _time;


    public void Setup(Camera targetCamera, float bobInterval)
    {
        _bobInterval = bobInterval;
        _originalCameraPosition = targetCamera.transform.localPosition;
        _time = BobCurve[BobCurve.length - 1].time;
    }


    public Vector3 DoHeadBob(float speed)
    {
        float xPos = _originalCameraPosition.x + (BobCurve.Evaluate(_cyclePositionX) * HorizontalRange);
        float yPos = _originalCameraPosition.y + (BobCurve.Evaluate(_cyclePositionY) * VerticalRange);

        _cyclePositionX += (speed * Time.deltaTime) / _bobInterval;
        _cyclePositionY += ((speed * Time.deltaTime) / _bobInterval) * VerticalHorizontalRatio;


        if (_cyclePositionX > _time)
        {
            _cyclePositionX = _cyclePositionX - _time;
        }
        if (_cyclePositionY > _time)
        {
            _cyclePositionY = _cyclePositionY - _time;
        }


        return new Vector3(xPos, yPos, 0f);
    }


    public Vector3 GenerateOffset(float speed)
    {
        float xPos = _originalCameraPosition.x + (BobCurve.Evaluate(_cyclePositionX) * HorizontalRange);
        float yPos = _originalCameraPosition.y + (BobCurve.Evaluate(_cyclePositionX) * VerticalRange);


        _cyclePositionX += (speed * Time.deltaTime);
        _cyclePositionX %= _time;

        _cyclePositionY += (speed * Time.deltaTime);
        _cyclePositionX %= _time;

        Debug.Log(new Vector3(xPos, yPos, 0f) + "is your offset");
        return new Vector3(xPos, yPos, 0f);
    }


}
