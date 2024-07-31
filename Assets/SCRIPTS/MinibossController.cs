// MinibossController
// Handles miniboss movement
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinibossController : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform target;
    [HideInInspector] public Animator anim;
    [HideInInspector] public MinibossStats minibossStats;
    [HideInInspector] public MinibossCombat minibossCombat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerCapsule").transform;
        anim = GetComponent<Animator>();
        minibossStats = GetComponent<MinibossStats>();
        minibossCombat = GetComponent<MinibossCombat>();

        // Set stoppingDistance to shootRange
        agent.stoppingDistance = minibossStats.shootRange;
    }

    private void Update()
    {
        if (!minibossStats.isDead)
        {
            // Outside of shootRange
            if (Vector3.Distance(transform.position, target.position) > minibossStats.shootRange)
            {
                if (anim.GetBool("movementEnabled"))
                {
                    anim.SetBool("inShootRange", false);
                    anim.SetBool("inMeleeRange", false);
                    anim.SetBool("isShooting", false);
                    MoveToTarget();
                }
            }
            // Inside meleeRange
            else if (Vector3.Distance(transform.position, target.position) < minibossStats.meleeRange)
            {
                Vector3 lookTarget = target.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget);
                anim.SetBool("inShootRange", false);
                anim.SetBool("inMeleeRange", true);
                anim.SetBool("isShooting", false);
            }
            // Inside shootRange && !Inside meleeRange
            else
            {
                Vector3 lookTarget = target.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget);
                anim.SetBool("inMeleeRange", false);
                anim.SetBool("inShootRange", true);

                Shooting();
            }
        }
        else
        {
            anim.SetBool("isDead", true);

            // Find death animation
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            DestroyCollider();
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "Die")
                    Invoke("DestroyComponents", 3f/*clip.length*/); // Invoke after animation completed
            }
        }
    }

    private void Shooting()
    {
        // if in sight (not behind wall)
        anim.SetBool("isShooting", true);
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
    }

    private void DestroyCollider()
    {
        Destroy(gameObject.GetComponent<CapsuleCollider>());
    }

    private void DestroyComponents()
    {
        foreach (Component c in gameObject.GetComponents<Component>()) // Destroy components
        {
            if (!(c is Transform)) // Ignore transform component (to keep position)
            {
                Destroy(c);
            }
        }
    }
}
