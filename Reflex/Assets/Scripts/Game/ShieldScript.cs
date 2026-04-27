using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public GameObject player;
    public CombatController combat;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!combat.isShielding) return;

        ShotEngine shot = collision.GetComponent<ShotEngine>();

        if (shot != null)
        {
            if (shot.shooter != player)
            {
                Destroy(collision.gameObject);
                combat.OnBlockSuccessful();
            }
        }
    }

}
