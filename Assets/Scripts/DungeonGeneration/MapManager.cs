using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField]
    GameObject map, dungeonMesh;
    [SerializeField]
    Light directionalLight;

    public bool PlayerTookExit;

    [HideInInspector]
    public EncounterCell CurrentCell;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMapState(bool _activate)
    {
        if (_activate)
        {
            map.SetActive(true);
            dungeonMesh.SetActive(true);
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light != directionalLight)
                {
                    light.enabled = false;
                }
            }
            directionalLight.enabled = true;
            if (CurrentCell == null)
            {
                PlayerInventory.Instance.transform.position = NewDungeonGridGenerator.Instance.transform.position
                    + ((new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x) + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.GridWidth * 0.5f))
                    + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y * 0.5f)
                    + new Vector3(0,1,0);
                return;
            }
            if (PlayerTookExit)
            {
                PlayerInventory.Instance.transform.position = CurrentCell.ExitPos.position;
                PlayerInventory.Instance.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                PlayerInventory.Instance.transform.position = CurrentCell.EntrancePos.position;
                PlayerInventory.Instance.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            Destroy(CurrentCell.gameObject);
            CurrentCell.IsCleared = true;
        }
        else
        {
            directionalLight.enabled = false;
            map.SetActive(false);
            dungeonMesh.SetActive(false);
        }
    }
}
