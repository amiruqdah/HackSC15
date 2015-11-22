using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class Startup{
	static Startup()
	{
		MapGeneration.onCreateBlock += NodePath.generateNode;
	}
}
