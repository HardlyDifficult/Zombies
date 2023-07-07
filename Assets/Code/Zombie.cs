using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    float impuleForce;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        var startingAnimation = Random.value > 0.25f ? "Walk" : "Run";

        // Randomize the animation start time so that the zombies don't all move in sync
        animator.Play(startingAnimation, 0, Random.Range(0, 1f));

        // Randomize the animation speed
        animator.speed = Random.Range(0.5f, 1.5f);
    }

    public void OnHit(float damage, Collider collider, Vector3 bulletDirection, Vector3 hitPoint)
    {
        OnDeath(collider.gameObject, bulletDirection, hitPoint);
    }

    void OnDeath(GameObject hitComponent, Vector3 bulletDirection, Vector3 hitPoint)
    {
        Debug.Log($"Zombie died from {hitComponent.name} shot");

        animator.enabled = false;
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var body in bodies)
        {
            body.isKinematic = false;
            if (body.gameObject == hitComponent)
            {
                body.AddForceAtPosition(bulletDirection * impuleForce, hitPoint, ForceMode.Impulse);
            }
        }
    }
}
