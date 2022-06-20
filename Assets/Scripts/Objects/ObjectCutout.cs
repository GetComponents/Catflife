using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used for the Shader
public class ObjectCutout : MonoBehaviour
{
    private Transform playerTransform;

    [SerializeField]
    private LayerMask objectMask;

    private Camera mainCam;

    private List<Transform> currentHitObjects;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        playerTransform = PlayerInventory.Instance.transform;
        currentHitObjects = new List<Transform>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCam.WorldToViewportPoint(playerTransform.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 rayLength = (playerTransform.position - transform.position);
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, rayLength, rayLength.magnitude, objectMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (!currentHitObjects.Contains(hitObjects[i].transform))
            {
                currentHitObjects.Add(hitObjects[i].transform);
                Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
                for (int j = 0; j < materials.Length; j++)
                {
                    StartCoroutine(SetValues(materials[j], cutoutPos));
                }
            }
        }
        //In case an fewer Objects were hit by the raycast than last frame,
        //Checks for each Object that was hit before and remove it from the Coroutine if they are no longer hit
        if (hitObjects.Length < currentHitObjects.Count)
        {
            List<Transform> objectsToRemove = new List<Transform>();
            for (int i = 0; i < currentHitObjects.Count; i++)
            {
                bool containsObject = false;
                for (int j = 0; j < hitObjects.Length; j++)
                {
                    if (currentHitObjects[i].transform == hitObjects[j].transform)
                    {
                        containsObject = true;
                        break;
                    }
                }
                if (!containsObject)
                {
                    Material[] materials = currentHitObjects[i].transform.GetComponent<Renderer>().materials;
                    for (int j = 0; j < materials.Length; j++)
                    {
                        objectsToRemove.Add(currentHitObjects[i]);
                        //TODO remove the coroutine of the missing Object
                        ResetValues(materials[j]);
                    }
                }
            }
            foreach (Transform raycastHit in objectsToRemove)
            {
                if (currentHitObjects.Contains(raycastHit))
                {
                    currentHitObjects.Remove(raycastHit);
                }
            }
        }
    }


    private void ResetValues(Material _material)
    {
        _material.SetVector("_CutoutPos", new Vector2(0, 0));
        _material.SetFloat("_CutoutSize", 0);
        _material.SetFloat("_FalloffSize", 0.05f);
    }


    /// <summary>
    /// Sets the values of the information in the Shader in an Updatetype Coroutine
    /// </summary>
    /// <param name="_material"></param>
    /// <param name="_cutoutPos"></param>
    /// <returns></returns>
    IEnumerator SetValues(Material _material, Vector2 _cutoutPos)
    {
        for (float i = 0.5f; i < 1f; i += 0.1f)
        {
            _material.SetVector("_CutoutPos", _cutoutPos);
            _material.SetFloat("_CutoutSize", Mathf.Lerp(0, 0.1f, i));
            _material.SetFloat("_FalloffSize", 0.05f);
            yield return new WaitForEndOfFrame();
        }
    }
}
