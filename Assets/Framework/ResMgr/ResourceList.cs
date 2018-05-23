using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "ScriptableObject/ResourceList", fileName="ResourceList")]
public class ResourceList: ScriptableObject
{
	public Resource[] resources;
}

[System.Serializable]
public class Resource
{
	public ResourceID resName;
	public string fullPath;
}