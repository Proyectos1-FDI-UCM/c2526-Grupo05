//---------------------------------------------------------
// Herramienta de fades de transparencia
// Responsable de la creación de este archivo
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
    /// Inicializado en el Start()
    /// </summary>
    private SpriteRenderer _sprite;

    /// <summary>
    /// Si lo tiene, almacena la Image del target.
    /// Inicializado en el Start()
    /// </summary>
    private Image _uiImage;

    /// <summary>
    /// Si lo tiene, almacena el Renderer con color del target.
    /// Inicializado en el Start()
    /// </summary>
    private Renderer _rend;

    /// <summary>
    /// Almacena el color inicial del target.
    /// Inicializado en el Start()
    /// </summary>
    private Color _startColor;

    /// <summary>
    /// Almacena, si se ha iniciado, la corrutina del fade.
    /// </summary>
    private Coroutine _fadeCoroutine;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en escena por primera vez, si el componente está activo.
    /// Intenta sacar un componente SpriteRenderer, Image o Renderer con color del objeto target, y registra el color inicial con GetColor().
    /// Si no hay target o no hay componente con color hay programación defensiva que evita que el componente falle.
    /// </summary>
    private void Start()
    {
        if (target != null)
        {
            _sprite = target.GetComponent<SpriteRenderer>();
            _uiImage = target.GetComponent<Image>();
            _rend = target.GetComponent<Renderer>();
            if (_sprite == null && _uiImage == null && _rend == null)
            {
                Debug.Log("Script \"FadeColor\" colocado en un objeto sin color. Me destruyo");
                Destroy(this);
            }
            _startColor = GetColor();
        }
        else
        {
            Debug.Log("Componente \"Fade Color\" colocado y sin target asignado. No funcionará.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama al desactivarse el componente (necesariamente ha de ocurrir Enabled -> Disabled, si empieza desactivado, no se llama).
    /// Se asegura de que se haya acabado la corrutina al desactivarse.
    /// </summary>
    private void OnDisable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
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
    /// Método público que permite iniciar el fade de transparencia.
    /// Se asegura de que solo pueda ocurrir uno de estos a la vez.
    /// </summary>
    public void StartFade()
    {
        if (_fadeCoroutine == null) _fadeCoroutine = StartCoroutine(FadeCoroutine());
        else Debug.Log("Se ha intentado iniciar un fade cuando otro ya estaba en proceso");
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
     
    /// <summary>
    /// Método que recoge el color del objeto, de los 3 posibles componentes con color.
    /// </summary>
    /// <returns></returns>
    private Color GetColor()
    {
        if (_sprite != null) return _sprite.color;
        if (_uiImage != null) return _uiImage.color;
        if (_rend != null && _rend.material.HasProperty("_Color")) return _rend.material.color;

        return Color.white;
    }

    /// <summary>
    /// Método que establece el color del objeto, de los 3 posibles componentes con color.
    /// </summary>
    /// <param name="c"></param>
    private void SetColor(Color c)
    {
        if (_sprite != null) _sprite.color = c;
        else if (_uiImage != null) _uiImage.color = c;
        else if (_rend != null && _rend.material.HasProperty("_Color")) _rend.material.color = c;
    }

    /// <summary>
    /// Corrutina que cambia progresivamente la transparencia del objeto durante el tiempo asignado.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeCoroutine()
    {
        float t = 0f;
        _startColor = new Color(_startColor.r, _startColor.g, _startColor.b, StartAlpha);
        Color endColor = new Color(_startColor.r, _startColor.g, _startColor.b, FinalAlpha);

        while (t < FadeTime)
        {
            t += Time.deltaTime;
            SetColor(Color.Lerp(_startColor, endColor, t / FadeTime));
            yield return null;
        }
        SetColor(endColor); // asegurarse de que acabe en el final
        _fadeCoroutine = null;
    }
        #endregion

    } // class FadeColor 
// namespace
