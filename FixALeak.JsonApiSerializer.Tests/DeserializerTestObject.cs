using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FixALeak.JsonApiSerializer.Tests
{
    public class DeserializerTestObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }
        public float SomeFloat { get; set; }
        public decimal SomeDecimal { get; set; }
        public double SomeDouble { get; set; }
        public int DeserializerTestRelatedObjectID { get; set; }
        public DeserializerTestRelatedObject DeserializerTestRelatedObject { get; set; }
        public IEnumerable<DeserializerTestRelatedObject> DesCollection { get; set; }
        public ICollection<DeserializerTestRelatedObject> DesCollection2 { get; set; }
        public IList<DeserializerTestRelatedObject> DesCollection3 { get; set; }


        private bool _EqualsCollections(IEnumerable<DeserializerTestRelatedObject> col1, IEnumerable<DeserializerTestRelatedObject> col2)
        {
            if (col1 == null ^ col2 == null)
            {
                return false;
            }
            else if (col1 != null && col2 != null)
            {
                if (!Enumerable.SequenceEqual(col1, col2))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            var other = (DeserializerTestObject)obj;
             
            return _EqualsCollections(DesCollection, other.DesCollection) &&
                _EqualsCollections(DesCollection2, other.DesCollection2) &&
                _EqualsCollections(DesCollection3, other.DesCollection3) &&
                DeserializerTestRelatedObject.Equals(other.DeserializerTestRelatedObject) &&
                DeserializerTestRelatedObjectID == other.DeserializerTestRelatedObjectID &&
                ID == other.ID &&
                Title == other.Title &&
                Src == other.Src &&
                SomeDecimal == other.SomeDecimal &&
                SomeDouble == other.SomeDouble &&
                SomeFloat == other.SomeFloat;
         
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + ID.GetHashCode();
            hash = (hash * 7) + Title  .GetHashCode();
            hash = (hash * 7) + Src.GetHashCode();
            hash = (hash * 7) + DeserializerTestRelatedObject.GetHashCode();
            hash = (hash * 7) + SomeDecimal.GetHashCode();
            hash = (hash * 7) + SomeFloat.GetHashCode();
            hash = (hash * 7) + SomeDouble.GetHashCode();
            DesCollection.ToList().ForEach(x => { hash = (hash * 7) + x.GetHashCode(); });
            DesCollection2.ToList().ForEach(x => { hash = (hash * 7) + x.GetHashCode(); });
            DesCollection3.ToList().ForEach(x => { hash = (hash * 7) + x.GetHashCode(); });
            return hash;
           
        }

    }

    public class DeserializerTestRelatedObject
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var other = (DeserializerTestRelatedObject)obj;
            return ID == other.ID && Name == other.Name;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + ID.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            return hash;
        }
    }
}
