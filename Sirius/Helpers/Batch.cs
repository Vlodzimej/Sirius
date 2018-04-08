using System;

namespace Sirius.Helpers
{
    public class Batch : IEquatable<Batch>
    {
        public double Amount { get; set; }
        public decimal Cost { get; set; }

        public bool Equals(Batch other)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Amount.Equals(other.Amount) && Cost.Equals(other.Cost);
        }

        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null. 
            int hashProductName = Amount == null ? 0 : Amount.GetHashCode();

            //Get hash code for the Code field. 
            int hashProductCode = Cost.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName ^ hashProductCode;
        }
    }
}