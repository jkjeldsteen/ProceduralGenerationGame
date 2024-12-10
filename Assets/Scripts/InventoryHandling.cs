using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryHandling : MonoBehaviour
{
    [SerializeField] Gamehandler gameHandler;
    [SerializeField] private Image potionFrame;
    [SerializeField] private Image potion;
    [SerializeField] private Sprite fullPotion;
    [SerializeField] private Sprite emptyPotion;
    [SerializeField] private TextMeshProUGUI potionAmount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameHandler.playerMovement != null)
        {
            if(gameHandler.playerMovement.healthPots <= 0)
            {
                potion.sprite = emptyPotion;
            }
            else
            {
                potion.sprite = fullPotion;
            }
            potionAmount.text = gameHandler.playerMovement.healthPots + "/10";
        }
    }
}
