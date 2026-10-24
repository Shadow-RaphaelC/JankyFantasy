using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Liens de référence qui ont servi a faire le système de multijoueur local (CLAVIER + MANETTE)
// https://discussions.unity.com/t/multiple-players-on-keyboard-new-input-system/754028
// https://github.com/JJoeDev/UnityLocalMultiplayer
// https://www.youtube.com/watch?v=u3KoWI92blE

// On commence part set les action map dans le package d'input
// Ensuite il faut ajouter un control scheme pour chaque joueur (ex: KeyboardWASD, KeyboardIJKL, Gamepad1, Gamepad2)
// Ajoute le player input script pour les movements et les attaques
// Ensuite, dans le PlayerSpawner, on instancie les joueurs en fonction de leurs choix de personnages et de control scheme
// IL FAUT QU'ON PAIR AVEC LE BON DEVICE!!!


public class PlayerSpawner : MonoBehaviour
{
    [Header("Character Prefabs")]
    [SerializeField] private GameObject[] characterPrefabs;

    [Header("Point de spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject[] healthBars;

    private readonly string[] schemas = { "KeyboardWASD", "KeyboardIJKL", "Gamepad", "Gamepad" };

    private void Start()
    {
        // Récupère le nombre de joueurs et leurs choix de personnages depuis PlayerPrefs
        int playerCount = PlayerPrefs.GetInt("playerCount", 2);

        for (int i = 0; i < playerCount; i++)
        {
            int characterIndex = PlayerPrefs.GetInt("player" + i, 0);

            // Spawn le joueur
            SpawnPlayer(i, characterIndex);
        }
    }

    private void SpawnPlayer(int playerIndex, int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= characterPrefabs.Length) return;

        // Récupère le device correspondant au joueur
        InputDevice device = GetDevice(playerIndex);
        if (device == null) return;

        // Instancie le PlayerInput en connectant le bon device
        PlayerInput input = PlayerInput.Instantiate(
            characterPrefabs[characterIndex], 
            controlScheme: schemas[playerIndex], 
            pairWithDevice: device
        );

        // Place le joueur au point de spawn correspondant
        if (spawnPoints != null && playerIndex < spawnPoints.Length && spawnPoints[playerIndex] != null)
            input.transform.position = spawnPoints[playerIndex].position;

        // Active le ring de couleur pour différencier les joueurs
        EnablePlayerRing(input.gameObject, playerIndex);

        // Lie la barre de vie au joueur sur l'écran
        LinkHealthBar(input.gameObject, playerIndex);

        // Enregistre le joueur dans le GameManager (niceee)
        Character character = input.GetComponent<Character>();
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null && character != null)
            gm.RegisterPlayer(character);
    }

    private InputDevice GetDevice(int playerIndex)
    {
        // Les 2 premiers joueurs utilisent le clavier, les autres utilisent les manettes
        if (playerIndex <= 1) return Keyboard.current;
        int gamepadIndex = playerIndex - 2;
        if (gamepadIndex < Gamepad.all.Count)
            return Gamepad.all[gamepadIndex];

        return null;
    }

    private void EnablePlayerRing(GameObject player, int playerIndex)
    {
        // On cherche le tag "playerX" pour activer son ring de couleur
        string wantedTag = "player" + (playerIndex + 1);

        foreach (Transform child in player.transform)
        {
            // Si on trouve le bon tag, on active le ring
            if (child.CompareTag(wantedTag))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void LinkHealthBar(GameObject player, int playerIndex)
    {
        // Active la barre de vie / lie le script HealthBarLink pour UPDATE les PVs
        if (playerIndex >= healthBars.Length) return;
        GameObject bar = healthBars[playerIndex];
        if (bar == null) return;

        bar.SetActive(true);

        // Récupère le slider de la barre de vie et le script Combattant du joueur
        Slider slider = bar.GetComponent<Slider>();
        Combattant combattant = player.GetComponent<Combattant>();

        if (slider != null && combattant != null)
        {
            // Ajoute le script HealthBarLink au joueur
            HealthBarLink link = player.AddComponent<HealthBarLink>();

            // Setup le lien pour que la barre de vie suive les PVs du combattant
            link.Setup(combattant, slider);
        }
    }
}

// Finalement, le PlayerSpawner gère l'instanciation des joueurs avec leurs personnages et le control scheme
// correspondant, il lie les barres de PV au BON joueur et aussi active le ring pour faire la différence entre les joueurs.
// litéralement c'est juste ça, le GameManager prend un joueur pour juste verifier le WIN/LOSE