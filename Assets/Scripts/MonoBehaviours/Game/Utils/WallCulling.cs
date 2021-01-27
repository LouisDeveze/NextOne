using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextOne;

[RequireComponent(typeof(MeshRenderer))]
public class WallCulling : MonoBehaviour
{
    // The renderer of the mesh
    public MeshRenderer meshRenderer;
    // The normal vector
    public Vector2 normal = Vector2.right;
    // The camera reference used to know player view angle
    private Camera sceneCamera;

    private Material[] materials;
    // Semi Transparent Material
    private Material[] semiTransparent;

    private GameContext context;

    private bool isTransparent = false;

    public bool debug = false;


    private void Start()
    {
        this.sceneCamera = Camera.main;
        this.materials = meshRenderer.sharedMaterials;

        this.context = GameObject.Find("State Manager").GetComponent<GameContext>();
        Material st = context.semiTransparent;
        semiTransparent = new Material[materials.Length];
        for (int i = 0; i < semiTransparent.Length; i++) semiTransparent[i] = st;
    }

    void Update()
    {
        if(sceneCamera == null) { Debug.LogError("No camera found in the scene"); return; }

        Vector3 pos = this.transform.position;
        Vector3 playerLogicalPos = context.playerController.Model.transform.position;
        playerLogicalPos.z = 4*Mathf.RoundToInt(playerLogicalPos.z/4);
        

        bool shouldBeOpaque = true;
        /* if (context.playerOccluded
             && (Mathf.Abs(playerLogicalPos.x - pos.x) < 10f)
             && (Mathf.Abs(playerLogicalPos.z - pos.z) < 2f)
             ) shouldBeOpaque = false;
 */
        // Default for walls seen from behind
        if (normal.y == 1) shouldBeOpaque = false;
        // If walls are facing and culling the player
        else if (context.playerOccluded && normal.y == -1 && playerLogicalPos.z > pos.z) shouldBeOpaque = false;
        

        if(shouldBeOpaque && isTransparent) { SwitchToOpaque();} 
        else if(!shouldBeOpaque && !isTransparent) { SwitchToTransparent(); } 
    }

    public void SwitchToTransparent()
    {
        this.meshRenderer.sharedMaterials = semiTransparent;
        isTransparent = true;
    }

    public void SwitchToOpaque()
    {
        this.meshRenderer.sharedMaterials = materials;
        isTransparent = false;
    }

}
