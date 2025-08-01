using UnityEngine;
using UnityEngine.AI;


public class Unit: MonoBehaviour
{
    public Transform position;


    NavMeshAgent nav;
    Animator animator;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

   private void Update()
   {
        
        if(position)
        {
            nav.SetDestination(position.position);
        }
        Animate();
   }

    protected virtual void Animate()
    {
        var vectorSpeed = nav.velocity;
        vectorSpeed.y = 0;

        float speed = vectorSpeed.magnitude;

        animator.SetFloat("Speed", speed);
    }


}
