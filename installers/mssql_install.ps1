#ps1_sysnative
<#
.SYNOPSIS
    MSSQL Service installation script
.DESCRIPTION
    This script installs the Uhuru MSSQL service and all its dependencies
.PARAMETER messageBus
    ALS Nats endpoint.
.PARAMETER index
    MSSQL Service index in the deployment. Default 0.
    Note that each Windows MSSQL Node must have a unique index.
.PARAMETER localRoute
    Used to determine the network interface.
    The application takes the interface used when trying to reach the provided IP.
.PARAMETER statusPort
    Port for publishing an http endpoint used for monitoring. Default is 0.    
.PARAMETER plan
	Type of plan associated with the Windows MSSQL Node. Default is free. 
.PARAMETER capacity
	Default is 200.
.PARAMETER BaseDir
	Default is .\ 
.PARAMETER NodeId
	Identification name of the Windows MSSQL Node. Default is mssql_node_free_1
.PARAMETER ZInterval
	Default is 30000.
.PARAMETER LocalDb
	Default is localDb.xml
.PARAMETER MaxNatsPayLoad
	Default is 1048576
.PARAMETER FqdnHosts
	Uses fully qualified domain name for hosts. Default is false.
.PARAMETER OPTimeLimit
	Default is 6.
.PARAMETER Host
    Specifies the MSSQL Server host. Default is "(LOCAL)".
.PARAMETER User
	MSSQL Server Administrative User. Default is "sa". 
.PARAMETER Password.
	MSSQL Server password for the specified user. Default is "password1234!".
.PARAMETER Port
	MSSQL Server Port. Default value is "1433"
.PARAMETER MaxDbSize
	MSSQL Server maximum allowed database size in GB. Default is "20" 
.PARAMETER MaxLongQuery
	Default is 3.
.PARAMETER MaxLongTx
	Default is 30.
.PARAMETER MaxUserConns 
	MSSQL Server maximum allowed simultaneous client connections. Default is 20.
.PARAMETER InitialDataSize
	Default is "4096KB".
.PARAMETER DataFileGrowth
	Default is "5120KB".
.PARAMETER LogFileGrowth
	Default is "1024KB".
.PARAMETER MaxDataSize
	Allows fixed maximum data size. Default is "UNLIMITED".
.PARAMETER InitialLogSize
	Default is "2048KB".
.PARAMETER MaxLogSize
	Allows fixed maximum log size. Default is "UNLIMITED".
.PARAMETER LogicalStorageUnits
	Allows setting specific storage unit for data. Default is "C:"
.PARAMETER DefaultVersion
	Allows setting default MSSQL Server version. Default value is "2008R2"
.PARAMETER SupportedName
	Allows setting specific supported MSSQL Server versions. Default value is "2008R2"
.PARAMETER BackupBaseDir
	Allows setting remote directory for data backup. Default is "\\192.168.1.105\migration\backup"
.PARAMETER TimeOut
	Specifies timeout options for backup. Default value is 120. 
.PARAMETER ServiceName
	Default is "MSSQL" 
.PARAMETER MSSQLServiceDownloadURL
        URL of the MSSQL Service msi.
.PARAMETER installDir
 	Target install directory. This is where all the Windows MSSQL Service binaries will be installed. Default is C:\WinMSSQL
#>

[CmdletBinding()]
param (
	$messageBus = '',   
	$index = 0,
	$localRoute = '',
	$statusPort =  0,
	$plan = 'free',
   	$BaseDir ='.\',
	$capacity=200,
	$NodeId='mssql_node_free_0',
	$ZInterval=30000,
	$LocalDb='localDb.xml',
	$MaxNatsPayLoad='1048576',
	$FqdnHosts='false',
	$OPTimeLimit=6,
	$HostName='(LOCAL)',
	$User='',
	$Password='',
	$Port=1433,
	$MaxDbSize=20,
	$MaxLongQuery=3,
	$MaxLongTx=30,
	$MaxUserConns=20,
	$InitialDataSize='4096KB',
	$DataFileGrowth='5120KB',
	$LogFileGrowth='1024KB',
	$MaxDataSize='UNLIMITED',
	$InitialLogSize='2048KB',
	$MaxLogSize='UNLIMITED',
	$LogicalStorageUnits='C:',
	$DefaultVersion='2012',
	$SupportedName='2012',
	$BackupBaseDir='\\192.168.1.105\migration\backup',
	$TimeOut=120,
	$ServiceName='MSSQL',
    $mssqlDownloadURL = "http://15.125.102.70/installers/mssqlinstaller-1.2.55.msi",
	$installDir = 'C:\WinMSSQL'
)

$location = $pwd.Path
$tempDir = [System.Guid]::NewGuid().ToString()


