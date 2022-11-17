using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationInputController : MonoBehaviour
{
    public Animator playerAnimator;
    public InputActionAsset playerController;
    public List<GameObject> nameplates;
    public bool IsDebugOn = false;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("text"))
        {
            if (!nameplates.Contains(obj))
            {
                nameplates.Add(obj);
            }
        }

        if (playerController.FindAction("Move").IsPressed())
        {
            playerAnimator.SetBool("IsRunning", true);
        }
        else if(playerController.FindAction("Move").WasReleasedThisFrame())
        {
            playerAnimator.SetBool("IsRunning", false);
        }

        if (playerController.FindAction("Quick Attack").WasPressedThisFrame() && playerController.FindAction("Quick Attack").IsPressed())
        {
            playerAnimator.SetTrigger("IsQuickAttack");
            StartCoroutine(GameObject.FindGameObjectWithTag("sounds").GetComponent<SceneSound>().PlayDelaySound(2, 0.731f));
        }

        if (playerController.FindAction("Heavy Attack").WasPressedThisFrame())
        {
            playerAnimator.SetTrigger("IsHeavyAttack");
            StartCoroutine(GameObject.FindGameObjectWithTag("sounds").GetComponent<SceneSound>().PlayDelaySound(3, 0.5f));
        }

        if (playerController.FindAction("Debug").WasPressedThisFrame())
        {
            IsDebugOn = !IsDebugOn;
        }

        foreach (GameObject obj in nameplates)
        {
            if(obj != null)
            {
                obj.SetActive(IsDebugOn);
            }
        }
    }
}
