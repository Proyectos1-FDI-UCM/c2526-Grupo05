//---------------------------------------------------------
// Controlador individual de la UI de cada corazón
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona la animación de rotura de un corazón en el HUD
/// Almacena los sprites y reproduce una secuencia
/// cuando el jugador recibe daño lo deja en su estado vacío
/// </summary>
public class HeartUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Componente Image de la UI que muestra el corazón
    /// </summary>
    [SerializeField]
    private Image HeartImage;


    /// <summary>
    /// Animaciones de daño
    /// </summary>
    [SerializeField]
    private Sprite[] TransitionEmptyToHalf;
    [SerializeField]
    private Sprite[] TransitionHalfToEmpty;

    /// <summary>
    /// Animaciones de curación
    /// </summary>
    [SerializeField]
    private Sprite[] TransitionFullToHalf;
    [SerializeField]
    private Sprite[] TransitionHalfToFull;

    /// <summary>   
    /// Tiempo en segundos que dura cada frame de la animación de rotura.
    /// </summary>
    [SerializeField]
    private float TimeBetweenFrames = 0.05f;

    /// <summary>   
    /// Tiempo en segundos que dura cada frame de la animación de cura al completo.
    /// </summary>
    [SerializeField]
    private float TimeBetweenTransitionHalfToFullFrames = 0.02f;

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
    /// Referencia al array de sprites que se está reproduciendo actualmente.
    /// </summary>
    private Sprite[] _animacionActual;

    /// <summary>
    /// Índice del frame actual de la animación.
    /// </summary>
    private int _currentFrame = 0;

    /// <summary>
    /// Temporizador para el avance de los frames.
    /// </summary>
    private float _timer = 0f;

    /// <summary>
    /// Indica si hay una animación en curso.
    /// </summary>
    private bool _isAnimating = false;

    /// <summary>
    /// Almacena el tiempo de espera actual entre frames (ajustable según la animación).
    /// </summary>
    private float _currentTimeStep = 0.05f;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Gestiona el temporizador para avanzar los frames de las animaciones de rotura y curación
    /// </summary>
    void Update()
    {
        if (_isAnimating && _animacionActual != null)
        {
            _timer += Time.deltaTime;

            if (_timer >= _currentTimeStep)
            {
                _timer = 0f;
                _currentFrame++;

                if (_currentFrame < _animacionActual.Length)
                {
                    HeartImage.sprite = _animacionActual[_currentFrame];
                }
                else
                {
                    _currentFrame = _animacionActual.Length - 1;
                    _isAnimating = false;
                }
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

    /// <summary>
    /// Pinta el corazon vacio sin animación
    /// </summary>
    public void EmptyHeart()
    {
        HeartImage.sprite = TransitionEmptyToHalf[0];
    }

    /// <summary>
    /// Pinta el corazon lleno sin animación
    /// </summary>
    public void FullHeart()
    {
        HeartImage.sprite = TransitionFullToHalf[0];
    }

    /// <summary>
    /// Pinta el medio corazón sin animación
    /// </summary>
    public void HalfHeart()
    {
        HeartImage.sprite = TransitionHalfToEmpty[0];
    }

    public void HitToHalf()
    {
        ReproducirAnimacion(TransitionFullToHalf, TimeBetweenFrames);
    }

    public void HitToEmpty()
    {
        ReproducirAnimacion(TransitionHalfToEmpty, TimeBetweenFrames);
    }

    public void HealToHalf()
    {
        ReproducirAnimacion(TransitionEmptyToHalf, TimeBetweenFrames);
    }

    public void HealToFull()
    {
        ReproducirAnimacion(TransitionHalfToFull, TimeBetweenTransitionHalfToFullFrames);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Configura y arranca la animación seleccionada
    /// </summary
    private void ReproducirAnimacion(Sprite[] nuevaAnimacion, float velocidad)
    {
        if (nuevaAnimacion == null || nuevaAnimacion.Length == 0) return;

        _animacionActual = nuevaAnimacion;
        _currentTimeStep = velocidad;
        _currentFrame = 0;
        _timer = 0;
        _isAnimating = true;

        HeartImage.sprite = _animacionActual[0];
    }

    #endregion   

} // class HeartUI 
// namespace
