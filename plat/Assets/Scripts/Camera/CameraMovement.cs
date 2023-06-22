using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toon.Extend;

namespace Toon
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        float deadZone = .001f;
        [SerializeField]
        float sensiX = 1.2f, sensiY = 1.2f;
        [SerializeField]
        TerrainCollider terrainCol;

        Vector3 lookAt = new(0, 1, 0), cameraPlayerDistance = new(0, 2, -4);
        GameObject player;
        float angleX = 0.0f, angleY = 0.0f;


        void Start()
        {
            player = GameObject.FindGameObjectWithTag(constant.Player);
            terrainCol ??= GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainCollider>();
        }

        void Update()
        {
            ViewRotation(sensiX, sensiY, deadZone);
            FollowPlayer(Quaternion.Euler(angleX, angleY, 0) * cameraPlayerDistance, lookAt);
            Raying();
        }

        bool hitTerrain = false;
        /// <summary>
        /// めり込み対策
        /// </summary>
        void Raying()
        {
            Ray r2 = new(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward);
            var dis = Vector3.Distance(transform.position, player.transform.position) + 1;
            if (!hitTerrain && Physics.Raycast(r2, out var hit2, dis))
            {
                // TODO terrainにめり込む ///////////////////////////////////////////////
                //? https://forum.unity.com/threads/physics-raycast-not-working-for-terrain.412005/
                if (!hit2.collider.compare(constant.Player))
                    transform.position = hit2.point;
            }
            if (terrainCol.Raycast(r2, out var hit3, dis))
            {
                hitTerrain = true;
                transform.position = hit3.point;
            }
        }

        /// <summary>
        /// 視点移動
        /// </summary>
        /// <param name="_sensiX">縦移動感度</param>
        /// <param name="_sensiY">横移動感度</param>
        /// <param name="_deadZone">入力感度の調整</param>
        void ViewRotation(float _sensiX, float _sensiY, float _deadZone)
        {
            float mx = Input.GetAxis(constant.MouseX),
                my = Input.GetAxis(constant.MouseY);

            // プレイヤーを中心に回転させる
            if (Mathf.Abs(mx) >= _deadZone)
                angleY += mx * _sensiY;

            if (Mathf.Abs(my) >= _deadZone)
                angleX -= my * _sensiX;
        }

        /// <summary>
        /// プレイヤーを追随
        /// </summary>
        /// <param name="_posDis">プレイヤーとカメラの距離</param>
        /// <param name="_lookAt">カメラがみる座標</param>
        void FollowPlayer(Vector3 _posDis, Vector3 _lookAt)
        {
            this.transform.position = player.transform.position + _posDis;
            this.transform.LookAt(player.transform.position + _lookAt);
        }

        async Task tasktest()
        {
            while (true)
            {
                await Task.Delay(1 * 1000);
                "call".show();
            }
        }
    }
}
