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
/// Este componente, cuando está activado, cambia el color de la cadena de texto que contiene el TextMeshPro del
/// GameObject. Pasado un tiempo configurable desde el editor, se desactiva. Tiene un método público que lo activa.
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

    /// <summary>
    /// Duración de la activación del componente.
    /// </summary>
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

    const int ZERO = 0;

    const float G_DIFF = 2.25f;

    const float B_DIFF = 10.5f;

    const float MUL = 20;

    /// <summary>
    /// Es el componente TextMeshPro de este GameObject
    /// </summary>
    private TextMeshProUGUI _thistext;

    /// <summary>
    /// Son los valores del color RGBA que tomará el texto.
    /// </summary>
    private float r = 0, g = 0, b = 0, a = 1;

    /// <summary>
    /// Es el momento en el que se ha activado el componente
    /// </summary>
    private float _enabledTime = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al iniciar la escena, o al instanciar el GameObject. 
    /// Dará valor a _thisText
    /// </summary>
    void Awake()
    {
        _thistext = GetComponent<TextMeshProUGUI>();
        if (_thistext == null)
        {
            Debug.Log("Se hapuesto el componente ChangeColorAndHide en un objeto sin TtextMeshPro," +
                "no funionará y es destruirá");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama cuando se active este componente.
    /// Inicializará los parámetros del color del texto, el propio color, y guarda el momento en el que se ha activado.
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
    /// Llevará todo el proceso de cambio de color. Cada parámetro de color cambia en función de una variación de la 
    /// función seno. Cuando Haya pasado el tiempo de duració, se desactiva el componente
    /// </summary>
    void Update()
    {
        r = Mathf.Sin(MUL * Time.time);
        g = Mathf.Sin(MUL * Time.time + G_DIFF);
        b = Mathf.Sin(MUL * Time.time + B_DIFF);
        _thistext.color = new Color(r, g, b, a);
        if (Time.time - _enabledTime > DurationTime)
        {
            this.enabled = false;
            _thistext.color = new Color(r, g, b, ZERO);
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
    /// Solo activaeste componente.
    /// </summary>
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