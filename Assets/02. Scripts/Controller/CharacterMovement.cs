using UnityEngine;

namespace Controller
{
    public class CharacterMovement
    {
        private readonly Rigidbody _rigidbody;
        public CharacterMovement(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

    }
}

