
# Only need to change this variable
$gameIndex = 2
# ^^^^^^^^^^^^

$game = @{
    gameId       = "GOODS$gameIndex";
    scenes       = [System.Collections.ArrayList]@();
    activeLevels = [System.Collections.ArrayList]@();
}

$ErrorActionPreference = "Stop"
$PSDefaultParameterValues['*:Encoding'] = 'utf8'

$assetsFolder = Resolve-Path $PSScriptRoot\Assets\
$guidesFolder = Resolve-Path $assetsFolder\Resources\Game$gameIndex\Guides\
$settingsFolder = Resolve-Path $assetsFolder\Resources\Game$gameIndex\LevelSettings\
$gameFileFolder = Resolve-Path $assetsFolder\Resources\Game$gameIndex\
$gameFile = Resolve-Path $gameFileFolder\game$gameIndex.json
$newGameFile = Join-Path $gameFileFolder new_game$gameIndex.json

function Get-LevelScene {
    param (
        [Parameter(Mandatory)]
        $levelIndex
    )
    if ($levelIndex -ge 0) {
        if ($gameIndex -eq 1) {

            if ($levelIndex -le 6) {
                return "Game1-Scene1"
            }
            if ($levelIndex -le 10) {
                return "Game1-Scene2"
            }
            if ($levelIndex -le 18) {
                return "Game1-Scene3"
            }
        }
        elseif ($gameIndex -eq 2) {
            if ($levelIndex -le 3) {
                return "Game2-Scene1"
            }
            if ($levelIndex -le 11) {
                return "Game2-Scene2"
            }
            if ($levelIndex -le 16) {
                return "Game2-Scene3"
            }
        }
        else {
            throw "Game index out of range: $gameIndex"
        }
    }

    throw "Level index out of range: $levelIndex"
}

function Get-LevelSettings {
    param(
        [Parameter(Mandatory)]
        $settingsFile
    )
    $content = Get-Content $settingsFile
    $settings = @{
        availableFunctions = [System.Collections.ArrayList]@()
    }
    foreach ($item in $content) {
        # Write-Host "#__# $item"
        $split = ([string]$item).Split(":", 2).Replace("\t", "`t").Replace("\n", "`n")
        switch ($split[0].ToLowerInvariant()) {
            "taskdescription" {
                $settings.taskDescription = @{
                    header = $split[1].Trim()
                }
            }
            "precode" { $settings.precode = $split[1].Trim() }
            "startcode" { $settings.startCode = $split[1].Trim() }
            "rowlimit" { $settings.rowLimit = [int]::Parse($split[1].Trim()) }
            "smartbuttons" {
                if ($split[1].Contains("svara")) {
                    $settings.availableFunctions.Add("Answer") | Out-Null
                }
            }
            "functions" {
                $functions = $split[1].Trim().Split(",")
                foreach ($func in $functions) {
                    $func = $func.Trim()
                    if (!$settings.availableFunctions.Contains($func)) {
                        $settings.availableFunctions.Add($func) | Out-Null
                    }
                }
            }
        }
    }
    return $settings
}

function Get-LevelGuides {
    param(
        [Parameter(Mandatory)]
        $guidesFile
    )
    $content = Get-Content $guidesFile
    $guides = @()

    foreach ($item in $content) {
        $item = $item.Trim().Replace("\t", "`t").Replace("\n", "`n")
        if ($item -eq "") {
            continue
        }
        $split = ([string]$item).Split(":", 2)
        $guides += @{
            target = $split[0];
            text   = $split[1].Trim()
        }
    }

    return $guides
}

Write-Host "Assets folder: $assetsFolder"
Write-Host "Guides folder: $guidesFolder"
Write-Host "Settings folder: $settingsFolder"

Write-Host ">>> READING GAME FILE $gameFile" -BackgroundColor GREEN
$gameOld = Get-Content $gameFile | ConvertFrom-Json

