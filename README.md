# Philiprehberger.IpRange

[![CI](https://github.com/philiprehberger/dotnet-ip-range/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-ip-range/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.IpRange.svg)](https://www.nuget.org/packages/Philiprehberger.IpRange)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-ip-range)](LICENSE)

Parse and match IP addresses against CIDR ranges with IPv4 and IPv6 support.

## Installation

```bash
dotnet add package Philiprehberger.IpRange
```

## Usage

### Parsing and Matching CIDR Ranges

```csharp
using System.Net;
using Philiprehberger.IpRange;

// Parse a CIDR range and check membership
var range = IpRange.Parse("192.168.1.0/24");
bool match = range.Contains(IPAddress.Parse("192.168.1.42")); // true

// Inspect range boundaries
var cidr = IpCidrRange.Parse("10.0.0.0/8");
Console.WriteLine(cidr.FirstAddress); // 10.0.0.0
Console.WriteLine(cidr.LastAddress);  // 10.255.255.255
Console.WriteLine(cidr.PrefixLength); // 8
```

### Matching Against Multiple Ranges

```csharp
using System.Net;
using Philiprehberger.IpRange;

// Parse comma-separated CIDR ranges into a list
var list = IpRangeList.Parse("10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16");
bool allowed = list.Contains(IPAddress.Parse("172.16.5.1")); // true

// IPv6 ranges work the same way
var ipv6Range = IpCidrRange.Parse("fe80::/10");
bool isLinkLocal = ipv6Range.Contains(IPAddress.Parse("fe80::1")); // true
```

### Classifying IP Addresses

```csharp
using System.Net;
using Philiprehberger.IpRange;

// Get the full classification for an address
var classification = IpClassification.Classify(IPAddress.Parse("192.168.1.1"));
Console.WriteLine(classification); // Private

// Quick checks via IpRange helpers
bool isPrivate  = IpRange.IsPrivate(IPAddress.Parse("10.0.0.1"));    // true
bool isLoopback = IpRange.IsLoopback(IPAddress.Parse("127.0.0.1")); // true
```

## API

### `IpRange`

| Member | Description |
|--------|-------------|
| `Parse(string cidr)` | Parse a CIDR string and return an `IpCidrRange` |
| `IsPrivate(IPAddress address)` | Check if the address is in a private range |
| `IsLoopback(IPAddress address)` | Check if the address is a loopback address |

### `IpCidrRange`

| Member | Description |
|--------|-------------|
| `Network` | The network address |
| `PrefixLength` | The CIDR prefix length |
| `FirstAddress` | First address in the range |
| `LastAddress` | Last address in the range |
| `Contains(IPAddress)` | Check if an address falls within this range |
| `Contains(IpCidrRange)` | Check if another range is fully contained |
| `Parse(string cidr)` | Parse a CIDR notation string |

### `IpRangeList`

| Member | Description |
|--------|-------------|
| `Parse(string ranges)` | Parse comma-separated CIDR ranges |
| `Contains(IPAddress)` | Check if any range contains the address |

### `IpClassification`

| Member | Description |
|--------|-------------|
| `Classify(IPAddress)` | Returns the classification (Private, Loopback, LinkLocal, Public) |
| `IsPrivate(IPAddress)` | Check for private ranges (10.0/8, 172.16/12, 192.168/16, fc00::/7) |
| `IsLoopback(IPAddress)` | Check for loopback (127.0.0.1, ::1) |
| `IsLinkLocal(IPAddress)` | Check for link-local (169.254/16, fe80::/10) |

## Development

```bash
dotnet build src/Philiprehberger.IpRange.csproj --configuration Release
```

## License

MIT
