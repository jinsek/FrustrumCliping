using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrumClipper : MonoBehaviour
{
    public Camera cullCamera;
    public GameObject viewTraget;
    public CreateScene sceneInfo;
    private Bounds cullBox;
    // Start is called before the first frame update
    void Start()
    {
        var box = viewTraget.GetComponent<Collider>();
        cullBox = box.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(cullCamera.transform.position, viewTraget.transform.position);
        float distanceTop = Vector3.Distance(cullCamera.transform.position, viewTraget.transform.position + Vector3.up * cullBox.size.y * 0.5f);
        float aspect = cullBox.size.x / cullBox.size.y;
        float fov = Mathf.Rad2Deg * Mathf.Acos(distance / distanceTop) * 2;
        Matrix4x4 projM = Matrix4x4.Perspective(fov, aspect, cullCamera.nearClipPlane, distance);
        Matrix4x4 world2Cam = cullCamera.worldToCameraMatrix;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(projM * world2Cam);
        for(int i= sceneInfo.BoxBNDs.Count - 1; i>= 0; --i)
        {
            var box = sceneInfo.BoxBNDs[i];
            if (GeometryUtility.TestPlanesAABB(planes, box))
            {
                sceneInfo.ShowHideBox(i, false);
            }
            else
            {
                sceneInfo.ShowHideBox(i, true);
            }
        }
    }
}
