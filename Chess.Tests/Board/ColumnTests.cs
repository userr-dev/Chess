using Chess.Core.Board;

namespace Chess.Tests.Board;

public class ColumnTests
{
    [Theory]
    [InlineData(Column.A, Column.C, 2)]
    [InlineData(Column.H, Column.F, 2)]
    [InlineData(Column.A, Column.H, 7)]
    [InlineData(Column.F, Column.B, 4)]
    [InlineData(Column.A, Column.B, 1)]
    [InlineData(Column.D, Column.E, 1)]
    public void DistanceTo_ReturnExpectedValue(Column from, Column to, int expectedDistance)
    {
        var distance = from.DistanceTo(to);
        Assert.Equal(expectedDistance, distance);
    }

    [Theory]
    [InlineData(Column.A, Column.A)]
    [InlineData(Column.C, Column.C)]
    [InlineData(Column.H, Column.H)]
    [InlineData(Column.E, Column.E)]
    [InlineData(Column.B, Column.B)]
    public void DistanceTo_SameColumn_Return0(Column from, Column to)
    {
        var distance = from.DistanceTo(to);
        Assert.Equal(0, distance);
    }
    
    [Theory]
    [InlineData(Column.A, Column.C)]
    [InlineData(Column.B, Column.F)]
    [InlineData(Column.A, Column.H)]
    public void DistanceTo_IsSymmetric(Column from, Column to)
    {
        Assert.Equal(from.DistanceTo(to), to.DistanceTo(from));
    }
    
    [Theory]
    [InlineData(Column.A, 1, Column.B)]
    [InlineData(Column.A, 7, Column.H)]
    [InlineData(Column.H, -1, Column.G)]
    [InlineData(Column.H, -7, Column.A)]
    [InlineData(Column.D, 2, Column.F)]
    [InlineData(Column.D, -2, Column.B)]
    public void Shift_ValidOffset_ReturnsExpectedColumn(Column column, int offset, Column expected)
    {
        var result = column.Shift(offset);
        Assert.Equal(expected, result);
    }
 
    [Fact]
    public void Shift_ZeroOffset_ReturnsSameColumn()
    {
        var result = Column.D.Shift(0);
        Assert.Equal(Column.D, result);
    }
 
    [Theory]
    [InlineData(Column.A, -1)]
    [InlineData(Column.A, -8)]
    [InlineData(Column.H, 1)]
    [InlineData(Column.H, 8)]
    [InlineData(Column.D, 5)]
    [InlineData(Column.D, -4)]
    public void Shift_OutOfBoundsOffset_ThrowsArgumentOutOfRangeException(Column column, int offset)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => column.Shift(offset));
    }

    [Theory]
    [InlineData(Column.A, 1, Column.B)]
    [InlineData(Column.A, 7, Column.H)]
    [InlineData(Column.H, -1, Column.G)]
    [InlineData(Column.H, -7, Column.A)]
    [InlineData(Column.D, 0, Column.D)]
    [InlineData(Column.D, -3, Column.A)]
    public void TryShift_ValidOffset_ReturnsTrueAndCorrectColumn(Column column, int offset, Column expected)
    {
        var success = Column.TryShift(column, offset, out var result);
        Assert.True(success);
        Assert.Equal(expected, result);
    }
 
    [Theory]
    [InlineData(Column.A, -1)]
    [InlineData(Column.A, -8)]
    [InlineData(Column.H, 1)]
    [InlineData(Column.H, 8)]
    [InlineData(Column.D, 5)]
    [InlineData(Column.D, -4)]
    public void TryShift_OutOfBoundsOffset_ReturnsFalse(Column column, int offset)
    {
        var success = Column.TryShift(column, offset, out _);
        Assert.False(success);
    }
 
    [Theory]
    [InlineData(Column.A, -1)]
    [InlineData(Column.H, 1)]
    [InlineData(Column.D, 5)]
    public void TryShift_OutOfBoundsOffset_ResultIsUnchanged(Column column, int offset)
    {
        Column.TryShift(column, offset, out var result);
        Assert.Equal(column, result);
    }

}