#!/bin/sh

set -e

generate_version() {
    cd ..
    cd UD_Protocol
    sh GenerateVersion.sh
    cd ..
    cd canserver3
	gitver=`git rev-parse HEAD`
	cat >Version.cs << EOF
using System;
namespace canserver3;
public sealed class Version
{
    private static readonly Lazy<Version> _instance =
            new Lazy<Version>(() => new Version());
    public static Version Instance => _instance.Value;
    public static string App_Version { get; } = "3.0.5";
    public static string Git_Version { get; } = "${gitver}";
}
EOF
}

[ -f Version.cs ] || generate_version

if [ "$1" = "ver" ] ; then
generate_version
elif [ "$1" = "x64" ] ; then
cd ..
cd canserver3.Desktop
rm -rf ./bin/Release/net10.0-windows/win-x64/publish
generate_version
cd ..
cd canserver3.Desktop
dotnet publish -c Release -r win-x64 --self-contained true
cd .\\bin\\Release\\net10.0-windows\\win-x64\\publish
rm -f *.pdb
explorer .
else
cd ..
cd canserver3.Desktop
rm -rf ./bin/Release/net10.0-windows/win-x86/publish
generate_version
cd ..
cd canserver3.Desktop
dotnet publish -c Release -r win-x86 --self-contained true
cd .\\bin\\Release\\net10.0-windows\\win-x86\\publish
rm -f *.pdb
explorer .
fi