# Changelog

## 0.1.1 (2026-03-24)

- Expand README usage section with feature subsections

## 0.1.0 (2026-03-22)

- Initial release
- `IpRange.Parse` for parsing CIDR notation strings
- `IpRange.IsPrivate` and `IpRange.IsLoopback` classification helpers
- `IpCidrRange` readonly struct with `Contains`, `FirstAddress`, `LastAddress`
- `IpRangeList` for matching against comma-separated CIDR lists
- `IpClassification` with private, loopback, and link-local detection
- Full IPv4 and IPv6 support
