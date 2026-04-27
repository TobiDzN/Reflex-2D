using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    //Prefabs And Objects
    public GameObject shotPrefab;
    public GameObject aimIndicator;
    public GameObject shield;
    public GameObject firePoint;

    //Bullet Related
    public int shotCount = 1;
    public int maxShots = 3;
    public float reloadTime = 5f;
    public Image[] bulletImages;
    public bool isReloading = false;
    public bool enableRicochet = false;
    private Coroutine reloadRoutine;

    //Shield Related
    public float shieldDuration = 1f;
    public float shieldCooldown = 5f;
    public bool isShielding = false;
    public bool shieldOnCooldown = false;
    public Image shieldImage;

    void Start()
    {
        UpdateBulletUI();

        if (shieldImage != null)
            shieldImage.fillAmount = 1f;
    }

    void Update()
    {
        if (PauseManager.isPaused) return;

        ReadInput();
    }

    void ReadInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        else if (Input.GetMouseButton(1))
        {
            Shield();
        }

    }

    void Shoot()
    {
        //Cant Shoot If Reloading
        if (isReloading)
            return;

        if (shotCount > 0)
        {
            GameObject newShot = Instantiate(shotPrefab, firePoint.transform.position, aimIndicator.transform.rotation);
            newShot.GetComponent<ShotEngine>().shooter = gameObject;
            if (enableRicochet)
            {
                ShotEngine b = newShot.GetComponent<ShotEngine>();
                b.canRicochet = true;
                b.maxRicochets = 2;
            }
            shotCount--;

            if (shotCount > 0)
            {
                UpdateBulletUI();
            }
            else
            {
                reloadRoutine = StartCoroutine(ReloadRoutine());
            }
        }
    }

    IEnumerator ReloadRoutine()
    {
        if (isReloading)
            yield break;

        isReloading = true;

        Image reloadImage = bulletImages[2];

        reloadImage.fillAmount = 0f;

        float t = 0f;
        while (t < reloadTime)
        {
            t += Time.deltaTime;
            reloadImage.fillAmount = t / reloadTime;
            yield return null;
        }

        shotCount = Mathf.Min(shotCount + 1, maxShots);

        reloadImage.fillAmount = 1f;

        UpdateBulletUI();

        isReloading = false;
    }

    public void FinishReloadInstant(bool giveAmmo)
    {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }
        isReloading = false;
        if (giveAmmo)
        {
            shotCount = Mathf.Min(shotCount + 1, maxShots);
        }
        Image reloadImage = bulletImages[2];
        reloadImage.fillAmount = 1f;
        UpdateBulletUI();
    }


    void UpdateBulletUI()
    {
        bulletImages[0].enabled = (shotCount >= 3);
        bulletImages[1].enabled = (shotCount >= 2);
        bulletImages[2].enabled = (shotCount >= 1);
    }


    public void RewardBullet()
    {
        if (shotCount < maxShots)
        {
            shotCount++;
            UpdateBulletUI();
        }
    }

    public void OnBlockSuccessful()
    {
        RewardBullet();
    }

    void Shield()
    {
        if (isShielding) return;
        if (shieldOnCooldown) return;

        StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        isShielding = true;
        shieldOnCooldown = true;

        shield.SetActive(true);

        if (shieldImage != null)
            shieldImage.fillAmount = 0f;

        float d = 0f;
        while (d < shieldDuration)
        {
            d += Time.deltaTime;
            if (shieldImage != null)
                shieldImage.fillAmount = d / shieldDuration;
            yield return null;
        }

        shield.SetActive(false);

        isShielding = false;

        float c = 0f;
        while (c < shieldCooldown)
        {
            c += Time.deltaTime;
            if (shieldImage != null)
                shieldImage.fillAmount = c / shieldCooldown;
            yield return null;
        }

        shieldOnCooldown = false;
    }



}
