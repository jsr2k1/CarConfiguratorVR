using System;
using UnityEngine;

namespace UnityEditor
{
internal class GridcellShaderGUI : ShaderGUI
{
	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
	{
		base.OnGUI(materialEditor, props);

		Material material = materialEditor.target as Material;

		UpdateFeatures(material);
	}

	public static void UpdateFeatures(Material material)
	{
		if (material.HasProperty("_ClearcoatIntensity"))
		{
			SetKeyword(material, "CLEARCOAT_ON", material.GetFloat("_ClearcoatIntensity") > 0.0f);
		}

		if (material.HasProperty("_DiffuseTexture"))
	    {
			SetKeyword(material, "DIFFUSEMAP_ON", material.GetTexture("_DiffuseTexture") != null);
		}

		if (material.HasProperty("_SpecularTexture"))
		{
			SetKeyword(material, "SPECULARMAP_ON", material.GetTexture("_SpecularTexture") != null);
		}

		if (material.HasProperty("_BumpTexture"))
		{
			SetKeyword(material, "NORMALMAP_ON", material.GetTexture("_BumpTexture") != null);
		}

		if (material.HasProperty("_Occlusionmap"))
		{
			SetKeyword(material, "OCCLUSIONMAP_ON", material.GetTexture("_Occlusionmap") != null);
		}

		if (material.HasProperty("_FlakeIntensity"))
		{
			SetKeyword(material, "FLAKES_ON", material.GetFloat("_FlakeIntensity") > 0.0f);
		}
	
		if (material.HasProperty("_SpecularFactor"))
		{
			SetKeyword(material, "SPECULAR_ON", material.GetFloat("_SpecularFactor") > 0.0f);
		}

		if (material.HasProperty("_CustomCubemap"))
		{
			SetKeyword(material, "REFLECTIONMAP_ON", material.GetTexture("_CustomCubemap") != null);
		}

        if (material.HasProperty("_WireframeBorder"))
        {
            SetKeyword(material, "WIREFRAME_ON", material.GetFloat("_WireframeBorder") < 1.0f);
        }
    }

        static void SetKeyword(Material m, string keyword, bool state)
	{
		if (state)
		{
			m.EnableKeyword(keyword);
		}
		else
		{
			m.DisableKeyword(keyword);
		}
	}
}

} // namespace UnityEditor
