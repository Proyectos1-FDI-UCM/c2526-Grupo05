//---------------------------------------------------------
// Herramienta de fades de transparencia
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente al que se le puede asignar cualquier objeto y busca este algún elemento de color.
/// Si lo encuentra, realiza un fade de transparencia entre 2 asignables (inical y final) durante un tiempo asignable.
/// (!) Ha de estar desactivada (como componente, no gameobject) al inicio de la escena. Si no lo está, se activará el fade.
/// 
/// +++
/// Añadida funcionalidad para que si no se asigna target, intente cambiarse el color a si mismo.
/// </summary>
public class FadeColor : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// GameObject objetivo al que se le cambiara la transparencia. Ha de tener algún tipo componente con color.
    /// </summary>
    [SerializeField]
    private GameObject target;

    /// <summary>
    /// Alpha desde el que iniciará el fade de transparencia
    /// </summary>
    [SerializeField]
    private float StartAlpha = 0f;

    /// <summary>
    /// Alpha desde el que acabará el fade de trasnparencia
    /// </summary>
    [SerializeField]
    private float FinalAlpha = 1f;

    /// <summary>
    /// Tiempo en segundos que tardará el fade en ocurrir.
    /// </summary>
    [SerializeField]
    private float FadeTime = 2f;


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
    /// Si lo tiene, almacena el SpriteRenderer del target.
    /// Inicializado en el Awake()
    /// </summary>
    private SpriteRenderer _sprite;

    /// <summary>
    /// Si lo tiene, almacena la Image del target.
    /// Inicializado en el Awake()
    /// </summary>
    private Image _uiImage;

    /// <summary>
    /// Si lo tiene, almacena el RawImage del target
    /// Inicializado en el Awake()
    /// </summary>
    private RawImage _rawImage;

    /// <summary>
    /// Si lo tiene, almacena el Renderer con color del target.
    /// Inicializado en el Awake()
    /// </summary>
    private Renderer _rend;

    /// <summary>
    /// Almacena el color inicial del target.
    /// Inicializado en el Awake()
    /// </summary>
    private Color _startColor;

    /// <summary>
    /// Almacena el color final del target.
    /// Inicializado en el Awake()
    /// </summary>
    private Color _endColor;

    /// <summary>
    /// Almacena el tiempo desde que inicia el fade.
    /// </summary>
    private float _t;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    /// <summary>
    /// Se llama al activarse el componente (incluyendo al inicio de la escena, si esta activo).
    /// Inicializa _t para empezar el fade.
    /// </summary>
    private void OnEnable()
    {
        _t = 0;
    }

    /// <summary>
    /// Se llama al cargarse en escena por primera vez.
    /// Intenta sacar un componente SpriteRenderer, Image, RawImage o Renderer con color del objeto target, y registra el color inicial con GetColor().
    /// También calcula cuales han de ser los colores iniciales y finales según las transparencias indicadas.
    /// Si no hay target o no hay componente con color hay programación defensiva que evita que el componente falle.
    /// </summary>
    private void Awake()
    {
        if (target == null) target = this.gameObject;

        _sprite = target.GetComponent<SpriteRenderer>();
        _uiImage = target.GetComponent<Image>();
        _rawImage = target.GetComponent<RawImage>();
        _rend = target.GetComponent<Renderer>();
        if (_sprite == null && _uiImage == null && _rend == null && _rawImage == null)
        {
            Debug.Log("Script \"FadeColor\" colocado en un objeto sin color. Me destruyo");
            Destroy(this);
        }
        _startColor = GetColor();
        _startColor = new Color(_startColor.r, _startColor.g, _startColor.b, StartAlpha);
        _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, FinalAlpha);
    }

    /// <summary>
    /// Realiza el FadeColor cambiando entre _startColor y _endColor (mismo color, distinta transparencia) a lo largo del tiempo.
    /// Desactiva el componente trás acabar.
    /// </summary>
    private void LateUpdate()
    {
        if (_t < FadeTime)
        {
            _t += Time.deltaTime;

            SetColor(Color.Lerp(_startColor, _endColor, _t / FadeTime));
        }
        else
        {
            SetColor(_endColor); // asegurarse de que acabe en el final
            this.enabled = false;
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
    /// Método para modificar el StartAlpha en caso de ser necesario.
    /// </summary>
    /// <param name="newStartAlpha"></param>
    public void SetStartAlpha(float newStartAlpha)
    {
        StartAlpha = newStartAlpha;
    }

    /// <summary>
    /// Método para leer el alpha actual
    /// </summary>
    /// <returns></returns>
    public float GetCurrentAlpha()
    {
        Color color = GetColor();
        return color.a;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
     
    /// <summary>
    /// Método que recoge el color del objeto, de los 4 posibles componentes con color.
    /// </summary>
    /// <returns></returns>
    private Color GetColor()
    {
        if (_sprite != null) return _sprite.color;
        if (_uiImage != null) return _uiImage.color;
        if (_rend != null && _rend.material.HasProperty("_Color")) return _rend.material.color;
        if (_rawImage != null) return _rawImage.color;

        return Color.white;
    }

    /// <summary>
    /// Método que establece el color del objeto, de los 4 posibles componentes con color.
    /// </summary>
    /// <param name="c"></param>
    private void SetColor(Color c)
    {
        if (_sprite != null) _sprite.color = c;
        else if (_uiImage != null) _uiImage.color = c;
        else if (_rend != null && _rend.material.HasProperty("_Color")) _rend.material.color = c;
        else if (_rawImage != null) _rawImage.color = c;
    }
        #endregion

    } // class FadeColor 
// namespace
