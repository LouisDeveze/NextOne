using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXCameras : MonoBehaviour
{
    //Use 2D? 
    public bool use2D;

    //The game object which contains our cameras
    public GameObject cameras;

    //VFXs to spawns - list
    public List<GameObject> VFXs = new List<GameObject>();

    //UI
    public Text effectName;
    //Firing Point Scripts

    public GameObject firePoint;

    //The script we're going to use to rotate to mouse
    public RotateToMouse rotateToMouse;

    private List<Camera> camerasList = new List<Camera>();
    private List<Camera> VFXcamerasList = new List<Camera>();
    private Camera singleCamera;

    //The current effect to be spawn
    private GameObject effectToSpawn;

    private int count = 0;

    //The time to fire
    private float timeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (cameras.transform.childCount > 0)
        {
            //Get all the cameras object from the parent
            for (int i = 0; i < cameras.transform.childCount; i++)
            {
                camerasList.Add(cameras.transform.GetChild(i).gameObject.GetComponent<Camera>());
                VFXcamerasList.Add(camerasList[i].transform.GetChild(0).GetComponent<Camera>());
            }

            if (camerasList.Count == 0)
            {
                Debug.Log("Please assign one or more Cameras in inspector");
            }
        }
        else
        {
            singleCamera = cameras.GetComponent<Camera>();
            if (singleCamera != null)
                camerasList.Add(singleCamera);
            else
                Debug.Log("Please assign one or more Cameras in inspector");
        }

        //If there is VFXs to spawn
        if (VFXs.Count > 0)
        {
            //Instantiate with the first vfx on the list
            effectToSpawn = VFXs[0];
        }
        else Debug.Log("Please assign one or more VFXs in inspector");

        //Set the UI to the according effect being spawned
        if (effectName != null && VFXs.Count > 0)
            effectName.text = effectToSpawn.name;

        //Start Ray Casting
        if (camerasList.Count > 0)
        {
            //The first camera to use for ray tracing
            rotateToMouse.setCamera(camerasList[0]);
            if (use2D)
                rotateToMouse.set2D(use2D);
            rotateToMouse.StartUpdateRay();
        }
        else
            Debug.Log("Please assign one or more Cameras in inspector");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (VFXs.Count > 0)
        {
            //If Fire rate or Cooldown is up and Space or M1 pressed -> Spawn VFX
            if (Input.GetKey(KeyCode.Space) && Time.time >= timeToFire ||
                Input.GetMouseButton(0) && Time.time >= timeToFire)
            {
                timeToFire = Time.time + 1f / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
                SpawnVFX();
            }

            //If D is pressed switch to next VFX (If Applicable)
            if (Input.GetKeyDown(KeyCode.D))
                Next();
            //If Q is pressed switch to previous VFX (If Applicable)
            if (Input.GetKeyDown(KeyCode.Q))
                Previous();
        }

        // Press C to switch camera
        if (Input.GetKeyDown(KeyCode.C))
            SwitchCamera();
        // Press X to zoom in
        if (Input.GetKeyDown(KeyCode.X))
            ZoomIn();
        // Press W to zoom out
        if (Input.GetKeyDown(KeyCode.W))
            ZoomOut();
    }

    public void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            if (rotateToMouse != null)
            {
                vfx.transform.localRotation = rotateToMouse.getRotation();
            }
            else Debug.Log("No RotateToMouseScript found on firePoint.");
        }
        else
            vfx = Instantiate(effectToSpawn);

        var ps = vfx.GetComponent<ParticleSystem>();

        if (vfx.transform.childCount > 0)
        {
            ps = vfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        }
    }

    //Switch to next vfx if applicable
    public void Next()
    {
        count++;

        if (count > VFXs.Count)
            count = 0;

        for (int i = 0; i < VFXs.Count; i++)
        {
            if (count == i) effectToSpawn = VFXs[i];
            if (effectName != null) effectName.text = effectToSpawn.name;
        }
    }

    //Switch to previous vfx if applicable
    public void Previous()
    {
        count--;

        if (count < 0)
            count = VFXs.Count;

        for (int i = 0; i < VFXs.Count; i++)
        {
            if (count == i) effectToSpawn = VFXs[i];
            if (effectName != null) effectName.text = effectToSpawn.name;
        }
    }

    // Zoom In from all the cameras i.e. change Field of View (fov)
    public void ZoomIn()
    {
        if (camerasList.Count > 0)
        {
            if (!camerasList[0].orthographic)
            {
                if (camerasList[0].fieldOfView < 101)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].fieldOfView += 5;
                        VFXcamerasList[i].fieldOfView += 5;
                    }
                }
            }
            else
            {
                if (camerasList[0].orthographicSize < 10)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].orthographicSize += 0.5f;
                        VFXcamerasList[i].orthographicSize += 0.5f;
                    }
                }
            }
        }
    }

    // Zoom Out from all the cameras i.e. change fov
    public void ZoomOut()
    {
        if (camerasList.Count > 0)
        {
            if (!camerasList[0].orthographic)
            {
                if (camerasList[0].fieldOfView > 20)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].fieldOfView -= 5;
                        VFXcamerasList[i].fieldOfView -= 5;
                    }
                }
            }
            else
            {
                if (camerasList[0].orthographicSize > 4)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].orthographicSize -= 0.5f;
                        VFXcamerasList[i].orthographicSize -= 0.5f;
                    }
                }
            }
        }
    }

    // Switch between cameras
    public void SwitchCamera()
    {
        if (camerasList.Count > 0)
        {
            for (int i = 0; i < camerasList.Count; i++)
            {
                if (camerasList[i].gameObject.activeSelf)
                {
                    camerasList[i].gameObject.SetActive(false);
                    if ((i + 1) == camerasList.Count)
                    {
                        camerasList[0].gameObject.SetActive(true);
                        rotateToMouse.setCamera(camerasList[0]);
                        break;
                    }
                    else
                    {
                        camerasList[i + 1].gameObject.SetActive(true);
                        rotateToMouse.setCamera(camerasList[i+1]);
                        break;
                    }
                }
            }
        }
    }
}