using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius = 10f;
    public float innerRadius = 5f;

    private float distance;

    private void Update()
    {
        // Calculer la distance entre la cible et la position du volume
        distance = Vector3.Distance(transform.position, target.transform.position);

        // Activer ou désactiver en fonction de la distance
        if (distance <= outerRadius && !IsActive)
        {
            View.GetComponent<AView>().SetActive(true);
        }
        else
        {
            View.GetComponent<AView>().SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        // Vérifier si on est à l'intérieur des rayons
        if (distance > outerRadius)
        {
            return 0f;
        }
        else if (distance <= innerRadius)
        {
            return 1f;
        }
        else
        {
            // Calculer un poids progressif en fonction de la distance
            float weight = 1f - ((distance - innerRadius) / (outerRadius - innerRadius));
            return Mathf.Clamp(weight, 0f, 1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