function CheckParam($paramName, [REF]$paramValue, $mandatory, $templateValue)
{
    if (([string]::IsNullOrWhiteSpace($templateValue) -eq $false) -and ($templateValue -notlike "*{.${paramName}}*"))
    {
        $paramValue.Value = $templateValue
    }

    if ([string]::IsNullOrWhiteSpace($paramValue.Value) -and $mandatory)
    {
        Write-Error "The ${paramName} parameter is mandatory."
        exit 1
    }
    $paramActualValue = $paramValue.Value
    Write-Host "Using <${paramName}> = '${paramActualValue}'"
}

function CheckMSSQLService(){
    if (Get-Service "MSSQLSERVER" -ErrorAction SilentlyContinue){
        Write-Host "MSSQL Server found, continuing instalation"
    } else {
        Write-Host "MSSQL Server not installed on this machine! Please install MSSQL Server before installing the MSSQL Node Service."
        Exit
    }
}

function VerifyParameters
{
    # Mandatory parameters
    CheckParam 'messageBus'             ([ref]$messageBus)            $true  '{{if .messageBus}}{{.messageBus}}{{else}}{{end}}'
    CheckParam 'localRoute'             ([ref]$localRoute)            $true  '{{if .localRoute}}{{.localRoute}}{{else}}{{end}}'    
    CheckParam 'User'                   ([ref]$User)                  $true  '{{if .User}}{{.User}}{{else}}{{end}}'
    CheckParam 'Password'               ([ref]$Password)              $true  '{{if .Password}}{{.Password}}{{else}}{{end}}'
	
	#Optional parameters
	CheckParam 'index'                  ([ref]$index)                 $false '{{if .index}}{{.index}}{{else}}{{end}}'
	CheckParam 'DefaultVersion'         ([ref]$DefaultVersion)        $false '{{if .version}}{{.version}}{{else}}{{end}}'
	CheckParam 'SupportedName'          ([ref]$SupportedName)         $false '{{if .version}}{{.version}}{{else}}{{end}}'	
	CheckParam 'statusPort'             ([ref]$statusPort)            $false '{{if .statusPort}}{{.statusPort}}{{else}}{{end}}'	
	CheckParam 'plan'                   ([ref]$plan)                  $false '{{if .plan}}{{.plan}}{{else}}{{end}}'
	CheckParam 'BaseDir'                ([ref]$BaseDir)               $false '{{if .BaseDir}}{{.BaseDir}}{{else}}{{end}}'
	CheckParam 'capacity'               ([ref]$capacity)              $false '{{if .capacity}}{{.capacity}}{{else}}{{end}}'
	CheckParam 'NodeId'                 ([ref]$NodeId)                $false '{{if .NodeId}}{{.NodeId}}{{else}}{{end}}'	   	
    CheckParam 'ZInterval'              ([ref]$ZInterval)             $false '{{if .ZInterval}}{{.ZInterval}}{{else}}{{end}}'		
    CheckParam 'LocalDb'                ([ref]$LocalDb)               $false '{{if .LocalDb}}{{.LocalDb}}{{else}}{{end}}'		
	CheckParam 'MaxNatsPayLoad'         ([ref]$MaxNatsPayLoad)        $false '{{if .MaxNatsPayLoad}}{{.MaxNatsPayLoad}}{{else}}{{end}}'		
	CheckParam 'FqdnHosts'              ([ref]$FqdnHosts)             $false '{{if .FqdnHosts}}{{.FqdnHosts}}{{else}}{{end}}'		
	CheckParam 'OPTimeLimit'            ([ref]$OPTimeLimit)           $false '{{if .OPTimeLimit}}{{.OPTimeLimit}}{{else}}{{end}}'		
	CheckParam 'HostName'               ([ref]$HostName)              $false '{{if .HostName}}{{.HostName}}{{else}}{{end}}'		
	CheckParam 'Port'                   ([ref]$Port)                  $false '{{if .Port}}{{.Port}}{{else}}{{end}}'		
	CheckParam 'MaxDbSize'              ([ref]$MaxDbSize)             $false '{{if .MaxDbSize}}{{.MaxDbSize}}{{else}}{{end}}'		
	CheckParam 'MaxLongQuery'           ([ref]$MaxLongQuery)          $false '{{if .MaxLongQuery}}{{.MaxLongQuery}}{{else}}{{end}}'		
	CheckParam 'MaxLongTx'              ([ref]$MaxLongTx)             $false '{{if .MaxLongTx}}{{.MaxLongTx}}{{else}}{{end}}'		
	CheckParam 'MaxUserConns'           ([ref]$MaxUserConns)          $false '{{if .MaxUserConns}}{{.MaxUserConns}}{{else}}{{end}}'		
	CheckParam 'InitialDataSize'        ([ref]$InitialDataSize)       $false '{{if .InitialDataSize}}{{.InitialDataSize}}{{else}}{{end}}'		
	CheckParam 'DataFileGrowth'         ([ref]$DataFileGrowth)        $false '{{if .DataFileGrowth}}{{.DataFileGrowth}}{{else}}{{end}}'		
	CheckParam 'LogFileGrowth'          ([ref]$LogFileGrowth)         $false '{{if .LogFileGrowth}}{{.LogFileGrowth}}{{else}}{{end}}'		
	CheckParam 'MaxDataSize'            ([ref]$MaxDataSize)           $false '{{if .MaxDataSize}}{{.MaxDataSize}}{{else}}{{end}}'		
	CheckParam 'InitialLogSize'         ([ref]$InitialLogSize)        $false '{{if .InitialLogSize}}{{.InitialLogSize}}{{else}}{{end}}'	
	CheckParam 'MaxLogSize'             ([ref]$MaxLogSize)            $false '{{if .MaxLogSize}}{{.MaxLogSize}}{{else}}{{end}}'			
	CheckParam 'LogicalStorageUnits'    ([ref]$LogicalStorageUnits)   $false '{{if .LogicalStorageUnits}}{{.LogicalStorageUnits}}{{else}}{{end}}'			
	CheckParam 'BackupBaseDir'          ([ref]$BackupBaseDir)         $false '{{if .BackupBaseDir}}{{.BackupBaseDir}}{{else}}{{end}}'			
	CheckParam 'TimeOut'                ([ref]$TimeOut)               $false '{{if .TimeOut}}{{.TimeOut}}{{else}}{{end}}'			
	CheckParam 'ServiceName'            ([ref]$ServiceName)           $false '{{if .ServiceName}}{{.ServiceName}}{{else}}{{end}}'			
	CheckParam 'mssqlDownloadURL'       ([ref]$mssqlDownloadURL)      $false '{{if .mssqlDownloadURL}}{{.mssqlDownloadURL}}{{else}}{{end}}'	
	CheckParam 'installDir'             ([ref]$installDir)            $false '{{if .installDir}}{{.installDir}}{{else}}{{end}}'						
}

