using System;

namespace NHibernateWorkshop.Models
{
    [Serializable]
    public class Entity
    {
        private int? oldHashCode;
        public virtual Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (other == null) { return false; }

            // handle the case of comparing two NEW objects

            var otherIsTransient = Equals(other.Id, Guid.Empty);
            var thisIsTransient = Equals(Id, Guid.Empty);

            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (oldHashCode.HasValue) { return oldHashCode.Value; }

            var thisIsTransient = Equals(Id, Guid.Empty);

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (thisIsTransient)
            {
                oldHashCode = base.GetHashCode();
                return oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}#{1}", GetType().Name, Id);
        }
    }
}