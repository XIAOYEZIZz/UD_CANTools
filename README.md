# UD_CANTools

基于 Avalonia 的 C# 桌面项目，包含共享界面项目 `canserver3` 和桌面入口 `canserver3.Desktop`。

## macOS 编译与运行

安装 [.NET 10 SDK](https://learn.microsoft.com/dotnet/core/install/macos)（10.0.300 或更高的 10.0 SDK）。Apple Silicon 选择 ARM64，Intel Mac 选择 x64。无需安装 .NET MAUI 工作负载。

在仓库根目录执行：

```sh
dotnet restore UD_CANTools.sln
dotnet build UD_CANTools.sln -c Debug
dotnet build UD_CANTools.sln -c Release
dotnet run --project canserver3.Desktop/canserver3.Desktop.csproj
```

如果使用 `dotnet-install.sh` 安装到默认目录，先将 SDK 加入当前终端环境：

```sh
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$DOTNET_ROOT:$PATH"
```

### 发布独立运行目录

以下脚本自动选择当前 Mac 的架构，生成包含 .NET 运行时的发布目录：

```sh
sh canserver3.Desktop/build.sh
```

Apple Silicon 上可直接启动：

```sh
./canserver3.Desktop/bin/Release/net10.0/osx-arm64/publish/canserver3.0
```

Intel Mac 将上述路径中的 `osx-arm64` 替换为 `osx-x64`。也可以显式传入架构，例如 `sh canserver3.Desktop/build.sh osx-arm64`。这是命令行启动的桌面程序目录，不是签名的 `.app` 或 `.dmg` 安装包。

macOS 使用跨平台的 `net10.0` 目标；应用数据保存在当前用户的 `LocalApplicationData/UDANTech/canserver3` 目录，不需要管理员权限。Windows 构建保留 `net10.0-windows` 目标、兼容包及原有发布设置，Windows DLL 不参与 macOS 构建。

## 当前功能范围

仓库目前保留了桌面界面和部分模型；CAN 硬件收发、项目打开/保存及部分按钮尚无实际实现。编译与启动成功不代表真实 CAN 设备已受支持。本次 macOS 适配不添加硬件驱动或补齐这些功能。
