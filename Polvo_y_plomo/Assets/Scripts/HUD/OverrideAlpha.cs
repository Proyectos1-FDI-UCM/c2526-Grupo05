//---------------------------------------------------------
// Script para evitar cambios en el Alpha de un objeto
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script para sobreescribir cambios en la transparencia en las balas del HUD al transparentarse este,
/// ya que actualmente estamos mezclando Animator y control del alpha mediante scripts y esta dando problemas.
/// Es una solución un poco bruta, pero funciona. Sobreescribe los cambios en el Alpha del Animator al hacerlo en el LateUpdate().
/// </summary>
public class OverrideAlpha : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Valor del alpha que se mantendrá
    /// </summary>
    [SerializeField]
    private float MantainAlpha = 0.15f;

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
    /// Almacena el color del objeto.
    /// Inicializado en el Awake()
    /// </summary>
    private Color _color;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _uiImage = GetComponent<Image>();
        _rawImage = GetComponent<RawImage>();
        _rend = GetComponent<Renderer>();
        if (_sprite == null && _uiImage == null && _rend == null && _rawImage == null)
        {
            Debug.Log("Script \"OverrideAlpha\" colocado en un objeto sin color. Me destruyo");
            Destroy(this);
        }

        _color = GetColor();
        this.enabled = false;
    }
    private void LateUpdate()
    {
        _color.a = MantainAlpha;
        SetColor(_color);
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
    /// Método que recoge el color del objeto, de los 3 posibles componentes con color.
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
    /// Método que establece el color del objeto, de los 3 posibles componentes con color.
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

} // class OverrideAlpha 
// namespace
