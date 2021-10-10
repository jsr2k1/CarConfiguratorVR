using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TransformUI : MonoBehaviour
{
    public GameObject[] Panels;
    public float ScaleFactor = 32.36f;
    public bool ViewInEditor = false;
    public List<Canvas> CanvasRedirect;
    public Camera Display2Cam;
    private CanvasScaler cs;
    public GameObject[] Menu1Display;

	GameObject Panel;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	void Start ()
	{
		Panel = Panels[(int)Globals.instance.currentModel];

        if (Display.displays.Length>1 && Menu1Display != null)
        {
            foreach(GameObject g in Menu1Display)
            {
                g.SetActive(false);
            }
        }
        if (Panel == null) return;

        GameObject ng = Object.Instantiate(Panel);
        ng.transform.SetParent(this.transform, false);
        Canvas ngCanvas = ng.GetComponent<Canvas>();

        if (Display.displays.Length > 1)
        {
            ng.GetComponent<RectTransform>().localPosition = Vector3.zero;
            ng.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 0, 0);
            ng.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }else
        {
            ngCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        CanvasRedirect.Add(ng.GetComponent<Canvas>());
        if (Display.displays.Length > 1) 
        {
            foreach(Canvas c in CanvasRedirect)
            {
                c.targetDisplay = 1;
            }
        }            

        cs = ng.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        cs.scaleFactor = ScaleFactor;

        JoanUtils.myGetComponentsInChildren<InteractionButton>(this.gameObject).ForEach(ib => ib.gameObject.AddComponent<Button>().onClick.AddListener(() => ib.OnPress.Invoke()));
        JoanUtils.myGetComponentsInChildren<Image>(this.gameObject).ForEach(i => i.raycastTarget = true);
        JoanUtils.myGetComponentsInChildren<RectTransform>(this.gameObject).Where(rt => rt.GetComponent<TransformUIAix>() == null).ToList().ForEach(rt => rt.localRotation = Quaternion.Euler(Vector3.zero));
        JoanUtils.myGetComponentsInChildren<TransformUIAix>(this.gameObject).ForEach(t => t.TransformUI());
        //JoanUtils.myGetComponentsInChildren<RectTransform>(this.gameObject).ForEach(rt => rt.localRotation = Quaternion.Euler(new Vector3(Vector3.zero)));
        JoanUtils.myGetComponentsInChildren<TabletMenuNoOptions>(this.gameObject).ForEach(m => m.gameObject.SetActive(false));

        ActiveMenu(); // Desactivat d'inici
    }

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	void Update()
	{
        if (Display.displays.Length > 1) {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                if (mousePos.x < 0 || mousePos.x > Display.displays[0].systemWidth)
                {
                    if (mousePos.x < 0) mousePos.x += Display.displays[1].systemWidth;
                    if (mousePos.x > Display.displays[0].systemWidth) mousePos.x -= Display.displays[0].systemWidth;
                    mousePos.y -= (Display.displays[0].systemHeight - Display.displays[1].systemHeight);

                    Ray ray = Display2Cam.ScreenPointToRay(mousePos);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f);
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.gameObject.GetComponent<Button>() != null)
                        {
                            hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ActiveMenu();
        }
    }

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	public void ActiveMenu()
    {
        if (Display.displays.Length == 1) cs.gameObject.SetActive(!cs.gameObject.activeSelf);
    }
}
