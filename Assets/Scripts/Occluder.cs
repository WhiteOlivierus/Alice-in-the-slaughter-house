using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Occluder : MonoBehaviour
{
    private List<Renderer> occluders = new List<Renderer>();
    private List<Renderer> newOccluders = new List<Renderer>();

    private void Update()
    {
        Vector3 camDirection = Camera.main.transform.position - transform.position;
        Ray ray = new Ray(transform.position, camDirection);
        List<RaycastHit> hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << 8).ToList();

        HideMaterials(hits);

        ShowMaterial();

        occluders.Clear();
    }

    private void HideMaterials(List<RaycastHit> hits)
    {
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            occluders.Add(renderer);

            if (!newOccluders.Contains(renderer))
            {
                foreach (Material material in renderer.materials)
                {
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;

                    Color solidColor = material.color;
                    solidColor.a = 0.5f;
                    material.color = solidColor;
                    newOccluders.Add(renderer);
                }
            }
        }
    }

    private void ShowMaterial()
    {
        List<Renderer> oldOccluders = newOccluders.Where(item => !occluders.Any(item2 => item2 == item)).ToList();

        foreach (Renderer hit in oldOccluders)
        {
            foreach (Material material in hit.materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;

                Color transparentColor = material.color;
                transparentColor.a = 1f;
                material.color = transparentColor;
                newOccluders.Remove(hit);
            }
        }
    }
}
