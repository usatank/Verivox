# Stop any running instances of ElectricityServiceApi
$existingProcess = Get-Process | Where-Object { $_.ProcessName -like "ElectricityServiceApi" }
if ($existingProcess) {
    Write-Host "Stopping existing ElectricityServiceApi process..."
    $existingProcess | Stop-Process -Force
}

# Stop any running instances of ElectricityServiceApi
$existingProcess = Get-Process | Where-Object { $_.ProcessName -like "CalculationService" }
if ($existingProcess) {
    Write-Host "Stopping existing CalculationService process..."
    $existingProcess | Stop-Process -Force
}

#Killing Frontend app
npx kill-port 4200