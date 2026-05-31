# Changelog

## 0.3.0 (2026-05-30)

- Add `IpCidrRange.TryParse(string?, out IpCidrRange)` for safe parsing without exceptions
- Add `IpCidrRange.AddressCount` property returning the total addresses in the range as `BigInteger`
- Add `IpRangeList.TryParse(string?, out IpRangeList?)` for safe parsing without exceptions
- Add card image to README

## 0.2.0 (2026-04-05)

- Add `IpCidrRange.Overlaps(IpCidrRange other)` method to check if two CIDR ranges share any common addresses

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
