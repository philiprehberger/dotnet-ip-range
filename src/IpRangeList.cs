using System.Net;

namespace Philiprehberger.IpRange;

/// <summary>
/// Represents a collection of CIDR ranges that can be checked against a single IP address.
/// </summary>
public sealed class IpRangeList
{
    private readonly IpCidrRange[] _ranges;

    /// <summary>
    /// Initializes a new instance of <see cref="IpRangeList"/> with the specified ranges.
    /// </summary>
    /// <param name="ranges">The CIDR ranges to include.</param>
    public IpRangeList(IEnumerable<IpCidrRange> ranges)
    {
        ArgumentNullException.ThrowIfNull(ranges);
        _ranges = ranges.ToArray();
    }

    /// <summary>
    /// Gets the number of CIDR ranges in this list.
    /// </summary>
    public int Count => _ranges.Length;

    /// <summary>
    /// Determines whether any range in this list contains the specified IP address.
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if any range contains the address; otherwise, <c>false</c>.</returns>
    public bool Contains(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        foreach (var range in _ranges)
        {
            if (range.Contains(address))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Parses a comma-separated list of CIDR ranges.
    /// </summary>
    /// <param name="ranges">A comma-separated string of CIDR ranges (e.g. "10.0.0.0/8, 172.16.0.0/12").</param>
    /// <returns>An <see cref="IpRangeList"/> containing all parsed ranges.</returns>
    /// <exception cref="FormatException">Thrown when any range is not valid CIDR notation.</exception>
    public static IpRangeList Parse(string ranges)
    {
        ArgumentNullException.ThrowIfNull(ranges);

        var parts = ranges.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new IpCidrRange[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            parsed[i] = IpCidrRange.Parse(parts[i]);
        }

        return new IpRangeList(parsed);
    }

    /// <inheritdoc />
    public override string ToString() => string.Join(", ", _ranges.Select(r => r.ToString()));
}
