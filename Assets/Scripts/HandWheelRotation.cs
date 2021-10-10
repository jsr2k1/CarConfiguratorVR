using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandWheelRotation : MonoBehaviour {

    // Use this for initialization    
    public GameObject Wheel;
    public GameObject DebugCube;
    public Vector3 IRot;
    float iniAng;
    float retAngVel = 2;
    void Start() {
        IRot = Wheel.transform.localRotation.eulerAngles;
        currAng = IRot.x;
        iniAng = currAng;
        /*Vector3 pos = new Vector3(Wheel.transform.position.x, Wheel.transform.position.y, Wheel.transform.position.z);
        Vector3 sca = Wheel.transform.lossyScale;
        Quaternion rot = Wheel.transform.rotation;

        Wheel.transform.parent = this.transform;

        Debug.Log(pos +" new pos "+ Wheel.transform.position);
        Wheel.transform.position = pos;
        //SetGlobalScale(Wheel.transform, sca);
        Wheel.transform.rotation = rot;
        */
    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    // Update is called once per frame
    public float pangle;
    float angle;
    float currAng;
    void Update() {
        foreach (ColInfo c in Cols.ToList())
        {
            if (c.Obj == null)
            {
                Cols.Remove(c);
            }
        }
        if (getCurrCol() != null)
        {
            retAngVel = 2;

            angle = getAngle(getCurrCol().Obj.gameObject);

            //DebugCube.transform.position = Wheel.transform.position;
            //DebugCube.transform.localPosition += new Vector3(0, 1, 0) * Mathf.Sin(angle / Mathf.Rad2Deg) * 0.2f;
            //DebugCube.transform.localPosition += new Vector3(0, 0, 1) * Mathf.Cos(angle / Mathf.Rad2Deg) * 0.2f;

            currAng = (currAng + (getCurrCol().angIni - angle))%360;
            Wheel.transform.localRotation = Quaternion.Euler(currAng, IRot.y, IRot.z);            

            foreach (ColInfo c in Cols)
            {
                c.angIni = getAngle(c.Obj.gameObject);
            }
        }
        else
        {
            if (Mathf.Abs(currAng - iniAng) <= retAngVel)
            {
                Wheel.transform.localRotation = Quaternion.Euler(iniAng, IRot.y, IRot.z);
            }
            else
            {
                float dir = Mathf.Abs(currAng - iniAng) < Mathf.Abs(currAng - (iniAng + 360)) ? (currAng > iniAng? -1: 1) : (currAng > 180 ? 1 : -1);

                currAng = (currAng + (dir * retAngVel)) % 360;
                Wheel.transform.localRotation = Quaternion.Euler(currAng, IRot.y, IRot.z);
                retAngVel += 0.2f;
            }            
        }
    }
    public float getAngle(GameObject go)
    {
        DebugCube.transform.position = go.transform.position;
        go = DebugCube;
        Vector3 dir = go.transform.position - Wheel.transform.position;
        dir = go.transform.InverseTransformDirection(dir);
        return Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
    }

    class ColInfo{
        public float angIni;
        public Collider Obj;

        public ColInfo(float angIni, Collider Obj)
        {
            this.angIni = angIni;
            this.Obj = Obj;
        }
    }

    List<ColInfo> Cols = new List<ColInfo>();

    ColInfo getCurrCol()
    {
        if (Cols.Count > 0) return Cols[0];
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        Cols.Add(new ColInfo(getAngle(other.gameObject), other));        
        /*if (OCol == null)
        {            
            OCol = other;

            IRot.x = Wheel.transform.localRotation.eulerAngles.x;
            Iangle = getAngle(OCol.gameObject);
        }*/
    }
    void OnTriggerExit(Collider other)
    {
        foreach(ColInfo c in Cols.ToList())
        {
            if (c.Obj == other)
            {
                Cols.Remove(c);
            }
        }
    }
}
