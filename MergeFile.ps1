# Скрипт для объединения C# файлов
$rootFolder = "D:\WPS\ElectronicDiary\ElectronicDiary"
$outputFile = "D:\WPS\ElectronicDiary\AllCode.cs"

# Очистить итоговый файл
if (Test-Path $outputFile) {
    Clear-Content $outputFile
}

# Получить все .cs файлы, исключая системные папки
$allCsFiles = Get-ChildItem -Path $rootFolder -Recurse -File -Include *.cs
$excludedFiles = $allCsFiles | Where-Object { 
    $_.FullName -match "(?i)\\bin\\?|\\obj\\?|\\Resources\\?" 
}
$csFiles = $allCsFiles | Where-Object { 
    $_.FullName -notmatch "(?i)\\bin\\?|\\obj\\?|\\Resources\\?" 
}

# Добавить заголовок
$header = @"
// Объединенный код C#
// Дата создания: $(Get-Date)
// Исключено файлов: $($excludedFiles.Count)

"@
$header | Out-File -FilePath $outputFile -Encoding utf8

# Обработать файлы
$csFiles | ForEach-Object {
    $divider = "//" + ("/" * 78)
    $fileHeader = @"
`n$divider
// FILE: $($_.FullName)
$divider`n
"@
    $fileHeader | Out-File -FilePath $outputFile -Append -Encoding utf8
    Get-Content $_.FullName -Raw | Out-File -FilePath $outputFile -Append -Encoding utf8
}

# Статистика
$totalSizeMB = ($csFiles | Measure-Object -Property Length -Sum).Sum / 1MB
$stats = @"
// ==================================================
// Статистика:
// Всего файлов: $($csFiles.Count)
// Общий размер: $("{0:N2} MB" -f $totalSizeMB)
// Исключено файлов: $($excludedFiles.Count)
// ==================================================
"@
$stats | Out-File -FilePath $outputFile -Append -Encoding utf8

Write-Host "Обработано $($csFiles.Count) .cs файлов (bin, obj, Resources исключены). Результат: $outputFile"