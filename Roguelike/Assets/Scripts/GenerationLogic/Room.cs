using UnityEngine;

public class Room : MonoBehaviour
{
    public Mesh[] BlockMeshes;

    private void Start()
    {
        if (BlockMeshes == null || BlockMeshes.Length == 0)
        {
            Debug.LogError("BlockMeshes не настроены для комнаты: " + gameObject.name);
            return;
        }

        foreach (var filter in GetComponentsInChildren<MeshFilter>())
        {
            if (filter.sharedMesh == BlockMeshes[0])
            {
                filter.sharedMesh = BlockMeshes[Random.Range(0, BlockMeshes.Length)];
              //  filter.transform.rotation = Quaternion.Euler(-90, 0, 90 * Random.Range(0, 4));
            }
        }
    }
}
