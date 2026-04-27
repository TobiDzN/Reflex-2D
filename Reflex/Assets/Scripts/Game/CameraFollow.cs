using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float minX, minY, maxX, maxY;

    void Start()
    {

    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        if (target == null) return;

        float x = Mathf.Clamp(target.position.x, minX, maxX);
        float y = Mathf.Clamp(target.position.y, minY, maxY);

        transform.position = new Vector3(x, y, -10f);
    }


}
