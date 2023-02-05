using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSfx : MonoBehaviour
    {

        private AudioSource _source;

        public string soundName;

        private void Awake()
        {

        Debug.Log("ButtonSfx");
            if (GetComponent<Button>() == null)
            {
            Debug.Log("ButtonSfx is null");

            return;
            }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.instance.Play(soundName);
            Debug.Log("ButtonSfx played sound");

        });

    }

}
