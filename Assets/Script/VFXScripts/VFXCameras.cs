using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCameras : MonoBehaviour
{
    public GameObject cameras;
    private List<Camera> camerasList = new List<Camera> ();
    private Camera singleCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        if (cameras.transform.childCount > 0) {
            for (int i = 0; i < cameras.transform.childCount; i++) {
                camerasList.Add (cameras.transform.GetChild (i).gameObject.GetComponent<Camera> ());
            }
            if(camerasList.Count == 0){
                Debug.Log ("Please assign one or more Cameras in inspector");
            }
        } else {
            singleCamera = cameras.GetComponent<Camera> ();
            if (singleCamera != null)
                camerasList.Add (singleCamera);
            else
                Debug.Log ("Please assign one or more Cameras in inspector");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Press C to switch camera
        if (Input.GetKeyDown(KeyCode.C))
            SwitchCamera();
        // Press X to zoom in
        if (Input.GetKeyDown (KeyCode.X))
            ZoomIn ();
        // Press W to zoom out
        if (Input.GetKeyDown (KeyCode.W))
            ZoomOut ();
    }
    
    // Zoom In from all the cameras i.e. change Field of View (fov)
    public void ZoomIn () {
        if (camerasList.Count > 0) {
            if (!camerasList [0].orthographic) {
                if (camerasList [0].fieldOfView < 101) {
                    for (int i = 0; i < camerasList.Count; i++) {
                        camerasList [i].fieldOfView += 5;
                    }
                }
            } else {
                if (camerasList [0].orthographicSize < 10) {
                    for (int i = 0; i < camerasList.Count; i++) {
                        camerasList [i].orthographicSize += 0.5f;
                    }
                }
            }
        }
    }

    // Zoom Out from all the cameras i.e. change fov
    public void ZoomOut () {
        if (camerasList.Count > 0) {
            if (!camerasList [0].orthographic) {
                if (camerasList [0].fieldOfView > 20) {
                    for (int i = 0; i < camerasList.Count; i++) {
                        camerasList [i].fieldOfView -= 5;
                    }
                }
            } else {
                if (camerasList [0].orthographicSize > 4) {
                    for (int i = 0; i < camerasList.Count; i++) {
                        camerasList [i].orthographicSize -= 0.5f;
                    }
                }
            }
        }
    }
    
    // Switch between cameras
    public void SwitchCamera () {
        if (camerasList.Count > 0) {
            for (int i = 0; i < camerasList.Count; i++) {
                if (camerasList [i].gameObject.activeSelf) {
                    camerasList [i].gameObject.SetActive (false);
                    if ((i + 1) == camerasList.Count) {
                        camerasList [0].gameObject.SetActive (true);
                        break;
                    } else {
                        camerasList [i + 1].gameObject.SetActive (true);
                        break;
                    }
                }
            }
        }
    }
}
