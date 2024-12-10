using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Gamehandler : MonoBehaviour
{

    public PlayerController playerMovement;
    [SerializeField] private GameObject player;
    [SerializeField] private Canvas playerUI;
    [SerializeField] private Image dashAbilityHolder;
    [SerializeField] private Image dashImage0;
    [SerializeField] private Image dashImage1;
    [SerializeField] private Image blackFade;
    [SerializeField] private Image loadingIcon;
    private TextMeshProUGUI enemyLeftText;
    private DungeonGenerator levelGeneration;

    void Awake()
    {
        levelGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        enemyLeftText = GameObject.FindGameObjectWithTag("EnemyLeftText").GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        levelGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        if (levelGeneration.playerSpawned)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerController>();
            DashCooldown();
        }
        enemyLeftText.text = ("Enemies left: \n" +  levelGeneration.enemyCount);
    }

    public void PlayerDead()
    {
        SceneManager.LoadScene("Menu");
        Destroy(player.gameObject);
        Destroy(GameObject.FindGameObjectWithTag("MainCamera").gameObject);
        Destroy(gameObject);
        Destroy(playerUI);
    }

    void DashCooldown()
    {
        if (playerMovement.dashUnlocked == true)
        {
            dashAbilityHolder.gameObject.SetActive(true);
            dashImage0.enabled = true;
            dashImage1.enabled = true;
        }

        if (playerMovement.dashCoolCounter < 1)
        {
            dashImage1.fillAmount = playerMovement.dashCoolCounter;
        }
        else if (playerMovement.dashCoolCounter >= 1)
        {
            dashImage1.fillAmount = playerMovement.dashCoolCounter;
        }
    }

    public void NextFloor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        levelGeneration.RunGeneration = true;
        levelGeneration.levelCount++;
        levelGeneration.playerSpawned = false;
        Color color = Color.black;
        color.a = 1f;
        blackFade.CrossFadeAlpha(1, 0.1f, true);
        loadingIcon.CrossFadeAlpha(1, 0.1f, true);
        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<PlayerController>().rb.velocity = new Vector3(0, 0, 0);
    }

    public IEnumerator CanvasFade()
    {
        playerMovement = player.GetComponent<PlayerController>();
        if (playerMovement.canMove == true)
        {
            playerMovement.canMove = false;
        }
        yield return new WaitForSeconds(1);
        blackFade.CrossFadeAlpha(0, 1, true);
        loadingIcon.CrossFadeAlpha(0, 1, true);
        playerMovement.canMove = true;

    }
}
