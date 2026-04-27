using UnityEngine;

public class ShotEngine : MonoBehaviour
{

    //Movement
    public float speed = 10f;
    float dir;

    //Ricochet
    public bool canRicochet = false;
    public int maxRicochets = 2;
    private int ricochetCount = 0;

    //Refrences
    [HideInInspector] public GameObject shooter;
    public CombatController cmbt;

    //Flags
    public bool shieldRollDone = false;
    public bool shieldWillTrigger = false;

    void Start()
    {
        Destroy(gameObject, 3);
        if (shooter.CompareTag("Player"))
        {
            cmbt = shooter.GetComponent<CombatController>();
        }
    }


    void Update()
    {
        dir = speed * Time.deltaTime;
        transform.Translate(0, dir, 0);

        Debug.DrawLine(
    transform.position,
    transform.position + transform.up * 2f,
    Color.blue
);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        if (shooter == null) return;

        if (collision.collider.CompareTag("Wall") && canRicochet && ricochetCount < maxRicochets)
        {
            Vector2 normal = collision.contacts[0].normal;

            Vector2 currentDir = transform.up;
            Vector2 reflectDir = Vector2.Reflect(currentDir, normal);

            float angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            ricochetCount++;
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && shooter.CompareTag("Player"))
        {
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

            if (enemy == null) return;

            if (enemy.shieldObject.activeSelf)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
            cmbt.FinishReloadInstant(true);
        }
        else if (collision.gameObject.CompareTag("Player") && shooter.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            PauseManager.isDead = true;
        }

    }


}
