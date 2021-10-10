using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomRenderQueue : MonoBehaviour
{
    public UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

    // public bool apply = false;

    // private void Update()
    // {
    //     if (apply)
    //     {
    //         apply = false;
    //         Debug.Log("Updated material val");
    //         Image image = GetComponent<Image>();
    //         Material existingGlobalMat = image.materialForRendering;
    //         Material updatedMaterial = new Material(existingGlobalMat);
    //         updatedMaterial.SetInt("unity_GUIZTestMode", (int)comparison);
    //         image.material = updatedMaterial;
    //     }
    // }

    private void Start()
    {
        MaskableGraphic[] graphics = GetComponentsInChildren<MaskableGraphic>(true);

        foreach (MaskableGraphic graphic in graphics)
        {
            Material existingGlobalMat = graphic.materialForRendering;
            Material updatedMaterial = new Material(existingGlobalMat);
            updatedMaterial.SetInt("unity_GUIZTestMode", (int)comparison);
            graphic.material = updatedMaterial;
        }
    }
}