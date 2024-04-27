# FEZStripGen

A simple .NET program that generates stripped FEZ binaries.

An input should be a directory containing binaries of DRM-free version of FEZ v1.12. As an output, it produces a copy of these binaries, with a couple of differences:

- Body of every method is stripped, making binaries header-only.
- Every reference of `MockAchievement` and `MockUser` has been removed to allow generating hooks which work on all platforms.
