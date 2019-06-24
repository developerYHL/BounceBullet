using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCtlr : MonoBehaviour {
    [SerializeField]
    private Material denyMaterial;
    private Material initalMeterial;
    private MeshRenderer mMeshRenderer;

    private void Awake()
    {
         mMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
        initalMeterial = mMeshRenderer.material;
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ClientLibrary.BuildingSystem>().canPlace == true)
                GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ClientLibrary.BuildingSystem>().canPlace = false;
            mMeshRenderer.material = denyMaterial;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ClientLibrary.BuildingSystem>().canPlace = true;
            mMeshRenderer.material = initalMeterial;
        }
    }
}
