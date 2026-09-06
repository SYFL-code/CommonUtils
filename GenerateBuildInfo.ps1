param(
    [string]$VersionString,
    [string]$BuildTime
)

# 使用环境变量 Configuration（MSBuild 会自动设置）
$config = if ($env:Configuration) { $env:Configuration } else { "Debug" }

# 构建输出目录和文件路径
$outDir = Join-Path $PSScriptRoot "src\obj\$config\net48"
$outFile = Join-Path $outDir "BuildInfo.g.cs"

# 确保目录存在
if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

# 生成内容（注意分号固定）
$content = @"
// auto-generated
public static class BuildInfo
{
    public const string Version = "$VersionString";
    public const string BuildTime = "$BuildTime";
}
"@

# 写入文件（UTF8 without BOM）
Set-Content -Path $outFile -Value $content -Encoding UTF8 -NoNewline