using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportCtrl : MonoBehaviour
{
	public Material[] materials;
	public Color[] dark_colors;

	Color[] original_colors;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY, OnUpholsteryChanged);

		original_colors = new Color[dark_colors.Length];

		for(int i = 0; i < materials.Length; i++){
			original_colors[i] = materials[i].color;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY, OnUpholsteryChanged);

		for(int i = 0; i < materials.Length; i++){
			materials[i].color = original_colors[i];
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnUpholsteryChanged()
	{
		//Dark interior roof for sport red upholstery
		if(Globals.instance.currentVersion == Globals.Version.SPORT){
			if(Globals.instance.currentUpholstery == Globals.Upholstery.ANTRACITA_CERAMICA_ROJO){
				for(int i = 0; i < materials.Length; i++){
					materials[i].color = dark_colors[i];
				}
			} else{
				for(int i = 0; i < materials.Length; i++){
					materials[i].color = original_colors[i];
				}
			}
		} else{
			for(int i = 0; i < materials.Length; i++){
				materials[i].color = original_colors[i];
			}
		}
	}
}