function InstallMSSQLService(){
    Write-Host "Downloading Windows MSSQL Service"
    Invoke-WebRequest $mssqlDownloadURL -OutFile "MSSQLInstaller.msi"
    $mssqlInstallFile = Join-Path -Path $env:temp -ChildPath "$tempDir\MSSQLInstaller.msi"
    $mssqlArgs =  "/c", "msiexec", "/i", "`"$mssqlInstallFile`"", "/qn",  "INSTALLDIR=`"$installDir`""
    $mssqlArgs += "MessageBus=$messageBus", "LocalRoute=$localRoute", "Index=$index", "StatusPort=$statusPort"
    $mssqlArgs += "Plan=$plan", "Capacity=$capacity", "BaseDir=$BaseDir"
    $mssqlArgs += "NodeId=$NodeId", "ZInterval=$ZInterval", "LocalDb=$LocalDb"
    $mssqlArgs += "MaxNatsPayLoad=$MaxNatsPayLoad", "FqdnHosts=$FqdnHosts", "OPTimeLimit=$OPTimeLimit"
    $mssqlArgs += "Host=$HostName", "User=$User","Password=$Password","Port=$Port","MaxDbSize=$MaxDbSize"
	$mssqlArgs += "MaxLongQuery=$MaxLongQuery","MaxLongTx=$MaxLongTx","MaxUserConns=$MaxUserConns","InitialDataSize=$InitialDataSize","DataFileGrowth=$DataFileGrowth"
	$mssqlArgs += "LogFileGrowth=$LogFileGrowth","MaxDataSize=$MaxDataSize","InitialLogSize=$InitialLogSize","MaxLogSize=$MaxLogSize"
	$mssqlArgs += "LogicalStorageUnits=$LogicalStorageUnits","DefaultVersion=$DefaultVersion","SupportedName=$SupportedName"
	$mssqlArgs += "BackupBaseDir=$BackupBaseDir","TimeOut=$TimeOut","ServiceName=$ServiceName"
    Write-Host "Installing Windows MSSQL Node Service"
    [System.Diagnostics.Process]::Start("cmd", [System.String]::Join(" ", $mssqlArgs)).WaitForExit()
    Write-Host "Done!" -ForegroundColor Green
}

function Install{
    Write-Host "Using message bus" $messageBus
    Write-Host "Using localRoute" $localRoute

    CheckMSSQLService

    Set-Location $env:temp | Out-Null 
    New-Item -Type Directory -Name $tempDir | Out-Null
    Set-Location $tempDir | Out-Null
    
    VerifyParameters   
	InstallMSSQLService
}

function Cleanup{
    Write-Host "Cleaning up"
    #clean temporary folder used
    Remove-Item *.* -Force
    Set-Location ..
    Remove-Item $tempDir
    Set-Location $location
}

Install
Cleanup

