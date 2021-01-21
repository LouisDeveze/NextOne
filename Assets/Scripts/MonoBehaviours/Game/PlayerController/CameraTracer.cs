using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracer : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.8f;
    private Vector3 offset;
    public float distance;
    public float angle;

    //SmoothLookAt à ameliorer
    void LateUpdate()
    {
        offset.y = Mathf.Sin(Mathf.Deg2Rad*angle)*distance;
        offset.z = -Mathf.Cos(Mathf.Deg2Rad*angle)*distance;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //transform.LookAt(target);
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

    }
}
