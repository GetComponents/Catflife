using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
//[CanEditMultipleObjects]
//[CustomEditor(typeof(Interactable))]
//[System.Serializable]
//public class InteractableCI : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        Interactable script = (Interactable)target;

//        script.Name = EditorGUILayout.TextField("Object Name", script.Name);
//        script.myAction = (EActionType)EditorGUILayout.EnumPopup("My Action", script.myAction);
//        script.costOfInteraction = EditorGUILayout.IntField("Interaction Cost", script.costOfInteraction);

//        if (script.myAction == EActionType.UNPACK)
//        {
//            script.objectToUnpack = (GameObject)EditorGUILayout.ObjectField("Contained Object", script.objectToUnpack, typeof(GameObject), true);
//            script.objectPosition = (Transform)EditorGUILayout.ObjectField("Final Position", script.objectPosition, typeof(Transform), true);
//            script.UnlockContent = EditorGUILayout.Toggle("Unlocks Something", script.UnlockContent);
//            script.timeForObjectMovement = EditorGUILayout.FloatField("Time for Movement (seconds)", script.timeForObjectMovement);
//        }
//        else if (script.myAction == EActionType.WATER)
//        {
//            script.myUpgradeType = (ETypeOfUpgrade)EditorGUILayout.EnumPopup("Type of Upgrade", script.myUpgradeType);
//        }
//        else if (script.myAction == EActionType.SLEEP)
//        {

//        }

//        EditorUtility.SetDirty(script);
//    }
//}
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

    public Transform objectPosition, objectMoveUpPosition;

    public float timeForObjectMovement = 1;

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
        if (costOfInteraction > 0)
            costUI.text = costOfInteraction.ToString();
        else
        {
            costUI.text = "Yes";
        }
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
        GetComponent<Animator>().SetBool("OpenBox", true);
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        GameObject movingObject = Instantiate(objectToUnpack, transform.position, transform.rotation);
        //movingObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        objectMoveUpPosition = Instantiate(new GameObject(), transform).transform;
        objectMoveUpPosition.position += new Vector3(0, 2, 0);
        objectMoveUpPosition.eulerAngles += new Vector3(0, 180, 0);
        objectMoveUpPosition.localScale *= 2;//Temporär
        for (float i = 0; i <= 1; i += 0.01f)
        {
            movingObject.transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, objectMoveUpPosition.position.x, i),
                Mathf.Lerp(transform.position.y, objectMoveUpPosition.position.y, i),
                Mathf.Lerp(transform.position.z, objectMoveUpPosition.position.z, i));
            movingObject.transform.localScale = new Vector3(
                Mathf.Lerp(0, objectMoveUpPosition.localScale.x, i),
                Mathf.Lerp(0, objectMoveUpPosition.localScale.y, i),
                Mathf.Lerp(0, objectMoveUpPosition.localScale.z, i));
            movingObject.transform.eulerAngles = new Vector3(
                Mathf.Lerp(transform.eulerAngles.x, objectMoveUpPosition.eulerAngles.x, i),
                Mathf.Lerp(transform.eulerAngles.y, objectMoveUpPosition.eulerAngles.y, i),
                Mathf.Lerp(transform.eulerAngles.z, objectMoveUpPosition.eulerAngles.z, i));
            yield return new WaitForSeconds((float)(timeForObjectMovement) / 100);
        }
        //movingObject.transform.position = objectPosition.position;
        //movingObject.transform.rotation = objectPosition.rotation;
        yield return new WaitForSeconds(1);
        Destroy(movingObject);
        yield return new WaitForSeconds(1);
        objectPosition.gameObject.SetActive(true);
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
