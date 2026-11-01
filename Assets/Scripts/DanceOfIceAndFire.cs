using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class DanceOfIceAndFire : MonoBehaviour
{
    [field: SerializeField] public Transform Circle1 { get; private set; }
    [field: SerializeField] public Transform Circle2 { get; private set; }

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    public float Distance { get; set; } = 1f;
    public float RotateSpeed { get; set; } = 2f;
    public bool RightRotate { get; set; } = true;

    private Transform _rotateCircleTrm;
    private Transform _centerCircleTrm;

    private float _x;
    private float _y;
    private float _timer;
    
    private void Awake()
    {
        _rotateCircleTrm = Circle1;
        _centerCircleTrm = Circle2;
        
        _cinemachineCamera.Target.TrackingTarget = _rotateCircleTrm;
    }

    private void Update()
    {
        Input();
        
        if (RightRotate) // 물체가 삼각 함수에 따라 회전하는 건 삼각 함수() 매개변수에 값에 따라 다름 음수면 왼 -> 오, else 오 -> 왼
        {
            _timer -= Time.deltaTime * RotateSpeed;
        }
        else
        {
            _timer += Time.deltaTime * RotateSpeed;
        }
        
        RotateCircle(_timer);
    }

    private void Input()
    {
        if(Keyboard.current.anyKey.wasPressedThisFrame)
        {
            CheckTile();
        }
    }

    private void CheckTile()
    {
        Vector3Int cellPosition = _tilemap.WorldToCell(_rotateCircleTrm.position);

        if (_tilemap.HasTile(cellPosition))
        {
            Vector3 worldPosition = _tilemap.GetCellCenterWorld(cellPosition);
            _rotateCircleTrm.position = worldPosition;
            
            Move();
            Sound();
        }
    }

    
    private void Move()
    {
        Transform temp = _rotateCircleTrm;
        _rotateCircleTrm = _centerCircleTrm;
        _centerCircleTrm = temp;

        Vector3 direction = _rotateCircleTrm.position - _centerCircleTrm.position;

        _timer = Mathf.Atan2(direction.y, direction.x);

        _cinemachineCamera.Target.TrackingTarget = _rotateCircleTrm;
    }

    private void RotateCircle(float time)
    {
        if (_rotateCircleTrm == null || _centerCircleTrm == null) return;

        _x = Distance * Mathf.Cos(time);
        _y = Distance * Mathf.Sin(time);

        _rotateCircleTrm.position = _centerCircleTrm.position + new Vector3(_x, _y, 0f);
    }
    # region Settings
    
    public void SetSpeedUp()
    {
        RotateSpeed += 1;
    }

    public void SetSpeedDown()
    {
        RotateSpeed -= 1;
    }

    public void SetDistanceUp()
    {
        Distance += 1;
    }

    public void SetDistanceDown()
    {
        Distance -= 1;
    }

    public void SetCameraSizeUp()
    {
        _cinemachineCamera.Lens.OrthographicSize += 1;
    }

    public void SetCameraSizeDown()
    {
        if (_cinemachineCamera.Lens.OrthographicSize <= 1) return;
        
        _cinemachineCamera.Lens.OrthographicSize -= 1;
    }
    
    public void SetBool()
    {
        RightRotate = !RightRotate;
    }

    public void ResetAll()
    {
        SceneManager.LoadScene(0);
    }

    private void Sound()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    # endregion
}