for ($levelIndex = 0; $levelIndex -lt $gameOld.levels.Count; $levelIndex++) {
    $level = $gameOld.levels[$levelIndex]
    $id = $level.id
    Write-Host ":: LEVEL #$levelIndex `"$id`" ::" -BackgroundColor WHITE
    
    $sceneName = Get-LevelScene $levelIndex
    Write-Host "Scene: `"$sceneName`""
    
    # level settings
    if (Test-Path $settingsFolder\settings-$id.txt) {
        $settingsFile = Resolve-Path $settingsFolder\settings-$id.txt
        Write-Host "Settings file: $settingsFile"
        $settings = Get-LevelSettings $settingsFile

        Write-Verbose $(ConvertTo-Json $settings)
    }
    else {
        Write-Host "Settings file: <none>" -ForegroundColor RED
        Write-Host "<<< NO SETTINGS FILE, ABORTING! >>>" -ForegroundColor RED
        exit
    }

    # guide bubbles
    if (Test-Path $guidesFolder\guide-$id.txt) {
        $guidesFile = Resolve-Path $guidesFolder\guide-$id.txt
        Write-Host "Guide file: $guidesFile"
        $guides = @(Get-LevelGuides $guidesFile)
        Write-Host "Guide bubbles: $($guides.Count)"
        Write-Verbose $(ConvertTo-Json $guides)
    }
    else {
        Write-Host "Guide file: <none>" -ForegroundColor DARKGRAY
        $guides = @()
    }

    # cases
    $casesOld = $level.cases
    $cases = [System.Collections.ArrayList]@()
    foreach ($case in $casesOld) {
        $obj = @{
            caseDefinition = @{
                cars = $case.cars;
            }
        }
        if ($case.answer) {
            $obj.caseDefinition.answer = $case.answer
        }
        if ($case.chargeBound) {
            $obj.caseDefinition.chargeBound = $case.chargeBound
        }
        if ($case.correctSorting) {
            $obj.caseDefinition.correctSorting = $case.correctSorting
        }

        $precode = $case.precode

        # Custom precode from SceneController1_1
        if ($sceneName -eq "Game1-Scene1") {
            $precode = ""
            foreach ($section in $case.cars[0].sections) {
                if ($section.itemCount -gt 0) {
                    if ($precode.Length -gt 0) {
                        $precode += "`n"
                    }
                    $precode += "$($section.type) = $($section.itemCount)"
                }
            }
        }
        # Custom precode from SceneController1_3
        elseif ($sceneName -eq "Game1-Scene3") {
            $precode = "antal_tåg = $($case.cars.Count)"
        }

        if ($precode) {
            $obj.caseSettings = @{
                precode = $precode
            }
        }

        $cases.Add($obj) | Out-Null
    }
    Write-Host "Cases: $($cases.Count)"
    Write-Verbose $(ConvertTo-Json $cases -Depth 3)

    # Get or create scene obj
    $scene = $null
    foreach ($s in $game.scenes) {
        if ($s.name -eq $sceneName) {
            $scene = $s
            Write-Host "Adding level to scene `"$sceneName`""
            break
        }
    }
    if ($null -eq $scene) {
        Write-Host "Creating new scene `"$sceneName`" and adding level to scene"
        $scene = @{
            name          = $sceneName;
            sceneSettings = @{
                availableFunctions = [System.Collections.ArrayList]@()
            };
            levels        = [System.Collections.ArrayList]@()
        }
        $game.scenes.Add($scene) | Out-Null
    }

    $newId = "$($game.gameId)_$($level.id)"

    # Add to scene
    $scene.levels.Add(@{
            id            = $newId;
            guideBubbles  = $guides;
            levelSettings = $settings;
            cases         = $cases;
        }) | Out-Null

    # Add to active levels
    $game.activeLevels.Add(@{
            sceneName = $sceneName;
            levelId   = $newId;
        }) | Out-Null

    Write-Host ""
}

# Move common functions to scene settings
foreach ($scene in $game.scenes) {
    Write-Host ":: SCENE `"$($scene.name)`" ::" -BackgroundColor BLUE
    if ($scene.levels.Count -eq 0) {
        Write-Host "No levels in this scene..." -ForegroundColor DARKYELLOW
        Write-Host ""
        continue
    }

    Write-Host "Levels: $($scene.levels.Count)"

    # Select all common functions
    $commonFunctions = [System.Collections.ArrayList]@()
    $firstFunctions = $scene.levels[0].levelSettings.availableFunctions

    foreach ($func in $firstFunctions) {
        $everyoneHasIt = $true
        for ($i = 1; $i -lt $scene.levels.Count; $i++) {
            if (-not $scene.levels[$i].levelSettings.availableFunctions.Contains($func)) {
                $everyoneHasIt = $false
                break
            }
        }
        if ($everyoneHasIt) {
            $commonFunctions.Add($func) | Out-Null
        }
    }

    if ($commonFunctions.Count -gt 0) {
        $joined = $commonFunctions -join "`", `""
        Write-Host "Common functions: `"$joined`""

        # Remove common functions from levelSettings
        foreach ($level in $scene.levels) {
            foreach ($func in $commonFunctions) {
                if ($level.levelSettings.availableFunctions.Contains($func)) {
                    $level.levelSettings.availableFunctions.Remove($func) | Out-Null
                    $removedFrom.Add($level.id) | Out-Null
                }
            }
        }

        # Add functions to scene
        $scene.sceneSettings.availableFunctions.AddRange($commonFunctions)
    }
    else {
        Write-Host "Common functions: <none>" -ForegroundColor DARKGRAY
    }

    foreach ($level in $scene.levels) {
        $uncommonFunctions = $level.levelSettings.availableFunctions
        if ($uncommonFunctions.Count -gt 0) {
            $joined = $uncommonFunctions -join "`", `""
            Write-Host "Special to `"$($level.id)`": `"$joined`"" -ForegroundColor DARKGRAY
        }
    }

    Write-Host ""
}

Write-Host ":: OUTPUT `"$newGameFile`" ::" -BackgroundColor GREEN

$json = (ConvertTo-Json $game -Depth 100).Replace("\u003c", "<").Replace("\u003e", ">")
$json | Set-Content -Path $newGameFile
