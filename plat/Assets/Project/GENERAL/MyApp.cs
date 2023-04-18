using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mine
{
    public static class Scenes
    {
        public static string
            Main = "Main"
            ;
    }

    public class Keys
    {
        public static string
            Hor = "Horizontal",
            Ver = "Vertical",
            Jump = "Jump",
            MX = "Mouse X",
            MY = "Mouse Y"
            ;
    }

    public class Tags
    {
        public static string
            Player = "Player",
            Ground = "Ground",
            MainCam = "MainCamera",
            Cam = "Cam"
            ;
    }

    public class Script : MonoBehaviour
    {
        /// <summary>
        /// 自分用printテスト
        /// </summary>
        /// <param name="msg">出力するテキスト</param>
        new public static void print(object msg) => Debug.Log(msg);

        public static float Randfloat(float min, float max = 0)
        => UnityEngine.Random.Range(min, max);

        public static int Randint(int min = 0, int max = 0)
        => UnityEngine.Random.Range(min, max);

        public static string Randstring(int? count)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            // var charasArr = new char[(int)count];
            char[] charaArr = count == null ?
                new char[Mine.Script.Randint(2, 16)] : new char[(int)count];
            System.Random random = new();
            for (int i = 0; i < charaArr.Length; i++)
                charaArr[i] = characters[random.Next(characters.Length)];
            return charaArr.ToString();
        }

        public static GameObject Randins(
            GameObject[] objects, Vector3 vec3, Quaternion quaternion)
        => Instantiate(objects[Randint(max: objects.Length)], vec3, quaternion);

        public static GameObject Ins(
            GameObject @object, Vector3 v3, Quaternion quaternion)
        => Instantiate(@object, v3, quaternion);

        public void FollowCursor(float z = 0)
        {
            var cursor = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursor.z = z;
            transform.position = cursor;
        }

        public void Move2D(
            float? speed = null, string _hor = "Horizontal", string _ver = "Vertical")
        {
            float hor = Input.GetAxis(_hor), ver = Input.GetAxis(_ver);
            Vector2 inputAxis = new(hor, ver);
            speed = speed != null ? speed : 20;
            transform.Translate(inputAxis * (float)speed * Time.deltaTime);
        }

        public static void Jump2D(Rigidbody2D rb, float jumpingPower = 5f)
        => rb.AddForce(Vector3.up * jumpingPower, ForceMode2D.Impulse);

        /// <summary>
        /// 破壊
        /// </summary>
        public static void RemoveObject(float lifeTime = 0)
        {
            Component comp = new();
            Destroy(comp.gameObject, lifeTime);
        }

        /// <summary>
        /// クリックでオブジェクト取得
        /// <param name="click">0: left, 1: right</param>
        /// </summary>
        public void GetObject(int click = 0)
        {
            if (Input.GetMouseButtonDown(click))
            {
                Vector3 or = transform.position, dir = new(100, 0, 0);
                RaycastHit2D h = Physics2D.Raycast(or, dir);
                if (h.collider)
                {
                    var pos = h.collider.gameObject.transform.position;
                    Debug.Log($"position:{pos}");
                }
            }
        }

        public static float ComputeFps() => Mathf.Floor(1 / Time.deltaTime);

        public static string Timer(int digits)
        => Time.time.ToString("F" + digits);

        public static void VisibleCursor(bool isVisible = false)
        => Cursor.visible = isVisible;

        public static void AudioSlider(AudioSource audioSource, float volume = .01f)
        => audioSource.volume = volume;

        public void LimitRange(float x, float y, float? z)
        {
            var tp = transform.position;
            Vector3 coords = new(
                Mathf.Clamp(tp.x, -x, x),
                Mathf.Clamp(tp.y, -y, y),
                Mathf.Clamp(tp.z, (float)-z, (float)z)
            );
            transform.position = coords;
        }

        public static string CurrentTime()
        {
            var now = DateTime.Now;
            var current = $"{now.Hour}:{now.Minute}:{now.Second}";
            return current.ToString();
        }

        public static IEnumerator Animation(
            Sprite[] sprites,
            SpriteRenderer sr,
            float? animeTime = null
        )
        {
            int i = 0;
            while (true)
            {
                i = i >= sprites.Length - 1 ? 0 : i + 1;
                sr.sprite = sprites[i];
                animeTime = animeTime != null ? animeTime : .05f;
                yield return new WaitForSeconds((float)animeTime);
            }
        }

        public static void ReloadScene(string name)
        => SceneManager.LoadScene(
            name is null ? SceneManager.GetActiveScene().name : name);
    }

    // public class TestException : System.Exception
    // {
    //     public TestException(string exceptMsg = null) : base(exceptMsg) { }
    // }
}
