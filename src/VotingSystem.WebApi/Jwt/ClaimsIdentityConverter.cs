using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using SharpDomain.AccessControl;
using SharpDomain.AccessControl.AspNetCore;

namespace VotingSystem.WebApi.Jwt
{
    internal class ClaimsIdentityConverter : IClaimsIdentityConverter
    {
        private readonly Dictionary<Type, object> _claimsFactories = new();
        private readonly Dictionary<Type, object> _identityFactories = new();
        
        public IEnumerable<Claim> GetClaims<TIdentity>(TIdentity identity) 
            where TIdentity : IIdentity
        {
            if (_claimsFactories.TryGetValue(typeof(TIdentity), out var factory))
            {
                var factoryTyped = (Func<TIdentity, IEnumerable<Claim>>)factory;
                var claims = factoryTyped(identity);
                
                return claims;
            }
            
            throw new InvalidOperationException(
                $"cannot create claims for type {typeof(TIdentity).FullName}");
        }
        
        public bool TryGetIdentity<TIdentity>(IEnumerable<Claim> claims, [NotNullWhen(true)] out TIdentity identity)
            where TIdentity : IIdentity
        {
            if (_identityFactories.TryGetValue(typeof(TIdentity), out var factory))
            {
                var factoryTyped = (Func<IReadOnlyList<Claim>, TIdentity>)factory;
                
                var identityNullable = factoryTyped(claims.ToArray());
                if (!identityNullable.IsValid())
                {
                    identity = default!;
                    return false;
                }
                
                identity = identityNullable;
                return true;
            }
            
            throw new InvalidOperationException(
                $"cannot create identity for type {typeof(TIdentity).FullName}");
        }
        
        public void ConfigureClaimsConverter<TIdentity>(Func<TIdentity, IEnumerable<Claim>> claimsFactory)
        {
            _claimsFactories.Add(typeof(TIdentity), claimsFactory);
        }
        
        public void ConfigureIdentityConverter<TIdentity>(Func<IReadOnlyList<Claim>, TIdentity> identityFactory)
        {
            _identityFactories.Add(typeof(TIdentity), identityFactory);
        }
    }
}