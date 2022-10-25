using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationInputController : MonoBehaviour
{
    public Animator playerAnimator;
    public InputActionAsset playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }

        if (playerController.FindAction("Heavy Attack").WasPressedThisFrame())
        {
            playerAnimator.SetTrigger("IsHeavyAttack");
        }
    }
}
