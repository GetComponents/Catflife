using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
    STARTWATER,
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

    public int UnlockIndex
    {
        get => m_unlockIndex;
        set
        {
            if (value == 0 && myAction == EActionType.UNPACK)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = interactableBoxMaterial;
            }
            m_unlockIndex = value;
        }
    }
    [SerializeField]
    [FormerlySerializedAs("UnlockIndex")]
    private int m_unlockIndex;

    public int BoxNumber;

    [SerializeField]
    Material interactableBoxMaterial;

    [SerializeField]
    private List<Interactable> FollowUpBoxes = new List<Interactable>();

    [SerializeField]
    private WateringCan wateringCan;

    private void Start()
    {
        player = PlayerInventory.Instance;
        ChangeUIText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && UnlockIndex <= 0)
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
            case EActionType.STARTWATER:
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
            //player.Energy -= costOfInteraction;

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
                //UnpackItem();
                BoxInteractionCanvas.Instance.OpenCanvas(costOfInteraction, this);
                break;
            case EActionType.WATER:
                PlantInteractionCanvas.Instance.OpenCanvas(costOfInteraction, this);
                //UpgradeStat();
                break;
            case EActionType.STARTWATER:
                Watering.Instance.EnableWatering();
                break;
            case EActionType.SLEEP:
                Sleep();
                break;
            default:
                break;
        }
    }

    private void OpenInteractionCanvas()
    {
    }

    public void UpgradeStat()
    {
        player.Energy -= costOfInteraction;
        switch (myUpgradeType)
        {
            case ETypeOfUpgrade.ATTACK:
                AkSoundEngine.PostEvent("Play_UpgradeJingle", this.gameObject);
                player.AttackUpgrades++;
                wateringCan.MoveWateringCan(transform.position);
                Debug.Log($"You Feel Stronger ({player.AttackUpgrades})");
                break;
            case ETypeOfUpgrade.HP:
                AkSoundEngine.PostEvent("Play_UpgradeJingle", this.gameObject);
                player.HPUpgrades++;
                wateringCan.MoveWateringCan(transform.position);
                Debug.Log($"You Feel Healthier ({player.HPUpgrades})");
                break;
            case ETypeOfUpgrade.MANA:
                AkSoundEngine.PostEvent("Play_UpgradeJingle", this.gameObject);
                player.ManaUpgrades++;
                wateringCan.MoveWateringCan(transform.position);
                Debug.Log($"You Feel More Resiliant ({player.ManaUpgrades})");
                break;
            case ETypeOfUpgrade.SPEED:
                AkSoundEngine.PostEvent("Play_UpgradeJingle", this.gameObject);
                player.DashUpgrades++;
                wateringCan.MoveWateringCan(transform.position);
                Debug.Log($"You Feel More Energetic ({player.DashUpgrades})");
                break;
        }
    }

    public void UnpackItem()
    {
        Destroy(interactionCanvas);
        isUnpacking = true;
        player.Energy -= costOfInteraction;
        GetComponent<Animator>().SetBool("OpenBox", true);
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        AkSoundEngine.PostEvent("Play_ImpactCardboard", this.gameObject);
        GameObject movingObject = Instantiate(objectToUnpack, transform.position, transform.rotation);
        objectMoveUpPosition = Instantiate(new GameObject(), transform).transform;
        objectMoveUpPosition.position += new Vector3(0, 2, 0);
        objectMoveUpPosition.eulerAngles += new Vector3(0, 180, 0);
        objectMoveUpPosition.localScale *= 2;
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

        yield return new WaitForSeconds(1);
        Destroy(movingObject);
        AkSoundEngine.PostEvent("Play_PoofCardboard", this.gameObject);
        yield return new WaitForSeconds(1);
        AkSoundEngine.PostEvent("Play_PoofCardboard", this.gameObject);

        BoxManager.Instance.OpenedBoxesIndex.Add(BoxNumber);
        OpenMyBox();
        if (UnlockContent)
        {
            GetComponent<PlayerUnlock>().UnlockMyContent();
        }
    }

    public void OpenMyBox()
    {
        foreach (Interactable box in FollowUpBoxes)
        {
            box.UnlockIndex--;
        }
        objectPosition.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void Sleep()
    {
        AkSoundEngine.PostEvent("Play_SleepJingle", this.gameObject);
        SceneTransition.Instance.ChangeScene("EncounterSelection", 0);
        PlayerController.Instance.HealFull();
    }
}
