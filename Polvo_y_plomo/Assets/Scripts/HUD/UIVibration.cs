//---------------------------------------------------------
// Hace vibrar a un elemento de UI modificando sus Anchors min y max 'x' e 'y'
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Hace vibrar un elemento de UI generando un vector 2D unitario al azar y añadiendoselo escalado
/// a los valores originales almacenados de los anchors del RectTransform.
/// El escalado se hace con el valor Intensity, dividiendolo entre _INTENSITYSCALE.
/// </summary>
public class UIVibration : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Parámetro que moverá los min y máx anchors dividido entre _INTENSITYSCALE.
    /// Ahora mismo esta ajustado para admitir valores de [0, 1000], con 1000 siendo cambios que mueven
    /// el componente 1 pantalla entera
    /// </summary>
    [SerializeField]
    private float Intensity = 0f;

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
    /// Número que determina la escala en la que esta la intensidad, tal que [0, _INTENSITYSCALE] pasa al rango [0, 1].
    /// </summary>
    private const int _INTENSITYSCALE = 1000;

    /// <summary>
    /// Almacena el RectTransform de este objeto.
    /// </summary>
    private RectTransform _UItransform;

    /// <summary>
    /// Almacena los Anchors originales minimos. Inicializado en el Awake().
    /// </summary>
    private Vector2 _minAnchors;

    /// <summary>
    /// Almacena los Anchors originales maximos. Inicializado en el Awake().
    /// </summary>
    private Vector2 _maxAnchors;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Llamado al cargar en escena.
    /// Inicializa el componente.
    /// </summary>
    private void Awake()
    {
        _UItransform = GetComponent<RectTransform>();
        if ( _UItransform == null)
        {
            Debug.Log("Componente UIVibration puesto en un GameOjbect sin RectTransform. No funcionará");
            Destroy(this);
        }
        else
        {
            _minAnchors = _UItransform.anchorMin;
            _maxAnchors = _UItransform.anchorMax;
        }
    }

    /// <summary>
    /// Llamado cada frame mientras el componente este activo.
    /// Hace vibrar al objeto o se apaga si la intensidad cambia a 0.
    /// </summary>
    private void Update()
    {
        if (Intensity > 0f)
        {
            float ang = Random.Range(0f, Mathf.PI * 2);
            Vector2 rndDir = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)); // unitario

            rndDir *= Intensity / _INTENSITYSCALE;

            _UItransform.anchorMin = _minAnchors + rndDir;
            _UItransform.anchorMax = _maxAnchors + rndDir;
        }
        else
        {
            _UItransform.anchorMin = _minAnchors;
            _UItransform.anchorMax = _maxAnchors;
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
    /// Método público para cambiar la intensidad de vibración.
    /// </summary>
    /// <param name="Intensity"></param>
    public void ChangeIntensity(float Intensity)
    {
        this.Intensity = Intensity;
        this.enabled = true; // si es 0 o menor en el update se comprueba y se para.
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class UIVibration 
// namespace
