using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    Mesh mesh = null;
    public int ID;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up * transform.localScale.y * 0.5f;
        Gizmos.color = Color.red;
        if (mesh != null) 
        {
            Gizmos.DrawWireMesh(mesh, pos, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, pos, transform.rotation, 1f, EventType.Repaint);
        UnityEditor.Handles.Label(pos, "SpawnPoint" + ID);
    }
#endif
}
