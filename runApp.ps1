$jsonConfig = Get-Content -Raw -Path "sharedAppSettings.json" | ConvertFrom-Json
$providers = $jsonConfig.Providers

# Stop any running instances of ElectricityServiceApi
$existingProcess = Get-Process | Where-Object { $_.ProcessName -like "ElectricityServiceApi" }
if ($existingProcess) {
    Write-Host "Stopping existing ElectricityServiceApi process..."
    $existingProcess | Stop-Process -Force
}
# Start Electricity Provider Microservices
foreach ($provider in $providers) {
    Write-Host "Starting Electricity Provider Service: $($provider.ProviderName) on port $($provider.Port)..."
    Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project `".\Backend\ElectricityServiceApi\ElectricityServiceApi.csproj`" --urls=http://localhost:$($provider.Port)"
    Start-Sleep -Seconds 20
}

$useRedis = "false"
# Check if Docker is installed
if (-Not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "Docker is not installed. Please install Docker."    
}
else {
    # Ensure Docker is running
    $dockerStatus = docker info --format "{{.ServerErrors}}" 2>$null
    if ($dockerStatus) {
        Write-Host "Starting Docker..."
        Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"
        Start-Sleep -Seconds 10
    }
    
    $containerName = "redis_cache"
    # Check if the container exists
    $container = docker ps -a --format "{{.Names}}" | Where-Object { $_ -eq $containerName }

    if ($container) {
        # Check if the container is stopped
        $status = docker inspect -f "{{.State.Running}}" $containerName

        if ($status -eq "false") {
            Write-Host "Redis container exists but is stopped. Starting it..."
            docker start $containerName
        } else {
            Write-Host "Redis container is already running."
        }
    } else {
        Write-Host "Redis container does not exist. Creating and starting it..."
        docker run --name $containerName -d -p 6379:6379 redis:latest
    }

    # Determine Redis usage
    $redisAvailable = docker ps --format "{{.Names}}" | Select-String -Pattern "redis_cache"

    $useRedis = if ($redisAvailable) { "true" } else { "false" }
    Write-Host "Use Redis: $useRedis"
}

# Stop any running instances of ElectricityServiceApi
$existingProcess = Get-Process | Where-Object { $_.ProcessName -like "CalculationService" }
if ($existingProcess) {
    Write-Host "Stopping existing CalculationService process..."
    $existingProcess | Stop-Process -Force
}

# Start Calculation Service with Redis parameter
$env:USE_REDIS = $useRedis
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project `".\Backend\CalculationService\CalculationService.csproj`" --urls=http://localhost:5050"
Start-Sleep -Seconds 2

#Killing Frontend app
npx kill-port 4200

# Start Angular Frontend
Start-Process -NoNewWindow -FilePath "cmd.exe" -ArgumentList "/c cd `".\Frontend\Verivox`" && ng serve --port 4200"

Write-Host "All services are running!"