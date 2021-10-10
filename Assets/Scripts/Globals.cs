using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using Valve.VR;

public class Globals : MonoBehaviour
{
	public static string AppPath = "C:\\DEMOS\\TempVW\\";
	public static bool firstopen = true;
    public static Globals instance;

	public static int INTERIOR = 0;
	public static int EXTERIOR = 1;

	public Material current_material_int_advance_style;

	public Material[] car_materials_body;
	public Material[] car_materials_int_advance_style;
	public Material[] car_materials_roof;
	[ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color[] ambilight_colors;
	public bool bAmbilight = false;
	public ColorFloating a;

	public Sprite paintSprite;
	public float info_distance = 1F;
	public bool isNight = false;
	public bool bCockpitOn = false;
	public bool hasDayNight = false;
	public Camera main_camera;

	public Material hand_mat;

	public enum Models{
		T_ROC,
		TOUAREG
	}
	public Models currentModel;

	public enum CamPositions{
		INTERIOR,
		EXTERIOR
	}
	public enum InternalPositions{
		FRONT_LEFT,
		FRONT_RIGHT,
		REAR_LEFT,
		REAR_RIGHT
	}
	public CamPositions camPosition;
	public InternalPositions internalPosition;
	public bool bCamModeRotation;

	//Car
	public float current_rotation;
	public bool rotate_left;
	public bool rotate_right;

	//Camera
	public bool move_forward;
	public bool move_backwards;
	public bool move_left;
	public bool move_right;
	public bool move_up;
	public bool move_down;

	//Hands
	public enum HandGender{
		MALE,
		FEMALE
	}
	public HandGender current_gender;

	//Version
	public enum Version{
		ADVANCE,
		ADVANCE_STYLE,
		SPORT,
		TOUAREG
	}
	public Version currentVersion;

	//Paint
	public enum Paint
	{
		//T-Roc
		LISO_GRIS_URANO,//0
		LISO_BLANCO_PURO,//1
		LISO_ROJO_FLASH,//2
		METAL_AMARILLO_CURCUMA,//3
		METAL_AZUL_ATLANTICO,//4
		METAL_AZUL_RAVENNA,//5
		METAL_GRIS_INDY,//6
		METAL_MARRON_ROBLE,//7
		METAL_NARANJA_CALATEA,//8
		PERLA_NEGRO_PROFUNDO,//9
		METAL_PLATA_CLARO,//10
		//Touareg
		BLANCO_PURO,//11
		NEGRO,//12
		METAL_AZUL_AGUAMARINA,//13
		METAL_AZUL_ARRECIFE,//14
		NACAR_BLANCO_ORYX,//15
		METAL_DORADO_ARENA,//16
		METAL_MARRON_TAMARINDO,//17
		NEGRO_PROFUNDO_PERLA,//18
		METAL_PLATA_ANTIMONIO//19
	}
	public Paint currentPaintBody;
	public Paint currentPaintRoof;

	//Sunroof
	public bool sunroof;

	//Tires
	public enum Tires
	{
		//T-Roc
		ADVANCE_MONTERO,	//De serie y únicos
		ADVANCE_STYLE_MAYFIELD,
		ADVANCE_STYLE_MAYFIELD_2,	//De serie
		SPORT_GRANGE_HILL,
		SPORT_MONTEGO_BAY,
		SPORT_MONTERO,	//De serie
		//Touareg
		TOUAREG_CONCORDIA,
		TOUAREG_CORDOBA,
		TOUAREG_OSMO,
		TOUAREG_ESPERANCE,
		TOUAREG_TIRANO,
		TOUAREG_MONTERO,
		TOUAREG_NEVADA,
		TOUAREG_BRAGA,
		TOUAREG_BRAGA_BLACK,
		TOUAREG_SUZUKA,
		TOUAREG_SUZUKA_BLACK
	}
	public Tires currentTires;

	//Upholstery
	public enum Upholstery
	{
		//T-Roc
		ANTRACITA_CERAMICA=0,		  //Advance y Advance Style
		CUARCITA_CERAMICA=1,  		  //Advance y Advance Style y Sport
		ANTRACITA_AZUL_RAVENNA=2, 	  //Advance Style
		ANTRACITA_CURCUMA=3,  		  //Advance Style
		ANTRACITA_NARANJA_CALATEA=4,  //Advance Style
		ANTRACITA_CERAMICA_V2=5,      //Sport
		ANTRACITA_CERAMICA_ROJO=6,    //Sport
		//Touareg
		TOUAREG_CUERO_BASIC_CREMA=7,
		TOUAREG_CUERO_BASIC_BLANCO=8,
		TOUAREG_CUERO_BASIC_NEGRO=9,
		TOUAREG_CUERO_BASIC_GRIS=10,
		TOUAREG_CUERO_SABONA_MARRON=11,
		TOUAREG_CUERO_SABONA_GRIS=12,
		TOUAREG_CUERO_R_LINE_GRACE=13,
		TOUAREG_CUERO_R_LINE_VIENA=14,
	}
	public Upholstery currentUpholstery;

	//Luz Ambiente
	public enum AmbiLight
	{
		ROJO,
		NARANJA,
		AMARILLO,
		VERDE,
		VERDE_AZUL,
		AZUL_CLARO,
		AZUL_OSCURO,
		LILA,
		ROSA
	}
	public AmbiLight currentAmbiLight;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
		currentModel = (Models)PlayerPrefs.GetInt("model", 0); //0: T-Roc, 1: Touareg
		currentVersion = (currentModel == Models.TOUAREG) ? Version.TOUAREG : Version.SPORT; 
		currentPaintBody = 	(currentModel == Models.TOUAREG) ? Paint.METAL_AZUL_AGUAMARINA : Paint.LISO_GRIS_URANO;
		currentPaintRoof = 	(currentModel == Models.TOUAREG) ? Paint.METAL_AZUL_AGUAMARINA : Paint.LISO_GRIS_URANO;
		currentTires = 		(currentModel == Models.TOUAREG) ? Tires.TOUAREG_CONCORDIA : Tires.SPORT_MONTERO;
		currentUpholstery = (currentModel == Models.TOUAREG) ? Upholstery.TOUAREG_CUERO_BASIC_NEGRO : Upholstery.ANTRACITA_CERAMICA;
		currentAmbiLight = AmbiLight.ROJO;
		current_gender = (HandGender)PlayerPrefs.GetInt("gender", 0);  //0 male (default), 1 female

		if(VRSettings.loadedDeviceName == "OpenVR"){
			Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
		}
		hasDayNight = false;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
        timeout = -1;
        if (VRSettings.loadedDeviceName == "OpenVR"){
			Valve.VR.OpenVR.System.ResetSeatedZeroPose();
		}
		InputTracking.Recenter();

//		OVRManager.display.RecenterPose();

		FixHandMaterial();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//En el escenario de dia/noche (4) las manos se ven muy brillantes por culpa de la postpo
	void FixHandMaterial()
	{
		int idx = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		Color base_color = hand_mat.GetColor("_BaseColor");
		if(idx == 4){
			hand_mat.SetColor("_BaseColor", new Color(base_color.r, base_color.g, base_color.b, 0.1F));
		}else{
			hand_mat.SetColor("_BaseColor", new Color(base_color.r, base_color.g, base_color.b, 0.9F));
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{        
        if (timeUserOut())
        {
            GameObject.Find("UI Overlay Canvas").GetComponent<MenuController>().LoadScene(0);
        }
		if(Input.GetKeyDown(KeyCode.R)){
            LogUseApp.SaveNewUser();
            if (VRSettings.loadedDeviceName == "OpenVR"){
				Valve.VR.OpenVR.System.ResetSeatedZeroPose();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public static int mod(int a, int b)
	{
		if(a < 0)
			return b-1;
		else
			return a % b;
    }

    static int timeout = -1;
    static int maxtimeout = 10;
    static bool timeUserOut()
    {
		if(Application.isEditor){
			return false;
		}
        if (GetIsUserPresent()) timeout = -1;
        else if (timeout == -1) timeout = getTime();
        if (timeout != -1 && (getTime() - timeout) > maxtimeout) return true;
        return false;
    }
    static int getTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

    static bool GetIsUserPresent()
    {
        return OVRPlugin.userPresent || GetIsUserPresentVIVE();
    }

    static bool GetIsUserPresentVIVE()
    {
        if (OpenVR.System != null)
            return OpenVR.System.GetTrackedDeviceActivityLevel(0)  == EDeviceActivityLevel.k_EDeviceActivityLevel_UserInteraction;
        return false;
    }
}
