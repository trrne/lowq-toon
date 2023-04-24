using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [System.Serializable]
    class V
    {
        /// <summary>
        /// 基本の移動速度
        /// </summary>
        public float basis;
        /// <summary>
        /// 減速比
        /// </summary>
        public float reduction;
        public V(float basis, float reduction)
        {
            this.basis = basis;
            this.reduction = reduction;
        }

        /// <summary>
        /// ジャンプ力
        /// </summary>
        public float power;
        public V(float power)
        {
            this.power = power;
        }
    }

    GameObject self;
    new GameObject camera;
    Rigidbody rb;
    CameraMovement cameraMovement;
    Quaternion q;

    // [SerializeField]
    // V v;

    V m, j;

    Vector3 selfPos;

    bool isMoving = false;
    /// <summary>
    /// 動いていたらtrue
    /// </summary>
    public bool IsMoving => isMoving;

    bool isFloating = false;
    /// <summary>
    /// 地に足がついていたらfalse
    /// </summary>
    public bool IsFloating => isFloating;

    /// <summary>
    /// プレイヤーの回転速度
    /// </summary>
    float rotSpeed = 10;

    float velocity;
    /// <summary>
    /// プレイヤーの移動速度
    /// </summary>
    public float Velocity => velocity;

    // async Task<int> test()
    // {
    //     print(0);
    //     await Task.Delay(2000);
    //     print(1);
    //     return 0;
    // }

    void Start()
    {
        self = this.gameObject;
        selfPos = this.gameObject.transform.position;

        m = new(basis: 15, reduction: 0.5f);
        j = new(power: 200);

        //q = new();

        rb = this.gameObject.GetComponent<Rigidbody>();
        cameraMovement = camera.GetComponent<CameraMovement>();
        try
        {
            camera = GameObject.FindGameObjectWithTag(Mine.Tags.Cam);
        }
        catch
        {
            camera = GameObject.Find("cam0");
        }
    }

    void FixedUpdate()
    {
        Moves(m.basis, m.reduction);
    }

    void Update()
    {
        if (!isFloating && Input.GetButton("Jump"))
        {
            Jumps(j.power);
        }
        Rots(1);
    }

    /// <summary>
    /// プレイヤーの移動、回転処理
    /// </summary>
    /// <param name="basis">基本の移動速度</param>
    /// <param name="reductionRatio">減速比</param>
    void Moves(float basis, float reductionRatio)
    {
        float h = Input.GetAxisRaw("Horizontal"), v = Input.GetAxisRaw("Vertical");
        // カメラの向きを起点に前後左右に動く
        Vector3 hv = h * camera.transform.right + v * camera.transform.forward;
        hv.y = 0.0f;
        velocity = hv.magnitude;
        isMoving = velocity > .01f;
        if (!isMoving)
        {
            rb.velocity *= reductionRatio;
            return;
        }

        rb.velocity += hv * basis * Time.deltaTime;
        q.SetLookRotation(view: hv, up: Vector3.up);
        self.transform.rotation = Quaternion.Lerp(
            transform.rotation, q, rotSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 移動したら回転
    /// </summary>
    void Rots(float speed)
    {
        // プレイヤーが移動していて視点と違う方向を向いていたらゆっくり視点の方向に回転
        // カメラとプレイヤーが5度以上ずれていたら
        if (!isMoving && (this.transform.localEulerAngles.y - cameraMovement.Rotation.y) <= 5)
        {
            return;
        }
        print("in rots");
        // this.transform.rotation = Quaternion.Lerp(transform.rotation, cameraMovement.Rotation, speed);
    }

    /// <summary>
    /// 発砲したら回転
    /// </summary>
    void Rotss()
    {
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    /// <param name="power">脚力</param>
    void Jumps(float power)
    {
        rb.AddForce(Vector3.up * power, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Ground"))
        {
            isFloating = false;
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Ground"))
        {
            isFloating = true;
        }
    }
}