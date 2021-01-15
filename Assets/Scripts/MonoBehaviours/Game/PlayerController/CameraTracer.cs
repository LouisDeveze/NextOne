using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracer : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.5f;
    private Vector3 offset;
    public float distance;

    //SmoothLookAt à ameliorer
    void LateUpdate()
    {
        offset.y = Mathf.Sqrt(Mathf.Pow(distance, 2) / 2);
        offset.z = Mathf.Sqrt(Mathf.Pow(distance, 2) / 2) * -1;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //transform.LookAt(target);
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

    }
}
