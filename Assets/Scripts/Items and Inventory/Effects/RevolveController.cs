using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveController : MonoBehaviour
{
    public Transform parent;
    //public Transform parent;
    public float pAngLocalX = 60;
    public float pAngleWorldZ = 0;
    public float pOmega = 180;

    public float radius = 2;
    public float angle = 0;//初始角度方向为-x轴
    public float omega = 360;//旋转角速度

    public TrailRenderer trail;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle += pOmega * Time.fixedDeltaTime;

        if (pAngleWorldZ > 360) pAngleWorldZ -= 360;
        else if (pAngleWorldZ < 0) pAngleWorldZ += 360;
        parent.rotation = Quaternion.Euler(0, 0, pAngleWorldZ) * Quaternion.Euler(pAngLocalX, 0, 0);

        angle += omega * Time.fixedDeltaTime;
        if (angle > 360) angle -= 360;
        else if (angle < 0) angle += 360;

        if(angle > 0 && angle <= 180)
        {
            //sr.sortingOrder = SortingLayer.GetLayerValueFromName("Player") + 1;
            //trail.sortingOrder = SortingLayer.GetLayerValueFromName("Player") + 1;
            sr.sortingOrder = 2;
            trail.sortingOrder = 1;

        }
        else
        {
            sr.sortingOrder = -1;
            trail.sortingOrder = -2;
            //sr.sortingOrder = SortingLayer.GetLayerValueFromName("Player") - 1;
            //trail.sortingOrder = SortingLayer.GetLayerValueFromName("Player") - 1;
        }

        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        transform.localPosition = radius * dir;
        transform.rotation = Quaternion.Euler(0 ,0 ,0);

    }
}
