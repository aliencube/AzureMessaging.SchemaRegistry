#
# This converts all YAML templates into JSON templates.
#

Param(
    [string] [Parameter(Mandatory=$true)] $RootDirectory
)

$directory = $RootDirectory
$yarmpath = "$RootDirectory\bin\yarm-cli\yarm.cmd"

Get-ChildItem -Path $directory\samples -Recurse -Include *.yaml | ForEach-Object {
    & $yarmpath -i $_.FullName -o $($_.FullName).Replace(".yaml", ".json")
}
