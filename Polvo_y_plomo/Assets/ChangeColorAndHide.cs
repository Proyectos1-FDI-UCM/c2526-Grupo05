//---------------------------------------------------------
// Este script se le añade a un objeto con TextMeshPro, para que tenga efectos de cambiar de color.
// Juan José de Reyna Godoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ChangeColorAndHide : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private float DurationTime = 0;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private TextMeshProUGUI _thistext;

    private float r = 0, g = 0, b = 0, a = 1;

    private float _enabledTime = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        _thistext = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        r = 1;
        g = 0;
        b = 0;

        _thistext.color = new Color(r, g, b, a);

        _enabledTime = Time.time;
    }

    /// <summary>
    /// Se llama una vez por frame.
    /// 
    /// </summary>
    void Update()
    {
        Debug.Log("rwrw");
        r = Mathf.Sin(Time.time * 20);
        g = Mathf.Sin(20 * Time.time + 2.25f);
        b = Mathf.Sin(20 * Time.time + 10.5f);
        _thistext.color = new Color(r, g, b, a);
        if (Time.time - _enabledTime > DurationTime)
        {
            this.enabled = false;
            _thistext.color = new Color(r, g, b, 0);
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

    public void ColorChanging()
    {
        this.enabled = true;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ChangeColorAndHide 
// namespace