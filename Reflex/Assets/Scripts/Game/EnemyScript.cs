using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Refrences
    public AnimatorOverrideController[] characters;
    Animator animator;
    public GameObject aimIndicator, shotPrefab, shieldObject, firePoint;
    public Transform player;

    //Sounds
    AudioSource audioSource;
    public AudioClip shotSFX, shieldSFX;

    //Timers
    private float fireRateTimer = 0f;
    private float shootCooldownTimer = 0f;
    private float shieldReactionTimer = 0f;
    private float shieldCooldownTimer = 0f;

    //Config
    public float fireRate = 1f;
    public float shootCooldown = 5f;
    public float shieldReactionSpeed = 0.5f;
    public float shieldCooldown = 5f;
    public float aimError = 0f;
    public float bulletDetectRange = 10f;

    //Movement
    public float moveSpeed = 3f;
    public float stoppingDistance = 3f;


    void Start()
    {
        //Choose Random Character
        int rnd = Random.Range(0, 4);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.runtimeAnimatorController = characters[rnd];
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        //Difficulty
        if (MainMenuScript.selectedDiff == "Easy")
        {
            fireRate = 1.3f;
            shootCooldown = 5f;
            shieldReactionSpeed = 999f;
            shieldCooldown = 999f;
            aimError = 25f;
        }
        else if (MainMenuScript.selectedDiff == "Medium")
        {
            fireRate = 1f;
            shootCooldown = 5f;
            shieldReactionSpeed = 0.4f;
            shieldCooldown = 5f;
            aimError = 10f;
        }
        else if (MainMenuScript.selectedDiff == "Hard")
        {
            fireRate = 0.6f;
            shootCooldown = 5f;
            shieldReactionSpeed = 0.1f;
            shieldCooldown = 5f;
            aimError = 0f;
        }


    }

    void Update()
    {
        UpdateTimers();
        TryShield();
        AimToPlayer();
        MoveToPlayer();
        TryShoot();
    }

    void UpdateTimers()
    {
        fireRateTimer -= Time.deltaTime;
        shootCooldownTimer -= Time.deltaTime;
        shieldReactionTimer -= Time.deltaTime;
        shieldCooldownTimer -= Time.deltaTime;
    }

    void TryShoot()
    {
        //Validations
        if (fireRateTimer > 0) return;
        if (shootCooldownTimer > 0) return;

        GameObject newShot = Instantiate(shotPrefab, firePoint.transform.position, aimIndicator.transform.rotation);
        newShot.GetComponent<ShotEngine>().shooter = gameObject;
        audioSource.PlayOneShot(shotSFX);
        fireRateTimer = fireRate;
        shootCooldownTimer = shootCooldown;
    }

    void AimToPlayer()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float randomOffset = Random.Range(-aimError, aimError);
        angle += randomOffset;
        angle -= 90f;
        aimIndicator.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryShield()
    {
        //Validations
        if (MainMenuScript.selectedDiff == "Easy") return;
        if (shieldReactionTimer > 0) return;
        if (shieldCooldownTimer > 0) return;
        if (!IsBulletThreatening()) return;

        shieldObject.SetActive(true);
        audioSource.PlayOneShot(shieldSFX);
        shieldCooldownTimer = shieldCooldown;
        StartCoroutine(ShieldDuration());
    }

    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(1f);
        shieldReactionTimer = shieldReactionSpeed;
        shieldObject.SetActive(false);
    }

    bool IsBulletThreatening()
    {
        ShotEngine[] allShots = UnityEngine.Object.FindObjectsByType<ShotEngine>(FindObjectsSortMode.None);
        foreach (ShotEngine shot in allShots)
        {
            //Validations
            if (shot == null) continue;
            if (shot.shooter == null) continue;
            if (shot.shooter == gameObject) continue;
            if (shot.shooter.CompareTag("Enemy")) continue;

            //Rest
            float dist = Vector2.Distance(transform.position, shot.transform.position);
            if (dist > bulletDetectRange)
                continue;
            Vector2 toBot = (Vector2)transform.position - (Vector2)shot.transform.position;
            Vector2 bulletDir = shot.transform.up.normalized;
            float dot = Vector2.Dot(bulletDir, toBot.normalized);
            //
            Debug.DrawLine(shot.transform.position, transform.position, Color.red);
            //

            float perpendicularDistance = Mathf.Abs(Vector2.Dot(Vector2.Perpendicular(bulletDir).normalized, toBot));

            if (perpendicularDistance > 0.5f)
                continue;

            if (dot > 0.7f)
            {
                if (MainMenuScript.selectedDiff == "Medium")
                {
                    if (!shot.shieldRollDone)
                    {
                        shot.shieldRollDone = true;
                        shot.shieldWillTrigger = Random.value >= 0.5f;
                    }

                    if (!shot.shieldWillTrigger)
                        continue;
                }
                return true;
            }
        }
        return false;
    }

    void MoveToPlayer()
    {
        if (shieldObject.activeSelf)
            return;

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            dir += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            dir = dir.normalized;
            Vector2 animDir = dir;
            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
            animator.SetFloat("InputX", animDir.x);
            animator.SetFloat("InputY", animDir.y);
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }

}
