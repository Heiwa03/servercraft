# Create directories if they don't exist
$teamDir = "team"
$partnersDir = "partners"

if (-not (Test-Path $teamDir)) {
    New-Item -ItemType Directory -Path $teamDir
}
if (-not (Test-Path $partnersDir)) {
    New-Item -ItemType Directory -Path $partnersDir
}

# Download team member photos
$teamMembers = @(
    @{Name="John Smith"; File="ceo.jpg"},
    @{Name="Sarah Johnson"; File="cto.jpg"},
    @{Name="Michael Brown"; File="support.jpg"}
)

foreach ($member in $teamMembers) {
    $url = "https://ui-avatars.com/api/?name=$($member.Name -replace ' ','+')&background=0D8ABC&color=fff&size=200"
    $output = Join-Path $teamDir $member.File
    Invoke-WebRequest -Uri $url -OutFile $output
    Write-Host "Downloaded $($member.File)"
}

# Create simple partner logos using PowerShell
$partners = @(
    @{Name="Dell"; File="dell.png"},
    @{Name="HP"; File="hp.png"},
    @{Name="Lenovo"; File="lenovo.png"},
    @{Name="Cisco"; File="cisco.png"},
    @{Name="IBM"; File="ibm.png"}
)

foreach ($partner in $partners) {
    $output = Join-Path $partnersDir $partner.File
    $svg = @"
<svg width="150" height="50" xmlns="http://www.w3.org/2000/svg">
    <rect width="150" height="50" fill="#0D8ABC"/>
    <text x="75" y="30" font-family="Arial" font-size="20" fill="white" text-anchor="middle">$($partner.Name)</text>
</svg>
"@
    $svg | Out-File -FilePath $output -Encoding utf8
    Write-Host "Created $($partner.File)"
}

Write-Host "All images created successfully!" 