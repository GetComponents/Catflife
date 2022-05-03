using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance;
    public GameObject myCanvas;

    [SerializeField]
    TextMeshProUGUI debugText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(myCanvas);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OpenConsole()
    {
        debugText.text = "";
        myCanvas.SetActive(true);
    }

    public void CloseConsole()
    {
        myCanvas.SetActive(false);
    }

    public void ReadInput(string _input)
    {
        string[] inputInfo = _input.Split(' ');
        switch (inputInfo[0])
        {
            case "":
                CloseConsole();
                break;
            case "giveEnergy":
                int energyAmount;
                if (int.TryParse(inputInfo[1], out energyAmount))
                {
                    PlayerInventory.Instance.Energy += energyAmount;
                    setDebugText($"You Gained {energyAmount} Energy");
                }
                else
                    giveErrorMessage();
                break;
            case "killEnemies":
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    enemy.TakeDamage(999);
                }
                setDebugText("Watch Your Enemies Faulter");
                break;
            case "turnInvincible":
                PlayerController.Instance.isInvincible = true;
                setDebugText("You Are Invincible As Long As You Dont Dash!");
                break;
            case "setUpgrade":
                int upgradeNumber;
                switch (inputInfo[1])
                {
                    case "attack":
                        if (int.TryParse(inputInfo[2], out upgradeNumber))
                        {
                            setDebugText($"Your Attack Has Been Set To {upgradeNumber}");
                            PlayerInventory.Instance.AttackUpgrades = upgradeNumber;
                        }
                        else
                            giveErrorMessage();
                        break;
                    case "health":
                        if (int.TryParse(inputInfo[2], out upgradeNumber))
                        {
                            setDebugText($"Your Health Has Been Set To {upgradeNumber}");
                            PlayerInventory.Instance.HPUpgrades = upgradeNumber;
                        }
                        else
                            giveErrorMessage();
                        break;
                    case "mana":
                        if (int.TryParse(inputInfo[2], out upgradeNumber))
                        {
                            setDebugText($"Your Mana Has Been Set To {upgradeNumber}");
                            PlayerInventory.Instance.ManaUpgrades = upgradeNumber;
                        }
                        else
                            giveErrorMessage();
                        break;
                    case "speed":
                        if (int.TryParse(inputInfo[2], out upgradeNumber))
                        {
                            setDebugText($"Your Speed/Dash Has Been Set To {upgradeNumber}");
                            PlayerInventory.Instance.DashUpgrades = upgradeNumber;
                        }
                        else
                            giveErrorMessage();
                        break;
                    default:
                        giveErrorMessage();
                        break;
                }
                break;
            case "infiniteMana":
                PlayerController.Instance.maxMana = 10000000;
                PlayerController.Instance.CurrentMana = PlayerController.Instance.maxMana;
                setDebugText("You Have Gained Enough Wisdom For A Lifetime");
                break;
            case "help":
                setDebugText(@"Possible Commands:
giveEnergy *amount*
killEnemies
turnInvincible
setUpgrade attack/health/mana/speed *upgrade level*
infiniteMana");
                break;
            default:
                giveErrorMessage();
                break;
        }
    }

    public void giveErrorMessage()
    {
        debugText.text += "\nERROR: Command Not Valid";
    }

    private void setDebugText(string _text)
    {
        debugText.text += $"\n {_text}";
    }
}
