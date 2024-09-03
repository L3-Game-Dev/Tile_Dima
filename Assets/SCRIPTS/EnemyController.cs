// EnemyController
// Handles enemy movement
// Created by Dima Bethune 14/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform target;
    public Animator anim;
    public EnemyStats enemyStats;

    private void Awake()
    {
        // Set references
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerCapsule").transform;
        anim = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        if (!enemyStats.isDead) // If enemy is alive
            MoveToTarget();
        else
        {
            anim.SetBool("isDead", true);

            agent.SetDestination(transform.position);

            // Find death animation
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            DestroyCollider();
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "Zombie Death")
                    Invoke("DestroyComponents", clip.length); // Invoke after death animation completed
            }
        }
    }

    /// <summary>
    /// Sets the agent's target destination to the player's location and determines what action to take
    /// </summary>
    private void MoveToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        agent.SetDestination(target.position);

        if (distanceToTarget <= agent.stoppingDistance)
        {
            Vector3 lookTarget = target.position;
            lookTarget.y = transform.position.y;
            transform.LookAt(lookTarget);
            anim.SetFloat("speed", 0f, 0.3f, Time.deltaTime);
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetFloat("speed", 1f, 0.3f, Time.deltaTime);
            anim.SetBool("isAttacking", false);
        }
    }

    /// <summary>
    /// Play metal footstep sound
    /// </summary>
    public void FootstepMetal()
    {
        if (GameStateHandler.gameState == "PLAYING")
            AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyFootstepMetal, transform.position);
    }

    /// <summary>
    /// Destroys the enemy's collider component
    /// </summary>
    private void DestroyCollider()
    {
        Destroy(gameObject.GetComponent<CapsuleCollider>());
    }

    /// <summary>
    /// Destroys all components attached to the enemy object
    /// </summary>
    private void DestroyComponents()
    {
        foreach (Component c in gameObject.GetComponents<Component>()) // Destroy components
        {
            if (!(c is Transform)) // Ignore transform component (to keep position)
            {
                Destroy(c);
            }
        }
        foreach (Transform t in gameObject.transform.Find("root")) // Destroy weapons
        {
            if (t.CompareTag("EnemyWeapon"))
            {
                Destroy(t.gameObject);
            }
        }
    }
}
