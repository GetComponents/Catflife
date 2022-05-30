using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavalamp : MonoBehaviour
{
    [SerializeField]
    MeshRenderer lavalampBody;

    [SerializeField]
    GameObject myCanvas;

    [SerializeField]
    GameObject PlayerProjectileRed, PlayerProjectileBlue;

    public void ChangeColor()
    {
        if (PlayerController.Instance.LavalampColor == 0)
        {
            PlayerController.Instance.LavalampColor = 1;
            PlayerController.Instance.Projectile = PlayerProjectileBlue;
            lavalampBody.material.color = Color.blue;
        }
        else
        {
            PlayerController.Instance.LavalampColor = 0;
            PlayerController.Instance.Projectile = PlayerProjectileRed;
            lavalampBody.material.color = Color.red;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            myCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            myCanvas.SetActive(false);
    }
}
