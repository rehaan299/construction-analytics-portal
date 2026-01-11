using Xunit;

public class AlertRulesTests
{
    [Fact]
    public void BasicMath_Sanity()
    {
        var budget = 100m;
        var actual = 90m;
        var pct = actual / budget;
        Assert.True(pct >= 0.9m);
    }
}
