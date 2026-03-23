using System.Net;
using System.Net.Sockets;

namespace Philiprehberger.IpRange;

/// <summary>
/// Represents the classification of an IP address.
/// </summary>
public enum IpAddressClass
{
    /// <summary>A publicly routable address.</summary>
    Public,

    /// <summary>A private (RFC 1918 / RFC 4193) address.</summary>
    Private,

    /// <summary>A loopback address (127.0.0.1 or ::1).</summary>
    Loopback,

    /// <summary>A link-local address (169.254.0.0/16 or fe80::/10).</summary>
    LinkLocal
}

/// <summary>
/// Classifies IP addresses as private, loopback, link-local, or public.
/// </summary>
public static class IpClassification
{
    private static readonly IpCidrRange Ipv4Private10 = IpCidrRange.Parse("10.0.0.0/8");
    private static readonly IpCidrRange Ipv4Private172 = IpCidrRange.Parse("172.16.0.0/12");
    private static readonly IpCidrRange Ipv4Private192 = IpCidrRange.Parse("192.168.0.0/16");
    private static readonly IpCidrRange Ipv6UniqueLocal = IpCidrRange.Parse("fc00::/7");

    private static readonly IpCidrRange Ipv4Loopback = IpCidrRange.Parse("127.0.0.0/8");

    private static readonly IpCidrRange Ipv4LinkLocal = IpCidrRange.Parse("169.254.0.0/16");
    private static readonly IpCidrRange Ipv6LinkLocal = IpCidrRange.Parse("fe80::/10");

    /// <summary>
    /// Classifies the specified IP address.
    /// </summary>
    /// <param name="address">The IP address to classify.</param>
    /// <returns>The <see cref="IpAddressClass"/> of the address.</returns>
    public static IpAddressClass Classify(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (IsLoopback(address))
        {
            return IpAddressClass.Loopback;
        }

        if (IsLinkLocal(address))
        {
            return IpAddressClass.LinkLocal;
        }

        if (IsPrivate(address))
        {
            return IpAddressClass.Private;
        }

        return IpAddressClass.Public;
    }

    /// <summary>
    /// Determines whether the specified IP address is in a private range.
    /// Private ranges: 10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16 (IPv4), fc00::/7 (IPv6).
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is private; otherwise, <c>false</c>.</returns>
    public static bool IsPrivate(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return Ipv4Private10.Contains(address) ||
                   Ipv4Private172.Contains(address) ||
                   Ipv4Private192.Contains(address);
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return Ipv6UniqueLocal.Contains(address);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified IP address is a loopback address.
    /// Loopback addresses: 127.0.0.0/8 (IPv4), ::1 (IPv6).
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is a loopback address; otherwise, <c>false</c>.</returns>
    public static bool IsLoopback(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return Ipv4Loopback.Contains(address);
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return address.Equals(IPAddress.IPv6Loopback);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified IP address is a link-local address.
    /// Link-local addresses: 169.254.0.0/16 (IPv4), fe80::/10 (IPv6).
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is link-local; otherwise, <c>false</c>.</returns>
    public static bool IsLinkLocal(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            return Ipv4LinkLocal.Contains(address);
        }

        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return Ipv6LinkLocal.Contains(address);
        }

        return false;
    }
}
