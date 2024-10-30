using System;
using System. Collections;
using System. Collections. Generic;
using UnityEngine;

public class Generation: MonoBehaviour
{
    void Start ()
    {
        FichierOff testOff = new FichierOff ("buddha");
		testOff. modeliser (Vector3. zero);
		testOff. exporter ("test");
    }

    public static void generer (string nom, ref List <Vector3> sommets, ref List <int> triangles, ref Vector3 [] normales)
	{
		Mesh modele = new Mesh ();
		modele. vertices  = sommets. ToArray ();
		modele. triangles = triangles. ToArray ();
		modele. normals   = normales;
		
		GameObject objet = new GameObject (nom, typeof (MeshRenderer), typeof (MeshFilter));
		objet. GetComponent <MeshFilter> (). mesh = modele;
	}
	
	public static Vector3 somme (List <Vector3> vecteurs)
	{
		Vector3 s = Vector3. zero;
		foreach (Vector3 v in vecteurs)
		{
			s += v;
		}
		return s;
	}
	
	public static float coordonneeMax (List <Vector3> vecteurs)
	{
		float m = 0;
		foreach (Vector3 v in vecteurs)
		{
			m = Math. Max (m, Math. Abs (v. x));
			m = Math. Max (m, Math. Abs (v. y));
			m = Math. Max (m, Math. Abs (v. z));
		}
		return m;
	}
	
	public static Vector3 produitVectoriel (Vector3 vecteurA, Vector3 vecteurB)
	{
		return new Vector3
		(
			vecteurA. y * vecteurB. z - vecteurA. z * vecteurB. y,
			vecteurA. z * vecteurB. x - vecteurA. x * vecteurB. z,
			vecteurA. x * vecteurB. y - vecteurA. y * vecteurB. x
		);
	}
}
