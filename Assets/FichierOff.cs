using System;
using System. Collections;
using System. Collections. Generic;
using System. IO;
using UnityEngine;


public class FichierOff
{
	private const string chemin = @"C:\Users\ecausse1\Downloads\";
	private int nbSommets;
	private int nbFaces;
	private int nbAretes;
	private List <Vector3> sommets;
	private List <List <int>> polygones;
	private List <int> triangles;
	private List <int> nbsSegments;
	private Vector3 [] normales;
	private string nom;


	public FichierOff (string nomFichier)
	{
		string [] elements;
		this. sommets     = new List <Vector3> ();
		this. polygones   = new List <List <int>> ();
		this. nbsSegments = new List <int> ();
		int numLigne = 0;
		foreach (string ligne in File. ReadLines (FichierOff. chemin + nomFichier + ".off"))
		{
			elements = ligne. Split (new char [1] {' '});
			if (numLigne == 0);
			
			// Chargement de la numération
			else if (numLigne == 1)
			{
				nbSommets = Convert. ToInt32 (elements [0]);
				nbFaces   = Convert. ToInt32 (elements [1]);
				nbAretes  = Convert. ToInt32 (elements [2]);
			}
			
			// Chargement des sommets
			else if (numLigne < this. nbSommets + 2)
			{
				this. sommets. Add (new Vector3
				(
					Convert. ToSingle (elements [0]),
					Convert. ToSingle (elements [1]),
					Convert. ToSingle (elements [2])
				));
			}
			
			// Chargement des faces
			else if (numLigne < this. nbFaces + this. nbSommets + 2)
			{
				int nbPoints = Convert. ToInt32 (elements [0]);
				this. nbsSegments. Add (nbPoints);
				List <int> polygone = new List <int> ();
				int point;
				for (int indicePoint = 1; indicePoint <= nbPoints; indicePoint ++)
				{
					point = Convert. ToInt32 (elements [indicePoint]);
					polygone. Add (point);
				}
				this. polygones. Add (polygone);
			}
			
			numLigne ++;
		}
		this. nom = nomFichier;
	}
	
	
	private void normaliser ()
	{
		// Moyenne ← (0, 0, 0)
		Vector3 barycentre = Generation. somme (this. sommets) / this. sommets. Count;
		for (int indice = 0; indice < this. nbSommets; indice ++)
		{
			this. sommets [indice] -= barycentre;
		}
		
		// Maximum ← 1
		float maxi = Generation. coordonneeMax (this. sommets);
		for (int indice = 0; indice < this. nbSommets; indice ++)
		{
			this. sommets [indice] /= maxi;
		}
	}
	
	
	private void genererNormales ()
	{
		int
			indexA,
			indexB,
			indexC
		;
		Vector3
			A, B, C,
			areteAB,
			areteAC,
			normale
		;
		List <int> polygone;
		this. normales = new Vector3 [this. nbSommets];
		for (int indice = 0; indice < this. nbFaces; indice ++)
		{
			polygone = polygones [indice];
			indexA = polygone [0];
			indexB = polygone [1];
			indexC = polygone [2];
			A = this. sommets [indexA];
			B = this. sommets [indexB];
			C = this. sommets [indexC];
			areteAB = B - A;
			areteAC = C - A;
			normale = Generation. produitVectoriel (areteAB, areteAC);
			this. normales [indexA] += normale;
			this. normales [indexB] += normale;
			this. normales [indexC] += normale;
		}
	}
	
	
	private void genererTriangles ()
	{
		this. triangles = new List <int> ();
		foreach (List <int> polygone in this. polygones)
		{
			foreach (int point in polygone)
			{
				this. triangles. Add (point);
			}
		}
	}
	
	
	public void modeliser (Vector3 translation)
	{
		this. normaliser ();
		this. genererTriangles ();
		this. genererNormales ();
		for (int indice = 0; indice < this. sommets. Count; indice ++)
		{
			this. sommets [indice] += translation;
		}
		
		Generation. generer (this. nom, ref this. sommets, ref this. triangles, ref this. normales);
	}
	
	
	public void exporter (string nomFichier)
	{
		string sortie = "OFF\n";
		
		// Écriture de la numération
		sortie +=
			this. nbSommets
			+ " "
			+ this. nbFaces
			+ " 0\n"
		;
		
		// Écriture des sommets
		foreach (Vector3 sommet in this. sommets)
		{
			sortie +=
				sommet. x
				+ " "
				+ sommet. y
				+ " "
				+ sommet. z
				+ "\n"
			;
		}
		
		// Écriture des triangles
		foreach (List <int> polygone in this. polygones)
		{
			sortie += polygone. Count;
			foreach (int point in polygone)
			{
				sortie +=
					" "
					+ point
				;
			}
			sortie += "\n";
		}
		
		File. WriteAllText (FichierOff. chemin + nomFichier + ".off", sortie);
	}
}
