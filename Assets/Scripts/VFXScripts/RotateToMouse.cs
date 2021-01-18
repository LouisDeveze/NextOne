using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public float maximumLength;

    private bool use2D;
    private Ray rayMouse;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;
    private Camera camera;
    private WaitForSeconds updateTime = new WaitForSeconds(.1f);

    public void StartUpdateRay()
    {
        StartCoroutine(UpdateRay());
    }

    IEnumerator UpdateRay()
    {
        if (camera != null)
        {
            if (use2D)
            {
                //Get the direction from the camera perspective
                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //Because we're in a 2D space -> angle [-180;180]
                if (angle > 180) angle -= 360;
                //Lock on the grid X -> .... Y | .... (Z:0)
                rotation.eulerAngles = new Vector3(-angle, 90, 0); // use different values to lock on different axis
                //Assign the rotation
                transform.rotation = rotation;
            }
            else
            {
                RaycastHit hit;
                //Get the position of the mouse
                var mousePos = Input.mousePosition;
                rayMouse = camera.ScreenPointToRay(mousePos);
                //Check if there's a collider at the current position
                if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumLength))
                {
                    //The point where the collider is
                    RotateTo(gameObject, hit.point);
                }
                else
                {
                    var pos = rayMouse.GetPoint(maximumLength);
                    RotateTo(gameObject, pos);
                }
            }

            yield return updateTime;
            StartCoroutine(UpdateRay());
        }
        else Debug.Log("Camera not set");
    }

    void RotateTo(GameObject o, Vector3 destination)
    {
        //Get the direction
        direction = destination - o.transform.position;
        //Create the rotation
        rotation = Quaternion.LookRotation(direction);
        o.transform.localRotation = Quaternion.Lerp(o.transform.rotation, rotation, 1);
    }

    public void set2D(bool state)
    {
        use2D = state;
    }

    public void setCamera(Camera camera)
    {
        this.camera = camera;
    }

    public Vector3 getDirection()
    {
        return direction;
    }

    public Quaternion getRotation()
    {
        return rotation;
    }
}