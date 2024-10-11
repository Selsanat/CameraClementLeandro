using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private static ViewVolumeBlender _instance;
    public static ViewVolumeBlender Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ViewVolumeBlender>();
            }
            return _instance;
        }
    }

    private List<AViewVolume> ActiveViewVolumes = new List<AViewVolume>();
    private Dictionary<GameObject, List<AViewVolume>> VolumesPerViews = new Dictionary<GameObject, List<AViewVolume>>();
    public List<AViewVolume> GetActiveViewVolumes()
    {
        return ActiveViewVolumes;
    }
    private void Update()
    {
        // Réinitialiser les poids des vues actives à 0
        foreach (var view in VolumesPerViews.Keys)
        {
            view.GetComponent<AView>().weight = 0f; 
        }

        // Trier les volumes actifs par priorité, puis par Uid
        var sortedVolumes = ActiveViewVolumes.OrderBy(v => v.Priority).ThenBy(v => v._Uid).ToList();

        // Mélange des volumes actifs
        foreach (var volume in sortedVolumes)
        {
            float weight = Mathf.Clamp(volume.ComputeSelfWeight(), 0f, 1f);
            float remainingWeight = 1.0f - weight;

            // Multiplier le poids des vues actives par le poids restant
            foreach (var view in VolumesPerViews.Keys)
            {
                view.GetComponent<AView>().weight *= remainingWeight;
            }

            // Ajouter le poids au volume associé
            volume.View.GetComponent<AView>().weight += weight;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        if (!ActiveViewVolumes.Contains(volume))
        {
            ActiveViewVolumes.Add(volume);
        }

        if (!VolumesPerViews.ContainsKey(volume.View))
        {
            VolumesPerViews[volume.View] = new List<AViewVolume>();
            volume.View.SetActive(true);
        }

        VolumesPerViews[volume.View].Add(volume);
    }

    public void RemoveVolume(AViewVolume volume)
    {
        ActiveViewVolumes.Remove(volume);

        if (VolumesPerViews.ContainsKey(volume.View))
        {
            VolumesPerViews[volume.View].Remove(volume);

            if (VolumesPerViews[volume.View].Count == 0)
            {
                VolumesPerViews.Remove(volume.View);
                volume.View.SetActive(false);
            }
        }
    }
}

