//---------------------------------------------------------
// Script singleton al que acceden otros varios scripts para cambiar la dificultad del juego
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEditor.SpeedTree.Importer;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que contiene un singleton y métodos para acceder a sus "variables modificadoras de dificultad".
/// Permite configurar estas dificultades y seleccionar entre estas mediante métodos.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [System.Serializable]
    public struct Difficulty
    {
        public string DifficultyName;

        #region Modificadores multiplicadores
        // Enemigos
        public float MeleeChaseSpeedMultiplier;
        public float RangedChaseSpeedMultiplier;
        public float RangedFireRateMultiplier;

        // Ataques
        public float BulletSpeedMultiplier;
        public float ExplosionRadiusMultiplier;
        public float DynaTravelSpeedMultiplier;

        // Puntos
        public float PointsGivenMultiplier;
        #endregion

        #region Modificadores "de suma"
        // Suzie
        public int SuzieHealthAdded;
        public int SuziePelletsAdded;
        #endregion

        #region Constructora
        public Difficulty(string name = "Unnamed")
        {
            this.DifficultyName = name;

            this.MeleeChaseSpeedMultiplier = 1f;
            this.RangedChaseSpeedMultiplier = 1f;
            this.RangedFireRateMultiplier = 1f;

            this.BulletSpeedMultiplier = 1f;
            this.ExplosionRadiusMultiplier = 1f;
            this.DynaTravelSpeedMultiplier = 1f;

            this.PointsGivenMultiplier = 1f;

            this.SuzieHealthAdded = 0;
            this.SuziePelletsAdded = 0;
        }
        #endregion
    }

    /// <summary>
    /// Dificultades configuradas para el juego.
    /// Se puede crear de cualquier forma, pero para que los nombres de los métodos tengan sentidos
    /// se recomienda configurarlos con el orden: más fácil -> más dificil
    /// </summary>
    [SerializeField]
    private Difficulty[] difficulties;

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
    /// Guarda la instancia del singleton.
    /// </summary>
    private static DifficultyManager _instance = null;

    /// <summary>
    /// Número que indica el indice del array "difficulties" que esta activo actualmente.
    /// </summary>
    private int _currentDifficulty = 0;


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Llamado al cargarse en escena.
    /// Inicializa al componente como singleton en DontDestroyOnLoad, o se borra si ya existia.
    /// </summary>
    private void Awake()
    {
        if (_instance == null) // somos el primero
        {
            // En caso de que no se hayan configurado dificultades se usa una por defecto
            if (difficulties.Length == 0)
            {
                Debug.Log("No se han configurado dificultades en el DifficultyManager activo. Se usará una por defecto");
                difficulties = new Difficulty[1];
                difficulties[0] = new Difficulty();
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
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

    #region Variables de acceso
    public static DifficultyManager Instance
    {
        get
        {
            return _instance;
        }
    }

    #endregion

    #region Cambios de dificultad
    /// <summary>
    /// Aumenta el indice de dificultad actual en 1.
    /// Si es mayor que el posible por la cantidad de dificultades configuradas, tiene el efecto de ciclo; se reinicia a 0.
    /// </summary>
    public void IncreaseDifficulty()
    {
        _currentDifficulty = (_currentDifficulty + 1) % difficulties.Length;
    }
    /// <summary>
    /// Disminuye el indice de dificultad actual en 1.
    /// Si es menor que el posible por ser previamente 0, tiene el efecto de ciclo; se reinicia a difficulty.Length - 1.
    /// </summary>
    public void DecreaseDifficulty()
    {
        _currentDifficulty = (_currentDifficulty + difficulties.Length - 1) % difficulties.Length;
    }
    #endregion

    #region Métodos de acceso
    public static bool HasInstance()
    {
        return (_instance != null);
    }

    /// <summary>
    /// Método para acceder al nombre de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentDifficultyName()
    {
        return difficulties[_currentDifficulty].DifficultyName;
    }

    /// <summary>
    /// Método para acceder al MeleeChaseSpeedMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetMeleeChaseSpeedMultiplier()
    {
        return difficulties[_currentDifficulty].MeleeChaseSpeedMultiplier;
    }

    /// <summary>
    /// Método para acceder al RangedChaseSpeedMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetRangedChaseSpeedMultiplier()
    {
        return difficulties[_currentDifficulty].RangedChaseSpeedMultiplier;
    }

    /// <summary>
    /// Método para acceder al RangedFireRateMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetRangedFireRateMultiplier()
    {
        return difficulties[_currentDifficulty].RangedFireRateMultiplier;
    }

    /// <summary>
    /// Método para acceder al BulletSpeedMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetBulletSpeedMultiplier()
    {
        return difficulties[_currentDifficulty].BulletSpeedMultiplier;
    }

    /// <summary>
    /// Método para acceder al ExplosionRadiusMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetExplosionRadiusMultiplier()
    {
        return difficulties[_currentDifficulty].ExplosionRadiusMultiplier;
    }

    /// <summary>
    /// Método para acceder al DynaTravelSpeedMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetDynaTravelSpeedMultiplier()
    {
        return difficulties[_currentDifficulty].DynaTravelSpeedMultiplier;
    }

    /// <summary>
    /// Método para acceder al PointsGivenMultiplier de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public float GetPointsGivenMultiplier()
    {
        return difficulties[_currentDifficulty].PointsGivenMultiplier;
    }

    /// <summary>
    /// Método para acceder al SuzieHealthAdded de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public int GetSuzieHealthAdded()
    {
        return difficulties[_currentDifficulty].SuzieHealthAdded;
    }

    /// <summary>
    /// Método para acceder al SuziePelletsAdded de la dificultad actual.
    /// </summary>
    /// <returns></returns>
    public int GetSuziePelletsAdded()
    {
        return difficulties[_currentDifficulty].SuziePelletsAdded;
    }
    #endregion
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class DificultyManager 
// namespace
