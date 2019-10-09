using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Occluder : MonoBehaviour
{
    public Material transparent;
    public Material normal;

    public List<Renderer> occluders = new List<Renderer>();
    public List<Renderer> newOccluders = new List<Renderer>();

    private void Update()
    {
        Vector3 camDirection = Camera.main.transform.position - transform.position;
        Ray ray = new Ray(transform.position, camDirection);
        List<RaycastHit> hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << 8).ToList();

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            occluders.Add(renderer);

            if (!newOccluders.Contains(renderer))
            {
                renderer.material = transparent;
                newOccluders.Add(renderer);
            }
        }

        List<Renderer> oldOccluders = newOccluders.Where(item => !occluders.Any(item2 => item2 == item)).ToList();

        foreach (Renderer hit in oldOccluders)
        {
            hit.material = normal;
            newOccluders.Remove(hit);
        }

        occluders.Clear();
    }
}
