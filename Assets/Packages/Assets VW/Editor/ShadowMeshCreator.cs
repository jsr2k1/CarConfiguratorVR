using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using GCContentLibrary;
using GCContentLibrary.MaterialHandling;

public class ShadowMeshCreator 
{
	private static GameObject _origin = null;

	struct HitData
    {
        public Vector3 hitPoint;
        public Vector3 hitDir;
    }

//    Dictionary<string, GameObject> _collisionDict = new Dictionary<string, GameObject>();
    static Dictionary<Vector2, HitData> _points = new Dictionary<Vector2, HitData>();

    static int s_reduce = 4;

	static public GameObject CreateShadowMeshObject(GameObject root) 
	{
        GameObject shadow = null;

		_origin = root;
		
		if (_origin != null)
		{
			CreateColliders();
            shadow = CreateShadowMesh();
		}

        return shadow;		
	}

    static void CreateColliders()
	{
		foreach (MeshFilter meshObj in _origin.GetComponentsInChildren<MeshFilter>())
		{
			GameObject obj = meshObj.gameObject;

			MeshCollider collider = obj.GetComponent<MeshCollider>();
			if (collider == null)
			{
//				Debug.Log("No Meshcollider for Mesh: " + meshObj.name + " Vertices: " + meshObj.sharedMesh.vertexCount);

				collider = obj.AddComponent<MeshCollider>();
			}

			collider.sharedMesh = meshObj.sharedMesh;
		}
	}
	
