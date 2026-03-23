using System.Net;

namespace Philiprehberger.IpRange;

/// <summary>
/// Provides static helper methods for parsing CIDR ranges and classifying IP addresses.
/// </summary>
public static class IpRange
{
    /// <summary>
    /// Parses a CIDR notation string (e.g. "192.168.1.0/24") into an <see cref="IpCidrRange"/>.
    /// </summary>
    /// <param name="cidr">A CIDR notation string such as "10.0.0.0/8" or "fe80::/10".</param>
    /// <returns>An <see cref="IpCidrRange"/> representing the parsed range.</returns>
    /// <exception cref="FormatException">Thrown when the input is not valid CIDR notation.</exception>
    public static IpCidrRange Parse(string cidr)
    {
        return IpCidrRange.Parse(cidr);
    }

    /// <summary>
    /// Determines whether the specified IP address is in a private range.
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is private; otherwise, <c>false</c>.</returns>
    public static bool IsPrivate(IPAddress address)
    {
        return IpClassification.IsPrivate(address);
    }

    /// <summary>
    /// Determines whether the specified IP address is a loopback address.
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is a loopback address; otherwise, <c>false</c>.</returns>
    public static bool IsLoopback(IPAddress address)
    {
        return IpClassification.IsLoopback(address);
    }
}
