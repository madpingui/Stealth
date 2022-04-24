using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore { get; private set; }

    private Text scoreText;

    public void IncreaseScore()
    {
        CurrentScore += 1;
        scoreText.text = "Llaves: " + CurrentScore;
    }

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Implementacion del Singleton 
    private static ScoreManager instance;
    private ScoreManager() { }

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("ScoreManager.Instance es nula pero se esta intentando accederla");
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            //Si soy la primera instancia, hacerme el Singleton
            instance = this;
            //Descomentar esta linea si el singleton debe persistir entre escenas
            //DontDestroyOnLoad(this);
        }
        else
        {
            //Si ya existe este singleton en la escena entonces destruir este objeto 
            if (this != instance)
                DestroyImmediate(this.gameObject);
        }
    }
}
