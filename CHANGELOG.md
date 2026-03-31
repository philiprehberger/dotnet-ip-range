# Changelog

## 0.1.2 (2026-03-31)

- Standardize README to 3-badge format with emoji Support section
- Update CI actions to v5 for Node.js 24 compatibility
- Add GitHub issue templates, dependabot config, and PR template

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
