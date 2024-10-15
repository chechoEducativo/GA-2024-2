using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayerMask;
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private Vector2 navVel;
    private Vector2 smoothNavVel;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!gameObject.scene.IsValid()) return;
        bool state = ctx.ReadValueAsButton();
        if (!state) return;
        Ray r = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(r, out RaycastHit hit, Mathf.Infinity, detectionLayerMask))
        {
            Debug.DrawLine(r.origin, r.origin + hit.point, Color.red, 2f);

            navMeshAgent.destination = hit.point;
        }
    }

    private void Update()
    {
        Vector3 animNavDelta = navMeshAgent.nextPosition - transform.position;
        animNavDelta.y = 0;
        float deltaX = Vector3.Dot(transform.right, animNavDelta);
        float deltaY = Vector3.Dot(transform.forward, animNavDelta);
        Vector2 positionDelta = new Vector2(deltaX, deltaY);
        float smoothingInterp = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothNavVel = Vector2.Lerp(smoothNavVel, positionDelta, smoothingInterp);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navVel = Vector2.Lerp(Vector2.zero, navVel, navMeshAgent.remainingDistance / navMeshAgent.stoppingDistance);
        }
        navVel = smoothNavVel / Time.deltaTime;
        anim.SetFloat("MotionY", navVel.y);
        anim.SetFloat("MotionX", navVel.x);
        
        float deltaMagnitude = positionDelta.magnitude;
        if (deltaMagnitude > navMeshAgent.radius * 0.5f)
        {
            transform.position = Vector3.Lerp(anim.rootPosition, navMeshAgent.nextPosition, smoothingInterp);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = anim.rootPosition;
        rootPosition.y = navMeshAgent.nextPosition.y;
        transform.position = rootPosition;
        navMeshAgent.nextPosition = rootPosition;
    }

    private void OnDrawGizmos()
    {
        if (anim == null || navMeshAgent == null) return;
        Gizmos.DrawLine(anim.rootPosition, anim.rootPosition + anim.rootRotation * Vector3.forward);
    }
}
