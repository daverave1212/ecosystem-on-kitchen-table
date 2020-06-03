using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingClick : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;
   public void OpenPanel()
    {
        if(Panel != null)
        {
            if (Panel.active == false)
            {
                Panel.SetActive(true);
            }
            else
            {
                Panel.SetActive(false);
            }

        }
        
    }
}
