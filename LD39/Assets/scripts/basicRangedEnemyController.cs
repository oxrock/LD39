using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicRangedEnemyController : basicEnemyController {

    public float A_stoppingDistance;
    public float M_stoppingDistance;
    public GameObject projectile;
    public Transform fireballSpawn;

    public override void attackLogic()
    {
        
        
            if (NavTarget != null)
            {
                if (Vector3.Distance(transform.position, NavTarget.position) < attackRange)
                {
                    if (!attackRdy)
                    {
                        attackTimer += Time.deltaTime;
                        if (attackTimer > attackCooldown)
                        {
                            attackTimer = 0.0f;
                            attackRdy = true;
                        }
                    }
                    else
                    {
                        //shoot smart projectile
                        GameObject fireball = Instantiate<GameObject>(projectile, fireballSpawn.position, Quaternion.identity);
                        fireball.transform.SetParent(null);
                        fireball.GetComponent<fireballController>().setTarget(NavTarget, attackDmg);
                        attackRdy = false;

                    }
                     //myAgent.stoppingDistance = A_stoppingDistance;

                    
                }
                else
                {
                    //myAgent.stoppingDistance = M_stoppingDistance;
                }
            }
            else
            {
                findTarget();
            }
        
    }
    public override void Start()
    {
        //A_stoppingDistance = attackRange - .3f;

    }
}
