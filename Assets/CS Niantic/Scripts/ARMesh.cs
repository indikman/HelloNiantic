using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMesh : MonoBehaviour
{
    private MeshRenderer rend;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void updateMaterial(Material mat)
    {
        rend.material = mat;
    }

    private void OnEnable()
    {
        MeshMaterialManager.Instance.AddMesh(this);
    }

    private void OnDestroy()
    {
        MeshMaterialManager.Instance.RemoveMesh(this);
    }
}
