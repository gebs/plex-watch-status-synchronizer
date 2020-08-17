using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace plexapi
{
    public class PlexMediaComparer : IEqualityComparer<PlexMedia>
    {
        public bool Equals([AllowNull] PlexMedia x, [AllowNull] PlexMedia y)
        {
            return x.Guid == y.Guid;
        }

        public int GetHashCode([DisallowNull] PlexMedia obj)
        {
            return obj.Guid.GetHashCode();
        }
    }
}
