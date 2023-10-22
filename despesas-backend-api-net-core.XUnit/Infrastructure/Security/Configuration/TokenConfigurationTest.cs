﻿using despesas_backend_api_net_core.Infrastructure.Security.Configuration;

namespace Test.XUnit.Infrastructure.Security.Configuration
{
    public class TokenConfigurationTest
    {
        [Fact]
        public void Properties_Should_Be_Set_Correctly()
        {
            // Arrange
            var tokenConfiguration = new TokenConfiguration();

            // Act
            tokenConfiguration.Audience = "TesteAudience";
            tokenConfiguration.Issuer = "TesteIssuer";
            tokenConfiguration.Seconds = 3600; // 1 hour

            // Assert
            Assert.Equal("TesteAudience", tokenConfiguration.Audience);
            Assert.Equal("TesteIssuer", tokenConfiguration.Issuer);
            Assert.Equal(3600, tokenConfiguration.Seconds);
        }
    }
}
