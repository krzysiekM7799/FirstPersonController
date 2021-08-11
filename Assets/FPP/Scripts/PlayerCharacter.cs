using UnityEngine;

namespace FPP
{
    public class PlayerCharacter : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public Vector2 transformedMove;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 move, float speed)
        {
            Vector3 transformedVector = move.y * transform.forward + move.x * transform.right;
            transformedMove.x = transformedVector.x;
            transformedMove.y = transformedVector.z;
            float currentYVelocity = _rigidbody.velocity.y;
            _rigidbody.velocity = new Vector3(transformedMove.x * speed, currentYVelocity, transformedMove.y * speed);
        }

        public void Jump(float jumpStrengh)
        {
          _rigidbody.AddForce(new Vector3(0, jumpStrengh, 0), ForceMode.Impulse);
        }
    }
}
