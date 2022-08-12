using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaterialManager : MonoBehaviour
{
    public static MeshMaterialManager Instance;
    public Material[] meshMaterials;

    List<ARMesh> ARMeshes = new List<ARMesh>();
    int currentMat = 0;

    private void Awake()
    {
        if(Instance != null && Instance != this )
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        currentMat = 0;
    }

    public void AddMesh(ARMesh mesh)
    {
        ARMeshes.Add(mesh);
    }

    public void RemoveMesh(ARMesh mesh)
    {
        try
        {
            ARMeshes.Remove(mesh);
        }
        catch (System.Exception)
        {
            Debug.Log("Cannot find the mesh");
        }
        
    }

    public void SwithMaterial()
    {
        if (meshMaterials.Length <= 0) return;

        currentMat++;

        if (currentMat >= meshMaterials.Length) currentMat = 0;

        foreach (var item in ARMeshes)
        {
            item.updateMaterial(meshMaterials[currentMat]);
        }
    }

}
