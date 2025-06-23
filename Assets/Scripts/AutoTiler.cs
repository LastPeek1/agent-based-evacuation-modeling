using UnityEngine;

[ExecuteInEditMode]
public class AutoTiler : MonoBehaviour
{
    public float xScaleMult = 1;
    public float yScaleMult = 1;
    private Material material;

    private Material Material
    {
        get
        {
            if (material == null)
            {
                MeshRenderer render = GetComponent<MeshRenderer>();

#if UNITY_EDITOR
                material = render.sharedMaterial;
#else
                    material = render.material;
#endif
            }
            return material;
        }
    }


    void Update()
    {
        Material.mainTextureScale = new Vector2(Mathf.Max(transform.localScale.x, transform.localScale.z) * xScaleMult, transform.localScale.y * yScaleMult);
    }
}