namespace TrendFox.Blazor.Regions.Tests;

public class ComponentParameterBuilderTests
{
    private const string LambdaMustTargetTestcomponentPropertyMessage = "propertySelector lambda must be an expression targetting a single property of TestComponent.";
    private const string CouldNotIdentifyMemberMessage = "Could not identify member name from lambda.";
    private readonly IComponentParameterBuilder<TestComponent> _parameters = new ComponentParameterBuilder<TestComponent>();

    [Fact]
    public void AddWorks()
    {
        // Arrange
        var expectedValue = "";
        // Act
        _parameters.Add(p => p.Text, expectedValue);
        var actual = _parameters.Build();
        // Assert
        Assert.Contains(nameof(TestComponent.Text), actual.Keys);
        Assert.Equal(expectedValue, actual[nameof(TestComponent.Text)]);
    }

    [Fact]
    public void AddNullThrowsArgumentNullException()
    {
        // Arrange
        // Assert
        var actualException = Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _parameters.Add(null!, "");
        });
        Assert.Equal("propertySelector", actualException.ParamName);
    }

    [Fact]
    public void AddInvalidExpressionThrowsInvalidOperationException()
    {
        // Arrange
        // Assert
        var actualException = Assert.Throws<InvalidOperationException>(() =>
        {
            // Act
            _parameters.Add(p => "", "");
        });
        Assert.Equal("Could not identify member name from lambda.", actualException.Message);
    }

    [Fact]
    public void AddInvalidMemberThrowsInvalidOperationException()
    {
        // Arrange
        var myVar = "";
        // Assert
        var actualException = Assert.Throws<InvalidOperationException>(() =>
        {
            // Act
            _parameters.Add(p => myVar, "");
        });
        Assert.Equal(LambdaMustTargetTestcomponentPropertyMessage, actualException.Message);
    }

    [Fact]
    public void AddInvalidObjectMemberThrowsInvalidOperationException()
    {
        // Arrange
        // Assert
        var actualException = Assert.Throws<InvalidOperationException>(() =>
        {
            // Act
            _parameters.Add(p => null!, "");
        });
        Assert.Equal(CouldNotIdentifyMemberMessage, actualException.Message);
    }

    [Fact]
    public void AddMemberOfWrongTypeThrowsOnTypeMismatch()
    {
        // Arrange
        var expectedValue = "";
        var otherComponent = new TestOtherComponent();
        // Assert
        var actualException = Assert.Throws<InvalidOperationException>(() =>
        {
            // Act
            _parameters.Add(p => otherComponent.Text, expectedValue);
        });
        Assert.Equal(LambdaMustTargetTestcomponentPropertyMessage, actualException.Message);
    }
}
