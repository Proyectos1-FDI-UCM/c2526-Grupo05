//---------------------------------------------------------
// Controlador de disparo específico para la escopeta. Dispara perdigones en cono.
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que genera múltiples proyectiles (perdigones) en una dispersión cónica.
/// Debe estar como hijo del arma principal.
/// </summary>
public class ShootEscopeta : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Prefab del perdigón que se instanciará. Debe ser una versión más pequeña de la bala normal.
    /// </summary>
    [SerializeField]
    private BulletMove PerdigonPrefab;

    /// <summary>
    /// Rango total en grados del cono de dispersión.
    /// </summary>
    [SerializeField]
    private float RangoCono = 60f;

    /// <summary>
    /// Número de perdigones a disparar por cada acción.
    /// </summary>
    [SerializeField]
    private int NumPerdigones = 4;

    /// <summary>
    /// Separación mínima en grados garantizada entre cada perdigón.
    /// </summary>
    [SerializeField]
    private float DisparidadMinima = 5f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Validaciones iniciales de dependencias.
    /// </summary>
    private void Awake()
    {
        if (PerdigonPrefab == null)
        {
            Debug.LogError("No se ha asignado el prefab del perdigón en ShootEscopeta. El arma no funcionará.");
            Destroy(this);
        }

        // Validación matemática para evitar superposiciones si se configura mal en el inspector
        if (DisparidadMinima * NumPerdigones >= RangoCono)
        {
            Debug.LogWarning("La disparidad mínima es demasiado alta para el rango del cono.");
            DisparidadMinima = (RangoCono / NumPerdigones) - 1f;
        }
    }

    /// <summary>
    /// Si el componente es destruido por no poder funcionar, se asegura que el resto de componentes dejen de funcionar también.
    /// No puede destruir el controlador del disparo puesto que aquí no sabemos que script será.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<HasAmmo>());
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
    /// Dispara múltiples perdigones en la dirección indicada, aplicando la dispersión en cono
    /// y asegurando una disparidad direccional mínima entre ellos mediante división por sectores.
    /// </summary>
    public void ShootBullet(Vector2 fireDir)
    {
        float anguloBase = 180f / Mathf.PI * Mathf.Atan2(fireDir.y, fireDir.x);

        float tamañoSector = RangoCono / NumPerdigones;
        float mitadCono = RangoCono / 2f;
        float padding = DisparidadMinima / 2f;

        for (int i = 0; i < NumPerdigones; i++)
        {
            float limiteMin = -mitadCono + (i * tamañoSector);
            float limiteMax = limiteMin + tamañoSector;

            float variacionAleatoria = Random.Range(limiteMin + padding, limiteMax - padding);
            float anguloFinal = anguloBase + variacionAleatoria;

            anguloFinal %= 360;
            if (anguloFinal < 0) anguloFinal += 360f;

            Quaternion rotacionPerdigon = Quaternion.Euler(0, 0, anguloFinal);

            Instantiate(PerdigonPrefab, transform.position, rotacionPerdigon);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ShootEscopeta 
// namespace
