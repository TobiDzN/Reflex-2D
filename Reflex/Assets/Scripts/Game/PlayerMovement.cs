using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Related
    float inputX, inputY;
    public float speed = 5f;

    //Animations
    Animator animator;
    public AnimatorOverrideController[] characterAOCs;

    public Transform aimIndicator;
    public CombatController cmbt;
    private int characterIndex;

    //Dash Related
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    float dashCooldown = 5f;
    private bool isDashing = false;
    private bool canDash = true;



    void Start()
    {
        animator = GetComponent<Animator>();
        cmbt = GetComponent<CombatController>();
        if (MainMenuScript.selectedCharacter == "Luna")
        {
            ChooseCharacter(0);
        }
        else if (MainMenuScript.selectedCharacter == "Rick")
        {
            ChooseCharacter(1);
        }
        else if (MainMenuScript.selectedCharacter == "Shelly")
        {
            ChooseCharacter(2);
        }
        else if (MainMenuScript.selectedCharacter == "Zip")
        {
            ChooseCharacter(3);
        }
    }

    void Update()
    {
        ReadInput();
        if (!isDashing)
            CharacterMovement();
        MouseRotation();
        UpdateAnimation();
        CharacterSkills();
    }

    public void ChooseCharacter(int index)
    {
        animator.runtimeAnimatorController = characterAOCs[index];
        characterIndex = index;
    }

    void CharacterSkills()
    {
        if (characterIndex == 0)//LunaLockshot
        {
            cmbt.reloadTime = 3f;
        }
        else if (characterIndex == 1)//Rick O'Shae
        {
            cmbt.enableRicochet = true;
        }
        else if (characterIndex == 2)//Shelly D.Flect
        {
            cmbt.shieldDuration = 1.5f;
        }
        else if (characterIndex == 3)//Zip Vortex
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && !isDashing)
            {
                StartCoroutine(DashForward());
                if (cmbt.isReloading)
                {
                    cmbt.FinishReloadInstant(true);  // finish reload + give ammo
                }
                else
                {
                    cmbt.FinishReloadInstant(false); // fix UI only, no ammo added
                }
            }
        }
    }

    void ReadInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", new Vector2(inputX, inputY).sqrMagnitude);
    }

    void UpdateAnimation()
    {
        animator.SetFloat("InputX", inputX);
        animator.SetFloat("InputY", inputY);
    }

    IEnumerator DashForward()
    {
        isDashing = true;
        canDash = false;
        float startTime = Time.time;
        Vector2 dashDir = aimIndicator.up;
        while (Time.time < startTime + dashDuration)
        {
            transform.position += (Vector3)dashDir * dashSpeed * Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void CharacterMovement()
    {
        Vector3 move = new Vector3(inputX, inputY, 0f);
        transform.Translate(move * speed * Time.deltaTime);
    }
    void MouseRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - aimIndicator.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        aimIndicator.rotation = Quaternion.Euler(0, 0, angle);
    }

}