	static GameObject CreateShadowMesh()
	{
		if (_origin == null)
		{
			return null;
		}

		float castFromDistance = 4.0f;
        float angleStep = 2.0f;
 //		float angleStep = 5.0f;
        
        Bounds bounds = GetBounds(_origin);
        Debug.Log("bounds: " + bounds);

//        float castToDistance = 0.5f;
        float castToDistance = ((bounds.extents.x + bounds.extents.y + bounds.extents.z) / 3.0f) * 0.25f;
        Debug.Log("castto: " + castToDistance);

        //		bounds.Encapsulate(new Vector3(bounds.center.x, -2.0f, bounds.center.z));

        //		Vector3 originPosition = _origin.transform.position;
        Vector3 originPosition = bounds.center;
//		originPosition.y = bounds.min.y;

		RaycastHit hitInfo;

		Debug.Log ("origin: " + originPosition);

//		_collisionDict.Clear();

        _points.Clear();

		for (float angleZ = 0.0f; angleZ <= 360.0f; angleZ += angleStep)
		{
			for (float angleX = 0.0f; angleX <= 360.0f; angleX += angleStep)
//			for (float angleX = 0.0f; angleX <= 5.0f; angleX += angleStep)
			{
				Vector3 spherePoint = getSpherePoint(angleX, angleZ);

                Vector3 castStartPosition = originPosition + (spherePoint * castFromDistance);
				Vector3 castEndPosition = originPosition + (spherePoint * castToDistance);
				Vector3 castDir = castEndPosition - castStartPosition;

                // debug show castpoints
                Debug.DrawLine(castStartPosition, castStartPosition + new Vector3(0.01f, 0.01f, 0.01f), Color.red, 100000.0f, false);
                //				Debug.Log ("start: " + castStartPosition);
                //				Debug.Log ("end: " + castEndPosition);
                //				Debug.Log ("dir: " + castDir);

                bool result = Physics.Raycast(castStartPosition, castDir.normalized, out hitInfo, castDir.magnitude);

				if (result == true)
				{
					Vector3 hitPoint = castStartPosition + (castDir.normalized * hitInfo.distance);

					float boundsDistance = 100.0f;
					Ray ray = new Ray(castStartPosition, castDir.normalized);

					if (bounds.IntersectRay(ray, out boundsDistance))
				 	{
						Vector3 boundsPoint = castStartPosition + (castDir.normalized * boundsDistance);

						Vector3 distance = boundsPoint - hitPoint;
						boundsDistance = distance.magnitude;

//						Debug.Log ("dtb: " + distanceToBounds);
					}

//					_collisionDict[hitInfo.transform.name] = hitInfo.transform.gameObject;

                    insertHitPoint(angleX, angleZ, angleStep, hitPoint, -castDir.normalized);
//                    Debug.DrawLine(hitPoint, hitPoint + (castDir.normalized * 0.01f), Color.green, 1000000.0f, false);

                    //					Debug.DrawRay(castPosition, -castDir * hitInfo.distance, Color.green);
                    //					Debug.Log("hit: " + hitPoint);
//                    float duration = 1000.0f;
					if (boundsDistance > 2.0f)
					{
//						Debug.DrawRay(hitPoint, -castDir.normalized * 0.05f, Color.cyan);
					}
					else
					{
//						Debug.DrawRay(hitPoint, -castDir.normalized * 0.05f, Color.green, duration);
					}
//					Debug.DrawRay(castStartPosition, -castDir.normalized * 0.1f, Color.red);					
				}
				else 
				{
//					Debug.DrawRay(castPosition, -castDir * hitInfo.distance, Color.green);
				}

//				return;
			}
		}

        Debug.Log("Hit Points: " + _points.Count);

//        return null;
       /*
        int count = 0;
        foreach (var pair in _points)
        {
            if (count < 10)
            {
                Debug.Log(pair.Value.hitDir);
            }
            count++;
        }
*/
//        return;

        completeHitPoints(angleStep);

        Debug.Log("Complete Points: " + _points.Count);

//        return null;

/*
        for (float angleZ = 0.0f; angleZ <= 360.0f; angleZ += angleStep)
        {
            for (float angleX = 0.0f; angleX <= 360.0f; angleX += angleStep)
            {
                Vector2 angleCoord = new Vector2(validateAngle(angleX), validateAngle(angleZ));

                if (_points.ContainsKey(angleCoord))
                {
                    // interpolate
                    int neighbours = 0;

                    Vector3 right = findClosestNeighbour(angleCoord, 1, 0, angleStep);
                    if (right != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 down = findClosestNeighbour(angleCoord, 0, 1, angleStep);
                    if (down != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 downright = findClosestNeighbour(angleCoord, 1, 1, angleStep);
                    if (downright != Vector3.zero)
                    {
                        neighbours++;
                    }

                    if (neighbours == 3)
                    {
                        Debug.DrawLine(_points[angleCoord], right, Color.green, 1000000.0f, false);
                        Debug.DrawLine(_points[angleCoord], down, Color.blue, 1000000.0f, false);
                        Debug.DrawLine(right, downright, Color.red, 1000000.0f, false);
                        Debug.DrawLine(down, downright, Color.yellow, 1000000.0f, false);
                    }
                }
            }
        }
*/
        return createMesh(angleStep);
    }

