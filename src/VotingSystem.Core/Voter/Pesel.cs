using System;

namespace VotingSystem.Core.Voter
{
    public class Pesel : IEquatable<Pesel>, IEquatable<string>
    {
        public Pesel(string? pesel)
        {
            if (string.IsNullOrWhiteSpace(pesel))
            {
                throw new EmptyPeselException();
            }
            
            Code = pesel;
        }

        public string Code { get; }

        public bool Equals(Pesel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code, StringComparison.InvariantCulture);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Pesel) obj);
        }

        public bool Equals(string other) => 
            string.Equals(other, Code, StringComparison.InvariantCulture);

        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString() => Code;

        public static Pesel ValidateAndCreate(string? pesel)
        {
            if (string.IsNullOrWhiteSpace(pesel))
            {
                throw new EmptyPeselException();
            }

            // deliberately do not do full validation
            // if you want - do it
            if (pesel.Length != 11)
            {
                throw new InvalidPeselException(pesel);
            }

            return new Pesel(pesel);
        }
    }
}