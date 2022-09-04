using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    public int ID;
    private Mesh mesh = null;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if (playerInputController != null && playerInputController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[ID];
            if (td == null)
            {
                Debug.LogFormat("TeleporterObject:character [{0}] Enter Teleporter [{1}],But TeleporterDefine not exited", playerInputController.character.Info.Name, ID);
                return;
            }
            Debug.LogFormat("TeleporterObject:character [{0}] Enter Teleporter [{1}:{2}]", playerInputController.character.Info.Name, td.ID,td.Name);
            if (td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleport(ID);
                }
                else
                {
                    Debug.LogFormat("Teleporter ID:{0} linkID:{1} error", td.ID, td.LinkTo);
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (mesh != null)
        {
            Gizmos.DrawWireMesh(mesh, transform.position + Vector3.up * transform.localScale.y * 0.5f, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1f, EventType.Repaint);
    }
#endif

}
