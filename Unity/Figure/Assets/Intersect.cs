﻿using UnityEngine;
using System.Collections.Generic;

public class Intersect : MonoBehaviour {

    [SerializeField]
    private GameObject plane01;
    [SerializeField]
    private GameObject plane02;

    private MeshFilter mf01;
    private MeshFilter mf02;

	private Vector3 pd01;
	private Vector3 pd02;
	private Vector3 pt;

	private float d01;
	private float d02;

    private List<Vector3> plane01Vertices = new List<Vector3>();
    private List<Vector3> plane02Vertices = new List<Vector3>();

    private Vector3 GetCrossProduct(Vector3 A, Vector3 B, Vector3 C)
    {
        // new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
        // new Vector3(C.x - A.x, C.y - A.y, C.z - A.z);
        var AB = B - A;
        var AC = C - A;

        // float a = (B.y - A.y) * (C.z - A.z) - (C.y - A.y) * (B.z - A.z);
        // float b = (B.z - A.z) * (C.x - A.x) - (C.z - A.z) * (B.x - A.x);
        // float c = (B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y);
        // pd01 = new Vector3(a, b, c);
        return Vector3.Cross(AB, AC);
    }

    private bool IsIntersectionLine()
    {


        return false;
    }

    void Start () {
        mf01 = plane01.GetComponent<MeshFilter>();
        mf02 = plane02.GetComponent<MeshFilter>();

        Vector3[] basePlane01Vertices = mf01.mesh.vertices;
        Vector3[] basePlane02Vertices = mf02.mesh.vertices;

        for (var i = 0; i < basePlane01Vertices.Length; i++)
        {
            plane01Vertices.Add(basePlane01Vertices[i]);
            plane02Vertices.Add(basePlane02Vertices[i]);

        }

        for (var i = 0; i < plane01Vertices.Count - 1; i++)
        {
            GameObject verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            verSp.transform.position = plane01Vertices[i];
            verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            verSp.name = "Plane01Vertex[" + i + "]";

            Debug.Log("Plane01Vertex[" + i + "]" + plane01Vertices[i]);

        }

		for (var i = 0; i < plane02Vertices.Count - 1; i++)
		{
			GameObject verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			verSp.transform.position = plane02Vertices[i];
			verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

			verSp.name = "Plane02Vertex[" + i + "]";

			Debug.Log("Plane02Vertex[" + i + "]" + plane02Vertices[i]);

		}

    }


	void Update () {
        // Plane01
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // PlaneVertex_00 = A
            // PlaneVertex_01 = B
            // PlaneVertex_02 = C

            var A = plane01Vertices[0];
            var B = plane01Vertices[1];
            var C = plane01Vertices[2];

            pd01 = GetCrossProduct(A, B, C);

            // d01 = -(a * A.x + b * A.y + c * A.z);
            d01 = -(pd01.x * A.x + pd01.y * A.y + pd01.z * A.z);

            var verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			verSp.transform.position = pd01;
            verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

			Debug.Log("n = " + pd01);
            Debug.Log("d = " + d01);

        }

        if(Input.GetKeyDown(KeyCode.Space))
		{
            // PlaneVertex_00 = A
            // PlaneVertex_01 = B
            // PlaneVertex_02 = C

			var A = plane02Vertices[0];
            var B = plane02Vertices[1];
            var C = plane02Vertices[2];

            pd02 = GetCrossProduct(A, B, C);

            // d02 = -(a * A.x + b * A.y + c * A.z);
            d02 = -(pd02.x * A.x + pd02.y * A.y + pd02.z * A.z);

            GameObject verSp02 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			verSp02.transform.position = pd02;
            verSp02.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

			Debug.Log("n = " + pd02);
            Debug.Log("d = " + d02);


        }

		if (Input.GetKeyDown(KeyCode.Backspace))
		{

            // pd01 (a1, b1, c1)
            // pd02 (a2, b2, c2)
            // e = (b1 c2 - c1 b2, c1 a2 - a1 c2, a1 b2 - b1 a2)

            //float Ex = pd01.y * pd02.z - pd01.z * pd02.y;
            //float Ey = pd01.z * pd02.x - pd01.x * pd02.z;
            //float Ez = pd01.x * pd02.y - pd01.y * pd02.x;

            //Vector3 e = new Vector3(Ex, Ey, Ez);
            var e = Vector3.Cross(pd01, pd02);

			var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			obj.transform.position = e;
			obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

			Debug.Log("e = " + e);
			if (0 != e.z)
			{
				pt.x = (d01 * pd02.y - d02 * pd01.y) / e.z;
				pt.y = (d01 * pd02.x - d02 * pd01.x) / (-e.z);
				pt.z = 0;

			}
			if (0 != e.y)
			{
				pt.x = (d01 * pd02.z - d02 * pd01.z) / (-e.y);
				pt.y = 0;
				pt.z = (d01 * pd02.x - d02 * pd01.x) / e.y;

			}

			if (0 != e.x)
			{
				pt.x = 0;
				pt.y = (d01 * pd02.z - d02 * pd01.z) / e.x;
				pt.z = (d01 * pd02.y - d02 * pd01.y) / (-e.x);

			}

			var obj02 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			obj02.transform.position = pt;
			obj02.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            Debug.Log("pt = " + pt);

		}

    }
}