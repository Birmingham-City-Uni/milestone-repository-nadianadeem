using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public bool isEnemy;
    public int health = 100;
    Animator playerAnimator;
    public float Timer;

    public int playerHeavyAttackDmg = 65;
    public int playerQuickAttackDmg = 15;

    private void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if(!isEnemy && health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(isEnemy && Timer < 0)
        {
            if (collision.gameObject.CompareTag("Sword"))
            {
                if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack"))
                {
                    health -= playerHeavyAttackDmg;
                    Timer = 0.5f;
                }

                if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Quick Attack"))
                {
                    health -= playerQuickAttackDmg;
                    Timer = 0.5f;
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Explosion"))
            {
                Damage collisionDmgComp = collision.gameObject.GetComponent<Damage>();
                if (collisionDmgComp)
                {
                    health -= collisionDmgComp.damage;
                    Timer = 0.5f;
                }
            }

            if (!isEnemy && Timer < 0 && !collision.gameObject.CompareTag("Sword"))
            {
                Damage collisionDmgComp = collision.gameObject.GetComponent<Damage>();
                if (collisionDmgComp && collision.transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    health -= collisionDmgComp.damage;
                    Timer = 0.5f;
                }
            }
        }
    }
}
