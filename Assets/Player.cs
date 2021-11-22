using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dninemfive.cmpm121.p3
{
    public class Player : MonoBehaviour
    {
        public static GameObject ActivePlayer;
        public CharacterController controller;
        public float speed = 10;
        // Start is called before the first frame update
        void Start()
        {
            ActivePlayer = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            float v = Input.GetAxis("Vertical"),
                  h = Input.GetAxis("Horizontal");
            Vector3 velocity = Vector3.zero;
            velocity += transform.forward * v * speed;
            velocity += transform.right * h * speed;
            controller.SimpleMove(velocity);
            float rot = 0;
            if (Input.GetKey(KeyCode.Q)) rot -= 1;
            if (Input.GetKey(KeyCode.E)) rot += 1;
            Vector3 angle = transform.eulerAngles;
            angle.y += rot * Time.deltaTime * 60;
            transform.eulerAngles = angle;
        }
    }
}
