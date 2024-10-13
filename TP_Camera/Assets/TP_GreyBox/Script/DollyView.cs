using UnityEngine;

public class DollyView : AView
{
    public float roll;
    public float fov;
    public Transform target;
    public Rail rail;
    public float distanceOnRail = 0f;
    public float speed = 5f;


    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        if (rail == null)
        {
            Debug.LogWarning("Rail non défini pour DollyView.");
            return config;
        }


        Vector3 railPosition = rail.GetPosition(distanceOnRail);


        if (target != null)
        {
            Vector3 dir = (target.position - railPosition).normalized;


            config.yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            config.pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        }


        config.roll = roll;
        config.fieldOfView = fov;
        config.pivot = railPosition;
        config.distance = 0;

        return config;
    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");  // Récupère l'input horizontal (clavier ou manette)
        distanceOnRail += input * speed * Time.deltaTime;  // Déplacement en fonction de la vitesse


        if (rail.isLoop)
        {
            distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
        }
        else
        {
            distanceOnRail = Mathf.Clamp(distanceOnRail, 0, rail.GetLength());
        }
    }
}