    static GameObject createMesh(float angleStep)
    {
        int faces = 0;

        // create mesh
        KrablMesh.MeshEdges kmesh = new KrablMesh.MeshEdges();

        for (float angleZ = 0.0f; angleZ <= 360.0f; angleZ += angleStep)
        {
            for (float angleX = 0.0f; angleX <= 360.0f; angleX += angleStep)
            {
                Vector2 angleCoord = new Vector2(validateAngle(angleX), validateAngle(angleZ));

                if (_points.ContainsKey(angleCoord))
                {
                    // interpolate
                    int neighbours = 0;

                    Vector3 rightDir = Vector3.zero;
                    Vector3 right = findClosestNeighbour(angleCoord, 1, 0, angleStep, ref rightDir);
                    if (right != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 downDir = Vector3.zero;
                    Vector3 down = findClosestNeighbour(angleCoord, 0, 1, angleStep, ref downDir);
                    if (down != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 downRightDir = Vector3.zero;
                    Vector3 downright = findClosestNeighbour(angleCoord, 1, 1, angleStep, ref downRightDir);
                    if (downright != Vector3.zero)
                    {
                        neighbours++;
                    }

                    if (neighbours == 3)
                    {
                        int index1 = kmesh.AddVertex(_points[angleCoord].hitPoint);
                        int index2 = kmesh.AddVertex(right);
                        int index3 = kmesh.AddVertex(downright);
                        int index4 = kmesh.AddVertex(down);

                        KrablMesh.Face face = new KrablMesh.Face(index1, index2, index3, index4);
                        face.normal = _points[angleCoord].hitDir;
                        face.vertexNormal[0] = _points[angleCoord].hitDir;
                        face.vertexNormal[1] = rightDir;
                        face.vertexNormal[2] = downRightDir;
                        face.vertexNormal[3] = downDir;
//                        int index = kmesh.AddFace(face);

//                        kmesh.CalculateFaceNormal(index);
                        /*
                                                KrablMesh.Face face = new KrablMesh.Face(index1, index2, index3);
                                                kmesh.AddFace(face);
                                                face = new KrablMesh.Face(index3, index4, index1);
                                                kmesh.AddFace(face);
                        */
                        faces++;
                    }
                }
            }
        }


        KrablMesh.Ops.RemoveDoubleVertices(kmesh);
        kmesh.GenerateEdgeList();
        kmesh.CalculateEdgeLinkedFaces();
//        kmesh.topology = KrablMesh.MeshTopology.Quads;
//        kmesh.CalculateFaceNormals();
//        kmesh.CalculateVertexNormals();
        kmesh.InvalidateDegenerateFaces();
        KrablMesh.CreaseDetect.MarkCreasesFromFaceNormals(kmesh);

        GameObject meshObj = new GameObject("ShadowMesh");
//        MeshRenderer renderer = meshObj.AddComponent<MeshRenderer>();
        MeshFilter filter = meshObj.AddComponent<MeshFilter>();
        meshObj.transform.Translate(new Vector3(1, 0, 0));

        Mesh resultMesh = new Mesh();
//        KrablMesh.ImportExport.MeshEdgesToUnityMesh(kmesh, resultMesh);

        KrablMesh.Simplify sim = new KrablMesh.Simplify();
        KrablMesh.SimplifyParameters simpars = new KrablMesh.SimplifyParameters();

//        KrablMesh.ImportExport.UnityMeshToMeshEdges(unityMesh, kmesh);
        Debug.Log(faces);
        int faceCount = kmesh.faceCount();
        Debug.Log(faceCount);
        simpars.targetFaceCount = faceCount / s_reduce;
        simpars.recalculateVertexPositions = true;
        simpars.checkTopology = true;
        simpars.maxEdgesPerVertex = 50;

        sim.Execute(ref kmesh, simpars);

        KrablMesh.ImportExport.MeshEdgesToUnityMesh(kmesh, resultMesh);

//        KrablMeshUtility.SmoothShadeMesh(resultMesh);

//            int reduce = 1;
//        filter.mesh = ModelImportChecker.SimplifyMesh(resultMesh, resultMesh.triangles.Length / reduce);

        filter.mesh = resultMesh;

        return meshObj;
    }

    static void insertHitPoint(float angleX, float angleZ, float angleStep, Vector3 hitPoint, Vector3 castDir)
    {
        Vector2 angleCoord = new Vector2(validateAngle(angleX), validateAngle(angleZ));

        HitData data = new HitData();
        data.hitPoint = hitPoint;
        data.hitDir = castDir;

        _points[angleCoord] = data;
    }

    static float validateAngle(float angle)
    {
        if (angle < 0.0f)
        {
            angle += 360.0f;
        }
        else if (angle > 360.0f)
        {
            angle -= 360.0f;
        }

        return angle;
    }

    // function to add missing hitpoints (interpolate from neighbours)
    static void completeHitPoints(float angleStep)
    {
        for (float angleZ = 0.0f; angleZ <= 360.0f; angleZ += angleStep)
        {
            for (float angleX = 0.0f; angleX <= 360.0f; angleX += angleStep)
            {
                Vector2 angleCoord = new Vector2(validateAngle(angleX), validateAngle(angleZ));

                if (_points.ContainsKey(angleCoord) == false)
                {
                    // interpolate
                    int neighbours = 0;

                    Vector3 leftDir = Vector3.zero;
                    Vector3 left = findClosestNeighbour(angleCoord , -1, 0, angleStep, ref leftDir);
                    if (left != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 rightDir = Vector3.zero;
                    Vector3 right = findClosestNeighbour(angleCoord, 1, 0, angleStep, ref rightDir);
                    if (right != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 upDir = Vector3.zero;
                    Vector3 up = findClosestNeighbour(angleCoord, 0, -1, angleStep, ref upDir);
                    if (up != Vector3.zero)
                    {
                        neighbours++;
                    }
                    Vector3 downDir = Vector3.zero;
                    Vector3 down = findClosestNeighbour(angleCoord, 0, 1, angleStep, ref downDir);
                    if (down != Vector3.zero)
                    {
                        neighbours++;
                    }

                    if (neighbours > 0)
                    {
                        Vector3 point;
                        point = (left + right + up + down) / (float)neighbours;
                        Vector3 dir;
                        dir = (leftDir + rightDir + upDir + downDir) / (float)neighbours;

                        HitData data = new HitData();
                        data.hitPoint = point;
                        data.hitDir =  dir;

                        _points[angleCoord] = data;
                    }
                    else
                    {
                        HitData data = new HitData();
                        data.hitPoint = Vector3.zero;
                        data.hitDir = Vector3.up;

                        _points[angleCoord] = data;
                    }
                }
            }
        }
    }

    static Vector3 findClosestNeighbour(Vector2 angleCoord, int xDir, int yDir, float angleStep, ref Vector3 hitDir)
    {
        Vector3 result = Vector3.zero;

        Vector2 step = new Vector2(xDir * angleStep, yDir * angleStep);

        bool foundNeighbour = false;

        Vector2 searchAngle = angleCoord;

        int iterations = 0;

        while (foundNeighbour == false)
        {
            searchAngle += step;

            searchAngle.x = validateAngle(searchAngle.x);
            searchAngle.y = validateAngle(searchAngle.y);

            if (_points.ContainsKey(searchAngle))
            {
                result = _points[searchAngle].hitPoint;
                hitDir = _points[searchAngle].hitDir;
                foundNeighbour = true;
            }

            if (searchAngle == angleCoord)
            {
                // no result
                foundNeighbour = true;
            }

            iterations++;

            if (iterations == 10)
            {
                // no result
                foundNeighbour = true;
            }
        }

        return result;
    }

    static Vector3 getSpherePoint(float angleX, float angleZ)
	{
		float radX = Mathf.Deg2Rad * angleX;
		float radZ = Mathf.Deg2Rad * angleZ;
		float radZPlus = Mathf.Deg2Rad * (angleZ + 90.0f);
		float deltaX = Mathf.Sin(radX) * Mathf.Sin(radZPlus);
		float deltaY = Mathf.Cos(radX) * Mathf.Sin(radZPlus);
		
		float deltaZ = Mathf.Sin(radZ);
	
		return new Vector3(deltaX, deltaZ, deltaY);
	}

	static Bounds GetBounds(GameObject obj)
	{
		Bounds combinedBounds = new Bounds();
		
		bool gotBounds = false;
		
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer renderer in renderers)
		{
			if (gotBounds == false)
			{
				combinedBounds = renderer.bounds;
				gotBounds = true;

                Debug.Log(renderer.name + " bounds: " + renderer.bounds);
			}
			else 
			{
                Debug.Log(renderer.name + " bounds: " + renderer.bounds);

                combinedBounds.Encapsulate(renderer.bounds);
			}
		}
		
		return combinedBounds;
	}
}

	