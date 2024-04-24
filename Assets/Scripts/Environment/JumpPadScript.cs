using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] Transform centerPos;
    private Vector3 targetVec;
    private Vector3 finalVel;

    [SerializeField] float jumpVel;

    private void Awake()
    {
        targetVec = targetPos.position - centerPos.position;
        targetVec = targetVec.normalized;

        finalVel = targetVec * jumpVel;
    }

    public Vector3 GetJumpVector()
    {
        return finalVel;
    }
}
