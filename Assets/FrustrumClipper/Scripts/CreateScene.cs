using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class HidableObject
{
    public MeshRenderer Mesh;
    public Material defaultMat;
    public Material transparentMat;
}

public class CreateScene : MonoBehaviour
{
    public Bounds BND;
    public float cubeSize = 1;
    public Object cubePrefab;
    public Material transparent;
    private List<HidableObject> lHidableObjects = new List<HidableObject>();
    [HideInInspector]
    public List<Bounds> BoxBNDs = new List<Bounds>();    
    void Awake()
    {
        int x_range = Mathf.CeilToInt(BND.size.x / cubeSize);
        int z_range = Mathf.CeilToInt(BND.size.z / cubeSize);
        for(int z = 0; z<z_range; ++z)
        {
            for (int x = 0; x < x_range; ++x)
            {
                var scale = new Vector3(cubeSize, BND.size.y, cubeSize);
                var pos = new Vector3((x + 0.5f) * cubeSize, BND.center.y, (z + 0.5f) * cubeSize);
                pos += BND.min;
                var rate = Mathf.PerlinNoise(pos.x, pos.z);
                if (rate > 0.75f)
                {
                    var go = GameObject.Instantiate(cubePrefab) as GameObject;
                    go.transform.position = pos;
                    go.transform.localScale = scale;
                    HidableObject ho = new HidableObject();
                    ho.Mesh = go.GetComponent<MeshRenderer>();
                    ho.defaultMat = ho.Mesh.material;
                    lHidableObjects.Add(ho);
                    BoxBNDs.Add(new Bounds(pos, scale));
                }
            }
        }
        Debug.Log("box count : " + lHidableObjects.Count);
    }
    public void ShowHideBox(int idx, bool show)
    {
        var ho = lHidableObjects[idx];
        if (ho.transparentMat == null)
        {
            ho.transparentMat = MonoBehaviour.Instantiate(transparent);
        }
        if (show)
        {
            ho.Mesh.material = ho.defaultMat;
        }
        else
        {
            ho.Mesh.material = ho.transparentMat;
        }
    }
    public void OnDestroy()
    {
        foreach(var ho in lHidableObjects)
        {
            if (ho.defaultMat != null)
                Destroy(ho.defaultMat);
            if (ho.transparentMat != null)
                Destroy(ho.transparentMat);
        }
        lHidableObjects.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(BND.center, BND.size);
    }
#endif
}
