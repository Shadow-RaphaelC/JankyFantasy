using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectScript : MonoBehaviour
{
    [SerializeField] GameObject knight;
    [SerializeField] GameObject wizard;
    [SerializeField] GameObject archer;
    [SerializeField] TextMeshProUGUI textPlayer;

    [SerializeField] GameObject countPanel;
    [SerializeField] GameObject selectPanel;

    int index = 0;
    int currPlayer = 1;

    int playerCount = 2;

    void Start()
    {
        if (countPanel != null) countPanel.SetActive(true);
        if (selectPanel != null) selectPanel.SetActive(false);
    }

    //set le count avec event
    public void SetPlayerCount(int count)
    {
        playerCount = count;

        ConfirmPlayerCount();
    }

    //permet de choisir le nombre de joueur...si pas de manette les personnage de trop ne spawneront pas
    public void ConfirmPlayerCount()
    {
        PlayerPrefs.SetInt("playerCount", playerCount);

        if (countPanel != null) countPanel.SetActive(false);
        if (selectPanel != null) selectPanel.SetActive(true);

        currPlayer = 1;
        index = 0;

        textPlayer.SetText("Player 1");
        refreshCharacters();
    }

    public void btnNext()
    {
        index++;
        if (index == 3)
            index = 0;
        refreshCharacters();
    }
   
    public void btnPrevious()
    {
        index--;
        if (index == -1)
            index = 2;
        refreshCharacters();
    }


    //refresh l'affichage des model
    void refreshCharacters()
    {
        switch (index)
        {
            case 0:
                knight.SetActive(true);
                wizard.SetActive(false);
                archer.SetActive(false);
                break;

            case 1:
                knight.SetActive(false);
                wizard.SetActive(true);
                archer.SetActive(false);
                break;

            case 2:
                knight.SetActive(false);
                wizard.SetActive(false);
                archer.SetActive(true);
                break;

            default:
                knight.SetActive(true);
                wizard.SetActive(false);
                archer.SetActive(false);
                break;
        }
    }

    //fait les players pref pour les characters
    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("player" + (currPlayer - 1), index);

        if (currPlayer >= playerCount)
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene("CharacterMovementPLANE");
        }
        else
        {
            currPlayer++;
            index = 0;
            textPlayer.SetText("Player " + currPlayer);
            refreshCharacters();
        }
    }
}
