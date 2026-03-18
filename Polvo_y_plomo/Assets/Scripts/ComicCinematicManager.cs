//---------------------------------------------------------
// Sistema de Cinemáticas mediante paneles estáticos
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // Librería de DOTween
// Añadir aquí el resto de directivas using


/// <summary>
/// Controlador principal para la secuencia de cinemáticas tipo cómic.
/// Gestiona la transición entre paneles usando DOTween para los fundidos,
/// permite al jugador saltar la animación actual o avanzar al siguiente panel,
/// y carga la escena del nivel al finalizar.
/// </summary>
public class ComicCinematicManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Lista de sprites que componen la cinemática en orden de aparición.
    /// </summary>
    [SerializeField] public Sprite[] ComicPanels;

    /// <summary>
    /// Componente de UI de tipo Image donde se mostrarán los paneles.
    /// </summary>
    [SerializeField] public Image PanelImage;

    /// <summary>
    /// Texto de UI para mostrar los mensajes de "saltar" o "continuar".
    /// </summary>
    [SerializeField] public TextMeshProUGUI PromptText;

    /// <summary>
    /// Duración en segundos del fundido de entrada de cada panel.
    /// </summary>
    [SerializeField] public float FadeDuration = 1.5f;

    /// <summary>
    /// Tiempo en segundos que el panel permanece en pantalla antes de pasar al siguiente automáticamente.
    /// </summary>
    [SerializeField] public float WaitTimeAfterAnimation = 3.0f;

    /// <summary>
    /// Referencia al componente ChangeScene para gestionar el cambio de nivel.
    /// </summary>
    [SerializeField] public ChangeScene SceneChanger;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Índice del panel que se está mostrando actualmente.
    /// </summary>
    private int _currentPanelIndex = 0;

    /// <summary>
    /// Bandera para saber si el panel actual está en mitad de su animación de fundido.
    /// </summary>
    private bool _isAnimating = false;

    /// <summary>
    /// Referencia a la animación actual de DOTween para poder interrumpirla limpiamente.
    /// </summary>
    private Tween _currentTween;

    /// <summary>
    /// Temporizador interno para el avance automático.
    /// </summary>
    private float _waitTimer = 0f;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Configura el estado inicial de la UI y lanza el primer panel.
    /// </summary>
    void Start()
    {
        // Asegurar que la imagen sea transparente al inicio
        Color initialColor = PanelImage.color;
        initialColor.a = 0f;
        PanelImage.color = initialColor;

        ShowNextPanel();
    }

    /// <summary>
    /// Escucha el input del jugador para saltar la animación o avanzar de panel.
    /// </summary>
    void Update()
    {
        if (_isAnimating)
        {
            // Si está animando y se pulsa un botón, forzamos el final de la animación
            if (Input.anyKeyDown)
            {
                SkipAnimation();
            }
        }
        else
        {
            // Si ya terminó de animar, restamos tiempo al temporizador automático
            _waitTimer -= Time.deltaTime;

            // Avanza si el jugador pulsa un botón O si el tiempo se agota
            if (Input.anyKeyDown || _waitTimer <= 0f)
            {
                _currentPanelIndex++;
                ShowNextPanel();
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Carga el siguiente sprite en la imagen y reproduce su animación de fundido.
    /// Si ya no hay más paneles, termina la cinemática.
    /// </summary>
    private void ShowNextPanel()
    {
        if (_currentPanelIndex >= ComicPanels.Length)
        {
            EndCinematic();
            return;
        }

        _isAnimating = true;

        PanelImage.sprite = ComicPanels[_currentPanelIndex];

        // Reiniciar alfa de la imagen a 0 antes de animar
        Color c = PanelImage.color;
        c.a = 0f;
        PanelImage.color = c;

        // Iniciar el Tween de fundido (Alpha de 0 a 1)
        _currentTween = PanelImage.DOFade(1f, FadeDuration).OnComplete(OnAnimationComplete);
    }

    /// <summary>
    /// Detiene el Tween actual, fuerza el panel a verse completamente y cambia el estado.
    /// </summary>
    private void SkipAnimation()
    {
        _currentTween?.Kill(); // Detiene el tween de DOTween instantáneamente

        Color c = PanelImage.color;
        c.a = 1f;
        PanelImage.color = c; // Fuerza la opacidad total

        OnAnimationComplete();
    }

    /// <summary>
    /// Se llama cuando el panel termina de aparecer, ya sea por tiempo o por un salto.
    /// </summary>
    private void OnAnimationComplete()
    {
        _isAnimating = false;
        _waitTimer = WaitTimeAfterAnimation; // Resetea el temporizador para el avance automático
        PromptText.text = "Pulsa cualquier botón para continuar";
    }

    /// <summary>
    /// Carga la escena del juego.
    /// </summary>
    private void EndCinematic()
    {
        if (SceneChanger != null)
        {
            SceneChanger.ChangeToNextScene();
        }
        else
        {
            Debug.LogError("Falta asignar el componente ChangeScene en el Inspector.");
        }
    }

    #endregion   

} // class ComicCinematicManager 
// namespace
