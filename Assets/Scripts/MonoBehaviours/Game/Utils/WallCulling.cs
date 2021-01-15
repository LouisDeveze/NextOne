using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WallCulling : MonoBehaviour
{
    // The renderer of the mesh
    public MeshRenderer meshRenderer;
    // The normal vector
    public Vector2 normal = Vector2.right;
    // The camera reference used to know player view angle
    private Camera sceneCamera;

    // Treshold = 1.0 for no backface culling
    // Treshold = 0.0 for full backface culling
    public static float treshold = 0f;


    private void Start()
    {
        this.sceneCamera = Camera.main;
    }

    void Update()
    {
        if(sceneCamera == null) { Debug.LogError("No camera found in the scene"); return; }

        // View Vector is the vector from the camera to the screen
        Vector3 vVector = sceneCamera.transform.forward ;
        // We keep only the 2D representation from an upper view point
        vVector.y = 0;
        vVector.Normalize();
        Vector2 viewVector = new Vector2(vVector.x, vVector.z);
        Debug.Log(viewVector);
        // Calculate the dot product of the view and the normal vector to see
        // if the wall is seen from behind
        float dotProduct = Vector2.Dot(viewVector, normal);
        if(dotProduct == 0) { }
        // If >0, the wall is seen from the back
        else if(dotProduct > treshold && meshRenderer.enabled) { meshRenderer.enabled = false; }
        // if <= 0, the wall is facing the camera
        else if (dotProduct <= treshold  && !meshRenderer.enabled) { meshRenderer.enabled = true; }


    }

}
