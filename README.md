# Warble.NET
Warble.NET provides a C# API around the [Warble C library](https://github.com/mbientlab/Warble).

# Install
Add the Warble.NetStandard package to your project with the Package Manager Console:

```bat
PM> Install-Package Warble.Net
```

Or add a **PackageReference** tag to your *.csproj file:
```xml
<PackageReference Include="Warble.Net" Version="[1.0.4, 2.0)" />
```

Linux developers will also need to build Warble on their machine and place the .so files in the appropriate folders.  

# Usage
See the example projects for sample code on connecting, reading the device information characteristics, and performing BLE scans.
