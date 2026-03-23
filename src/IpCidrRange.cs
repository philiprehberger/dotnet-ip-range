using System.Net;
using System.Net.Sockets;

namespace Philiprehberger.IpRange;

/// <summary>
/// Represents a CIDR IP address range with a network address and prefix length.
/// </summary>
public readonly struct IpCidrRange : IEquatable<IpCidrRange>
{
    /// <summary>
    /// Gets the network address of this CIDR range.
    /// </summary>
    public IPAddress Network { get; }

    /// <summary>
    /// Gets the prefix length (number of significant bits) of this CIDR range.
    /// </summary>
    public int PrefixLength { get; }

    private readonly byte[] _networkBytes;
    private readonly byte[] _maskBytes;

    /// <summary>
    /// Initializes a new instance of <see cref="IpCidrRange"/> with the specified network address and prefix length.
    /// </summary>
    /// <param name="network">The network address.</param>
    /// <param name="prefixLength">The prefix length in bits.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the prefix length is out of valid range.</exception>
    public IpCidrRange(IPAddress network, int prefixLength)
    {
        int maxPrefix = network.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;

        if (prefixLength < 0 || prefixLength > maxPrefix)
        {
            throw new ArgumentOutOfRangeException(nameof(prefixLength),
                $"Prefix length must be between 0 and {maxPrefix} for {network.AddressFamily}.");
        }

        _maskBytes = CreateMask(maxPrefix, prefixLength);
        _networkBytes = ApplyMask(network.GetAddressBytes(), _maskBytes);
        Network = new IPAddress(_networkBytes);
        PrefixLength = prefixLength;
    }

    /// <summary>
    /// Gets the first (lowest) address in this CIDR range.
    /// </summary>
    public IPAddress FirstAddress => Network;

    /// <summary>
    /// Gets the last (highest) address in this CIDR range.
    /// </summary>
    public IPAddress LastAddress
    {
        get
        {
            var lastBytes = new byte[_networkBytes.Length];
            for (int i = 0; i < _networkBytes.Length; i++)
            {
                lastBytes[i] = (byte)(_networkBytes[i] | ~_maskBytes[i]);
            }
            return new IPAddress(lastBytes);
        }
    }

    /// <summary>
    /// Determines whether the specified IP address is contained within this CIDR range.
    /// </summary>
    /// <param name="address">The IP address to check.</param>
    /// <returns><c>true</c> if the address is within this range; otherwise, <c>false</c>.</returns>
    public bool Contains(IPAddress address)
    {
        if (address.AddressFamily != Network.AddressFamily)
        {
            return false;
        }

        var addressBytes = address.GetAddressBytes();
        var maskedBytes = ApplyMask(addressBytes, _maskBytes);

        return maskedBytes.AsSpan().SequenceEqual(_networkBytes);
    }

    /// <summary>
    /// Determines whether the specified CIDR range is fully contained within this CIDR range.
    /// </summary>
    /// <param name="other">The CIDR range to check.</param>
    /// <returns><c>true</c> if the other range is fully contained; otherwise, <c>false</c>.</returns>
    public bool Contains(IpCidrRange other)
    {
        if (other.Network.AddressFamily != Network.AddressFamily)
        {
            return false;
        }

        // The other range must have an equal or longer prefix (smaller or equal subnet)
        // and its network address must fall within this range.
        return other.PrefixLength >= PrefixLength && Contains(other.Network);
    }

    /// <summary>
    /// Parses a CIDR notation string into an <see cref="IpCidrRange"/>.
    /// </summary>
    /// <param name="cidr">A string in CIDR notation (e.g. "192.168.0.0/16").</param>
    /// <returns>The parsed <see cref="IpCidrRange"/>.</returns>
    /// <exception cref="FormatException">Thrown when the input is not valid CIDR notation.</exception>
    public static IpCidrRange Parse(string cidr)
    {
        ArgumentNullException.ThrowIfNull(cidr);

        var parts = cidr.Trim().Split('/');
        if (parts.Length != 2)
        {
            throw new FormatException($"Invalid CIDR notation: '{cidr}'. Expected format: address/prefix.");
        }

        if (!IPAddress.TryParse(parts[0], out var address))
        {
            throw new FormatException($"Invalid IP address: '{parts[0]}'.");
        }

        if (!int.TryParse(parts[1], out var prefixLength))
        {
            throw new FormatException($"Invalid prefix length: '{parts[1]}'.");
        }

        return new IpCidrRange(address, prefixLength);
    }

    /// <inheritdoc />
    public bool Equals(IpCidrRange other)
    {
        return PrefixLength == other.PrefixLength &&
               _networkBytes.AsSpan().SequenceEqual(other._networkBytes);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is IpCidrRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(PrefixLength);
        foreach (var b in _networkBytes)
        {
            hash.Add(b);
        }
        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() => $"{Network}/{PrefixLength}";

    /// <summary>
    /// Determines whether two <see cref="IpCidrRange"/> instances are equal.
    /// </summary>
    public static bool operator ==(IpCidrRange left, IpCidrRange right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="IpCidrRange"/> instances are not equal.
    /// </summary>
    public static bool operator !=(IpCidrRange left, IpCidrRange right) => !left.Equals(right);

    private static byte[] CreateMask(int totalBits, int prefixLength)
    {
        int byteCount = totalBits / 8;
        var mask = new byte[byteCount];

        for (int i = 0; i < byteCount; i++)
        {
            int bitsInThisByte = Math.Min(8, Math.Max(0, prefixLength - (i * 8)));
            mask[i] = (byte)(0xFF << (8 - bitsInThisByte));
        }

        return mask;
    }

    private static byte[] ApplyMask(byte[] address, byte[] mask)
    {
        var result = new byte[address.Length];
        for (int i = 0; i < address.Length; i++)
        {
            result[i] = (byte)(address[i] & mask[i]);
        }
        return result;
    }
}
