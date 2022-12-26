using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public bool isEnemy;
    public int currentHealth = 100;
    Animator playerAnimator;
    public float Timer;

    public int playerHeavyAttackDmg = 65;
    public int playerQuickAttackDmg = 15;
    public int StartingHealth = 100;

    private void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        currentHealth = StartingHealth;
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if(!isEnemy && currentHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Academy.Instance.Dispose();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
                    currentHealth -= playerHeavyAttackDmg;
                    Timer = 0.5f;
                }

                if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Quick Attack"))
                {
                    currentHealth -= playerQuickAttackDmg;
                    Timer = 0.5f;
                }
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if(Timer < 0)
        {
            if (isEnemy)
            {
                if (collision.gameObject.CompareTag("Sword"))
                {
                    if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack"))
                    {
                        currentHealth -= playerHeavyAttackDmg;
                        Timer = 0.5f;
                    }

                    if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Quick Attack"))
                    {
                        currentHealth -= playerQuickAttackDmg;
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
                        currentHealth -= collisionDmgComp.damage;
                        Timer = 0.5f;
                        Destroy(collision.gameObject.GetComponent<Collider>());
                    }
                }

                if (!collision.gameObject.CompareTag("Sword"))
                {
                    Damage collisionDmgComp = collision.gameObject.GetComponent<Damage>();
                    if (collisionDmgComp && collision.transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        currentHealth -= collisionDmgComp.damage;
                        Timer = 0.5f;
                    }
                }
            }
        }
    }
}

