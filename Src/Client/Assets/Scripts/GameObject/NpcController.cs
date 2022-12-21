﻿using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public int npcId;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Animator anim;
    Color originColor;
    private bool inInteractive = false;
    NpcDefine npc;
    NpcQuestStatus questStatus;

    void Start()
    {
        skinnedMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = gameObject.GetComponentInChildren<Animator>();
        originColor = skinnedMeshRenderer.sharedMaterial.color;
        npc = NpcManager.Instance.GetNpcDefine(npcId);
        NpcManager.Instance.UpdateNpcPosition(this.npcId, this.transform.position);
        StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range( 5f,10f));
            }
            Relax();
        }
    }

    void Relax()
    {
        anim.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.currentCharacterObject.transform.position - transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(gameObject.transform.forward, faceTo)) > 5) 
        {
            gameObject.transform.forward = Vector3.Lerp(transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, User.Instance.currentCharacterObject.transform.position) > 2f) 
        {
            User.Instance.currentCharacterObject.StartNav(this.transform.position);
        }
        Interactive();
    }

    private void OnMouseEnter()
    {
        HighLight(true);
    }

    private void OnMouseOver()
    {
        HighLight(true);
    }

    private void OnMouseExit()
    {
        HighLight(false);
    }

    void HighLight(bool highLight)
    {
        if (highLight)
        {
            if (skinnedMeshRenderer.sharedMaterial.color != Color.white)
            {
                skinnedMeshRenderer.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (skinnedMeshRenderer.sharedMaterial.color != originColor)
            {
                skinnedMeshRenderer.sharedMaterial.color = originColor;
            }
        }
    }

    void OnQuestStatusChanged(Quest quest)
    {
        RefreshNpcStatus();
    }

    void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(npcId);
        UIWorldElementManager.Instance.AddNpcQuestStatus(transform, questStatus);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(transform);
        }
    }
}
