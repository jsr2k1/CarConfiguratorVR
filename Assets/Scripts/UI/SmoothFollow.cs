using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform Follow;
    public Transform Object;
    public Vector3 LocalMenuPosition;

    void Update()
	{
        //this.transform.position = Vector3.Lerp(this.transform.position, Follow.position, 0.5f);
        //this.transform.rotation = Quaternion.Euler(-87.3f, -205.2f, 6.95f);
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Follow.rotation, 0.05f);
		if(Camera.main)
			Object.LookAt(Camera.main.transform);
        //transform.rotation *= Quaternion.Euler(DiffRotation);
    }
    public void rePosition()
    {
		if(Camera.main){
			//this.transform.localPosition += Vector3.up * 0.08f;
	        Vector3 newPos = Follow.position + (Vector3.up * 0.2f);
	        float rotX = Object.transform.rotation.eulerAngles.x;
	        if (rotX > 180) rotX = rotX - 360;        
	        newPos += (Camera.main.transform.position - this.transform.position).normalized * (rotX * 0.002f);

	        //Debug.Log("ROT "+Object.transform.rotation.eulerAngles.x);
	        this.transform.position = Vector3.Lerp(this.transform.position, newPos, 0.5f);
	        //Vector3 a = Quaternion.AngleAxis(-45, Vector3.up) * Camera.main.transform.forward;
	        //this.transform.localPosition += Quaternion.AngleAxis(-90, Vector3.up)*(Camera.main.transform.position - this.transform.position).normalized*0.1f;
	        //this.transform.localPosition += (Camera.main.transform.position - this.transform.position).normalized * -0.04f;        
	        //Object.localPosition = LocalMenuPosition;
	        //Object.localPosition = new Vector3(0, 0.04f, 0.03f);
		}
    }
}
