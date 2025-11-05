$sourcePath = "ProductTest2.cs"
$sourceContent = Get-Content $sourcePath -Raw

1..100 | ForEach-Object {
    $newFileName = "ProductTest2_$_.cs"
    $newContent = $sourceContent -replace "class ProductTest2", "class ProductTest2_$_" `
        -replace "public ProductTest2\(", "public ProductTest2_$_("

    $newContent | Out-File -FilePath $newFileName -Encoding UTF8
    Write-Host "Created file: $newFileName"
}