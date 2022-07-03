using UnityEngine;

namespace Code.Game.Utils
{
    public class ContactPoller
    {
        private const float _collitionThresh = 0.5f;

        private ContactPoint2D[] _contacts = new ContactPoint2D[10];
        private int _contactsCount;
        private readonly Collider2D _collider2D;
        
        public bool HasUpContacts { get; private set; }
        public bool HasDownContacts { get; private set; }
        public bool HasLeftContacts { get; private set; }
        public bool HasRightContacts { get; private set; }

        public ContactPoller(Collider2D collider2D)
        {
            _collider2D = collider2D;
        }

        public void Update()
        {
            HasUpContacts = false;
            HasDownContacts = false;
            HasLeftContacts = false;
            HasRightContacts = false;

            _contactsCount = _collider2D.GetContacts(_contacts);

            for (int i = 0; i < _contactsCount; i++)
            {
                var normal = _contacts[i].normal;

                if (normal.y > _collitionThresh) HasUpContacts = true;
                if (normal.y < -_collitionThresh) HasDownContacts = true;
                if (normal.x > _collitionThresh) HasLeftContacts = true;
                if (normal.x < -_collitionThresh) HasRightContacts = true;
            }
        }
    }
}