//---------------------------------------------------------
// Singleton que maneja el audio del juego usando una AudioSource Pool.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que contiene métodos públicos para hacer sonar clips en posiciones indicas (SFX) y
/// música (con el AudioSource que ha de tener este manager).
/// Para que funcione se le debe asignar un AudioSourcePrefab con sonido en 3D (para que sea posicional)
/// mientras que el AudioSource del manager ha de ser 2D (para la música).
/// También tiene parámetros de volumen (editables desde el editor y métodos públicos) para SFX y música.
/// 
/// Al llamar a Play() o se instancia un AudioSourcePrefab o se mueve uno existente que no este funcionando
/// para poner el clip indicado en la posición indicada.
/// 
/// +++
/// Añadida función para ralentizar la música en función de la habilidad del tiempo
/// </summary>
public class AudioManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Almacena un prefab de AudioSource que instanciará hasta un máximo de AudioSources asignable.
    /// </summary>
    [SerializeField]
    private AudioSource AudioSourcePrefab;

    /// <summary>
    /// Determina cuantas AudioSources puede haber como máximo (cuántos sonidos puede haber a la vez).
    /// </summary>
    [SerializeField]
    private int MaxAudioSources = 30;

    /// <summary>
    /// Parámetro de volumen de SFX. 
    /// Tiene métodos para leer y modificarlo.
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    private float VolumeSFX = 1f;

    /// <summary>
    /// Parámetro de volumen de música.
    /// Tiene métodos para leer y modificarlo.
    /// </summary>
    [SerializeField, Range(0f, 1f)]
    private float VolumeMusic = 1f;

    /// <summary>
    /// Velocidad a la que se realiza la transición de volumen entre fases.
    /// Un valor menor hace que el fundido sea más lento.
    /// </summary>
    [SerializeField, Range(0.1f, 5f)]
    private float FadeSpeed = 0.5f;
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
    /// Almacena la instancia del singleton.
    /// </summary>
    static private AudioManager _instance;

    /// <summary>
    /// Lleva la lista de AudioSources instanciados. 
    /// Inicializado en el Awake().
    /// </summary>
    private AudioSource[] _poolAudioSources;

    /// <summary>
    /// Indice que indica el primer AudioSource ocupado de la lista (por detrás estan libres).
    /// </summary>
    private int _firstOccupiedAudioSource = 0;

    /// <summary>
    /// Contador que indica cuántas AudioSources han sido creadas.
    /// Este 0 indica el máximo indice que no tiene AudioSource null.
    /// </summary>
    private int _createdAudioSources = 0;

    /// <summary>
    /// Almacena el AudioSource del AudioManager.
    /// Servirá para la música.
    /// </summary>
    private AudioSource _mySource;

    /// <summary>
    /// Fuente secundaria para la fase 2 de Suzie
    /// </summary>
    private AudioSource _fase2Source;

    /// <summary>
    /// Pitch objetivo al que se deben extender los audios 
    /// </summary>
    private float _targetPitch = 1f;

    /// <summary>
    /// Pitch actual que se está aplicando a los audios.
    /// </summary>
    private float _currentPitch = 1f;

    /// <summary>
    /// Velocidad a la que se transiciona el pitch.
    /// </summary>
    private float _pitchTransitionSpeed = 5f;

    /// <summary>
    /// Volumen objetivo de la pista suave (Fase 1).
    /// </summary>
    private float _targetFase1Vol = 1f;

    /// <summary>
    /// Volumen objetivo de la pista metalizada (Fase 2).
    /// </summary>
    private float _targetFase2Vol = 0f;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama al cargarse en escena.
    /// Realiza comprobaciones necesarias para el componente y si tiene éxito
    /// se establece como _instance y se mete al DontDestroyOnLoad.
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            if (AudioSourcePrefab == null)
            {
                Debug.Log("Al AudioManager no se le ha asigando AudioSourcePrefab y no funcionará");
                Destroy(gameObject);
                return;
            }
            _mySource = GetComponent<AudioSource>();
            if (_mySource == null)
            {
                Debug.Log("El AudioManager no tiene componente AudioSource y no funcionará");
                Destroy(gameObject);
                return;
            }

            // Inicialización de la fuente para la música metalizada (Fase 2)
            _fase2Source = gameObject.AddComponent<AudioSource>();
            _fase2Source.loop = true;
            _fase2Source.playOnAwake = false;
            _fase2Source.spatialBlend = 0; // Sonido 2D para música

            _poolAudioSources = new AudioSource[MaxAudioSources];
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else // no somos la primera instancia
        {
            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// Se llama cada frame mientras el componente este activo.
    /// Revisa las AudioSource que estan marcadas como ocupadas para ver si ya
    /// han acabado.
    /// </summary>
    private void Update()
    {
        for (int i = _firstOccupiedAudioSource; i < _createdAudioSources; i++)
        {
            if (!_poolAudioSources[i].isPlaying) // AudioSource liberado
            {
                FreeAudioSource(i);
            }
        }

        // Lógica de Crossfade: Transiciona los volúmenes suavemente hacia sus objetivos
        _mySource.volume = Mathf.MoveTowards(_mySource.volume, _targetFase1Vol * VolumeMusic, Time.unscaledDeltaTime * FadeSpeed);
        _fase2Source.volume = Mathf.MoveTowards(_fase2Source.volume, _targetFase2Vol * VolumeMusic, Time.unscaledDeltaTime * FadeSpeed);

        if (_currentPitch != _targetPitch)
        {
            // MoveTowards garantiza alcanzar el 1.0 o el 0.5 exacto. 
            // unscaledDeltaTime ignora la ralentización del tiempo del juego.
            _currentPitch = Mathf.MoveTowards(_currentPitch, _targetPitch, Time.unscaledDeltaTime * _pitchTransitionSpeed);

            // Aplicar a la música
            if (_mySource != null)
            {
                _mySource.pitch = _currentPitch;
            }

            // Aplicar a todos los SFX
            for (int i = 0; i < _createdAudioSources; i++)
            {
                if (_poolAudioSources[i] != null)
                {
                    _poolAudioSources[i].pitch = _currentPitch;
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
    /// Variable de acceso a la instancia del AudioManager.
    /// </summary>
    static public AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// Indica si existe instancia de AudioManager.
    /// </summary>
    /// <returns></returns>
    static public bool HasInstance()
    {
        return (_instance != null);
    }

    /// <summary>
    /// Intenta hacer sonar el clip indicado.
    /// 
    /// Puede no sonar si:
    /// 1) Estan todos los AudioSource ocupados y
    /// 2) Se ha alcanzado la máxima capacidad de AudioSources.
    /// En ese caso no hará nada.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    public void Play(AudioClip clip, Vector3 position)
    {
        // Creacion de AudioSource si es necesario.
        if (_firstOccupiedAudioSource == 0)
        {
            if (_createdAudioSources < MaxAudioSources) // si es posible
            {
                ExtendAudioSourcePool();
                PlayNMove(clip, position);
            }
            // si no es posible no hace nada.
        }
        else if (_firstOccupiedAudioSource > 0) // solo si hay AudioSource libre
        {
            PlayNMove(clip, position);
        }
    }

    /// <summary>
    /// Hace sonar una música en el AudioSource del AudioManager.
    /// </summary>
    /// <param name="music"></param>
    public void PlayMusic(AudioClip music)
    {
        if (_mySource.isPlaying) _mySource.Stop();
        if (_fase2Source != null && _fase2Source.isPlaying) _fase2Source.Stop();

        _mySource.clip = music;
        _mySource.volume = VolumeMusic;
        _mySource.loop = true;
        _mySource.Play();

        // Reseteamos los objetivos de volumen para evitar bugs en futuros combates
        _targetFase1Vol = 1f;
        _targetFase2Vol = 0f;
    }

    /// <summary>
    /// Para la música.
    /// </summary>
    public void StopMusic()
    {
        _mySource.Stop();
        _fase2Source.Stop();
    }

    /// <summary>
    /// Establece el objetivo del pitch para simular la cámara lenta en el audio.
    /// </summary>
    public void SetSlowMotionAudio(bool isSlow)
    {
        _targetPitch = isSlow ? 0.5f : 1f;
    }

    /// <summary>
    /// Inicia ambas versiones de la canción (normal y metal) simultáneamente y en silencio la segunda.
    /// </summary>
    public void StartDoublePhaseMusic(AudioClip suave, AudioClip metal)
    {
        _mySource.clip = suave;
        _fase2Source.clip = metal;

        _mySource.volume = VolumeMusic;
        _fase2Source.volume = 0f;

        _mySource.Play();
        _fase2Source.Play();

        _targetFase1Vol = 1f;
        _targetFase2Vol = 0f;
    }

    /// <summary>
    /// Cambia el objetivo de volumen para que la pista normal baje y la metalizada suba.
    /// </summary>
    public void TransitionToPhase2Music()
    {
        _targetFase1Vol = 0f;
        _targetFase2Vol = 1f;
    }
    #region Métodos públicos para el volumen
    /// <summary>
    /// Método para saber cuál es el volumen actual de los SFX configurado en el AudioManager.
    /// Servirá para representarlo en el menú de ajustes.
    /// </summary>
    /// <returns></returns>
    public float GetSFXVolume()
    {
        return VolumeSFX;
    }

    /// <summary>
    /// Método para establecer el volumen de los SFX del AudioManager.
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        VolumeSFX = volume;
    }


    /// <summary>
    /// Método para saber cuál es el volumen actual de la música configurado en el AudioManager.
    /// Servirá para representarlo en el menú de ajustes.
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        return VolumeMusic;
    }

    /// <summary>
    /// Método para establecer el volumen de los música del AudioManager.
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        VolumeMusic = volume;
        _mySource.volume = VolumeMusic;
    }
    #endregion

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Hace sonar en el último AudioSource libre el clip indicado y lo coloca en la posicion
    /// indicada.
    /// (!) No comprueba que haya AudioSources libres. Ha de ser llamado solo si los hay.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    private void PlayNMove(AudioClip clip, Vector3 position)
    {
        _poolAudioSources[_firstOccupiedAudioSource - 1].PlayOneShot(clip);
        _poolAudioSources[_firstOccupiedAudioSource - 1].transform.position = position;
        _poolAudioSources[_firstOccupiedAudioSource - 1].volume = VolumeSFX;
        _poolAudioSources[_firstOccupiedAudioSource - 1].pitch = _currentPitch;
        OccupyAudioSource();

    }

    /// <summary>
    /// Crea una nueva AudioSource, añadiendola a la pool, y la marca como liberada.
    /// (!) No verifica si la acción es posible, solo ha de llamarse si es así.
    /// </summary>
    private void ExtendAudioSourcePool()
    {
        AudioSource newAS = Instantiate(AudioSourcePrefab, transform.position, transform.rotation, transform); // crea el objeto como hijo del AudioManager

        _poolAudioSources[_createdAudioSources] = newAS;

        FreeAudioSource(_createdAudioSources);
        _createdAudioSources++;
    }

    /// <summary>
    /// Marca la última AudioSource libre como ocupada.
    /// (!) Asume que hay al menos una AudioSource libre y solo ha de llamarse si es así.
    /// (!) El clip por hacer sonar ha de ser enviado a este último AudioSource libre.
    /// </summary>
    private void OccupyAudioSource()
    {
        _firstOccupiedAudioSource--;
    }

    /// <summary>
    /// Coloca en la primera posición de AudioSource ocupada el AudioSource recién liberado.
    /// </summary>
    /// <param name="audioSourcePosition"></param>
    private void FreeAudioSource(int audioSourcePosition)
    {
        (_poolAudioSources[_firstOccupiedAudioSource], _poolAudioSources[audioSourcePosition]) = (_poolAudioSources[audioSourcePosition], _poolAudioSources[_firstOccupiedAudioSource]);
        _firstOccupiedAudioSource++;
    }
    #endregion

} // class AudioManager 
// namespace
