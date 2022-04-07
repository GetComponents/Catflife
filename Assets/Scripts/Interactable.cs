using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(Interactable))]
[System.Serializable]
public class InteractableCI : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable script = (Interactable)target;

        script.Name = EditorGUILayout.TextField("Object Name", script.Name);
        script.myAction = (EActionType)EditorGUILayout.EnumPopup("My Action", script.myAction);
        script.costOfInteraction = EditorGUILayout.IntField("Interaction Cost", script.costOfInteraction);

        if (script.myAction == EActionType.UNPACK)
        {
            script.objectToUnpack = (GameObject)EditorGUILayout.ObjectField("Contained Object", script.objectToUnpack, typeof(GameObject), true);
            script.objectPosition = (Transform)EditorGUILayout.ObjectField("Final Position", script.objectPosition, typeof(Transform), true);
            script.UnlockContent = EditorGUILayout.Toggle("Unlocks Something", script.UnlockContent);
            script.timeForObjectMovement = EditorGUILayout.FloatField("Time for Movement (seconds)", script.timeForObjectMovement);
        }
        else if (script.myAction == EActionType.WATER)
        {
            script.myUpgradeType = (ETypeOfUpgrade)EditorGUILayout.EnumPopup("Type of Upgrade", script.myUpgradeType);
        }
        else if (script.myAction == EActionType.SLEEP)
        {

        }

        EditorUtility.SetDirty(script);
    }
}
#endif
public enum ETypeOfUpgrade
{
    NONE,
    ATTACK,
    HP,
    MANA,
    SPEED
}

public enum EActionType
{
    NONE,
    UNPACK,
    WATER,
    SLEEP
}


public class Interactable : MonoBehaviour
{


    public ETypeOfUpgrade myUpgradeType;

    public EActionType myAction;

    public string Name;

    [SerializeField]
    GameObject interactionCanvas;

    [SerializeField]
    TextMeshProUGUI costUI, NameUI;

    public int costOfInteraction;

    PlayerInventory player;
    bool isUnpacking;

    public bool UnlockContent;

    public GameObject objectToUnpack;

    public Transform objectPosition;

    public float timeForObjectMovement;

    private void Start()
    {
        player = PlayerInventory.Instance;
        ChangeUIText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnableUI(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            EnableUI(false);
        }
    }

    private void ChangeUIText()
    {
        costUI.text = costOfInteraction.ToString();
        switch (myAction)
        {
            case EActionType.NONE:
                break;
            case EActionType.UNPACK:
                NameUI.text = "Unpack " + Name;
                break;
            case EActionType.WATER:
                NameUI.text = "Water " + Name;
                break;
            case EActionType.SLEEP:
                NameUI.text = "Sleep in " + Name;
                break;
            default:
                break;
        }
    }

    public void StartInteraction()
    {
        if (costOfInteraction <= PlayerInventory.Instance.Energy)
        {
            player = PlayerInventory.Instance;
            processInteraction();
            player.Energy -= costOfInteraction;
        }
    }

    private void EnableUI(bool enable)
    {
        if (!isUnpacking)
            interactionCanvas.SetActive(enable);
    }

    private void processInteraction()
    {
        switch (myAction)
        {
            case EActionType.NONE:
                break;
            case EActionType.UNPACK:
                UnpackItem();
                break;
            case EActionType.WATER:
                UpgradeStat();
                break;
            case EActionType.SLEEP:
                Sleep();
                break;
            default:
                break;
        }
    }

    private void UpgradeStat()
    {
        switch (myUpgradeType)
        {
            case ETypeOfUpgrade.ATTACK:
                player.AttackUpgrades++;
                Debug.Log($"You Feel Stronger ({player.AttackUpgrades})");
                break;
            case ETypeOfUpgrade.HP:
                player.HPUpgrades++;
                Debug.Log($"You Feel Healthier ({player.HPUpgrades})");
                break;
            case ETypeOfUpgrade.MANA:
                player.ManaUpgrades++;
                Debug.Log($"You Feel More Resiliant ({player.ManaUpgrades})");
                break;
            case ETypeOfUpgrade.SPEED:
                player.DashUpgrades++;
                Debug.Log($"You Feel More Energetic ({player.DashUpgrades})");
                break;
        }
    }

    private void UnpackItem()
    {
        Destroy(interactionCanvas);
        isUnpacking = true;
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        GameObject movingObject = Instantiate(objectToUnpack, transform.position, transform.rotation);
        for (float i = 0; i <= 1; i += 0.01f)
        {
            movingObject.transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, objectPosition.position.x, i),
                Mathf.Lerp(transform.position.y, objectPosition.position.y, i),
                Mathf.Lerp(transform.position.z, objectPosition.position.z, i));
            movingObject.transform.eulerAngles = new Vector3(
                Mathf.Lerp(transform.eulerAngles.x, objectPosition.eulerAngles.x, i),
                Mathf.Lerp(transform.eulerAngles.y, objectPosition.eulerAngles.y, i),
                Mathf.Lerp(transform.eulerAngles.z, objectPosition.eulerAngles.z, i));
            yield return new WaitForSeconds((float)(timeForObjectMovement) / 100);
        }
        if (UnlockContent)
        {
            GetComponent<PlayerUnlock>().UnlockMyContent();
        }
        Destroy(gameObject);
    }

    private void Sleep()
    {
        SceneManager.LoadSceneAsync("EncounterSelection");
        PlayerController.Instance.HealFull();
    }
}
