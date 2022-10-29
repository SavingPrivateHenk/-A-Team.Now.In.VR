using UnityEngine;

public class ColorChanger
{
    public static void FindMaterials(Transform entity, Material material)
    {
        for (var i = 0; i < entity.childCount; i++)
        {
            var child = entity.GetChild(i);
            if (child.GetComponent<MeshRenderer>() == null)
            {
                FindMaterials(child, material);
                continue;
            }
            
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}
