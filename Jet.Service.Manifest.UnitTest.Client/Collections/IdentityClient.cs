using Jet.Service.Manifest.UnitTest.Client.Fixtures;
using Xunit;

namespace Jet.Service.Manifest.UnitTest.Client.Collections
{
    [CollectionDefinition("IdentityClient")]
    public class IdentityClientTestColl : ICollectionFixture<IdentityServiceFixture>
    {
    }
}
