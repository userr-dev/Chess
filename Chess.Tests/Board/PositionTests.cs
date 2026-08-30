namespace Chess.Tests.Board;

public class PositionTests
{
    // --- Create ---

    [Theory]
    [InlineData(Column.A, 0)]
    [InlineData(Column.H, 7)]
    [InlineData(Column.D, 3)]
    public void Create_ValidArguments_ReturnsPosition(Column column, int row)
    {
        var pos = Position.Create(column, row);
        Assert.Equal(column, pos.Column);
        Assert.Equal(row, pos.Row);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)]
    public void Create_RowOutOfRange_ThrowsArgumentOutOfRangeException(int row)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Position.Create(Column.A, row));
    }

    // --- Parse ---

    [Theory]
    [InlineData("A1", Column.A, 0)]
    [InlineData("h8", Column.H, 7)]
    [InlineData("D4", Column.D, 3)]
    public void Parse_ValidString_ReturnsPosition(string s, Column expectedColumn, int expectedRow)
    {
        var pos = Position.Parse(s);
        Assert.Equal(expectedColumn, pos.Column);
        Assert.Equal(expectedRow, pos.Row);
    }

    [Theory]
    [InlineData("Z1")]
    [InlineData("A9")]
    [InlineData("A0")]
    [InlineData("AA")]
    [InlineData("1A")]
    public void Parse_InvalidFormat_ThrowsFormatException(string s)
    {
        Assert.Throws<FormatException>(() => Position.Parse(s));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_NullOrWhitespace_ThrowsArgumentException(string? s)
    {
        Assert.Throws<ArgumentException>(() => Position.Parse(s!));
    }

    // --- ToString ---

    [Theory]
    [InlineData(Column.A, 0, "a1")]
    [InlineData(Column.H, 7, "h8")]
    [InlineData(Column.E, 3, "e4")]
    public void ToString_ReturnsHumanReadableFormat(Column column, int row, string expected)
    {
        var pos = Position.Create(column, row);
        Assert.Equal(expected, pos.ToString());
    }

    // --- IsPromotionRow ---

    [Theory]
    [InlineData(Column.A, 7, Color.Light, true)]
    [InlineData(Column.A, 0, Color.Dark, true)]
    [InlineData(Column.A, 0, Color.Light, false)]
    [InlineData(Column.A, 7, Color.Dark, false)]
    [InlineData(Column.A, 4, Color.Light, false)]
    public void IsPromotionRow_ReturnsExpected(Column column, int row, Color color, bool expected)
    {
        var pos = Position.Create(column, row);
        Assert.Equal(expected, pos.IsPromotionRow(color));
    }

    // --- IsInDirection ---

    [Fact]
    public void IsInDirection_TargetEqualsFrom_ReturnsTrue()
    {
        var from = D4;
        var to = D6;
        Assert.True(Position.IsInDirection(from, to, from));
    }

    [Fact]
    public void IsInDirection_TargetEqualsTo_ReturnsTrue()
    {
        var from = D4;
        var to = D6;
        Assert.True(Position.IsInDirection(from, to, to));
    }

    [Fact]
    public void IsInDirection_TargetBetweenFromAndTo_ReturnsTrue()
    {
        var from = D4;
        var to = D6;
        var target = D5;
        Assert.True(Position.IsInDirection(from, to, target));
    }

    [Fact]
    public void IsInDirection_TargetBeyondTo_ReturnsFalse()
    {
        var from = D4;
        var to = D6;
        var target = D7;
        Assert.False(Position.IsInDirection(from, to, target));
    }

    [Fact]
    public void IsInDirection_TargetNotCollinear_ReturnsFalse()
    {
        var from = D4;
        var to = D6;
        var target = E5;
        Assert.False(Position.IsInDirection(from, to, target));
    }

    // --- TryMove / directional helpers ---
    [Fact]
    public void TryMove_LargeOffset_ReturnsFalse()
    {
        var pos = A1;
        var result = Position.TryMove(ref pos, 0, -1);
        Assert.False(result);
    }
}