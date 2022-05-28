using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wwise
{
    public class WwiseSingelton : MonoBehaviour
    {
        public static WwiseSingelton Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
