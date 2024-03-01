# ##############################################################################################################
# Author: Brian A Roberts ()
# Description: Takes Json returned from a web service and places it in a file in CSV format. 
# Last Mod Date: 9.1.2022
# ##############################################################################################################


function TestWS
{
    # ParamTesting Modify as needed 
    $url = "https://localhost:44351/App/Recordset?entry_date=9%2F26%2F2022"
    $headers = @{
        "Content-Type"="application/json";
        "X-Api-Key"="eb7ed02c-8c48-4a85-a916-9cbea23e99cb"
    }
    $filePath = "c:\temp\WSOutput.csv"

    #WebServiceToCSV -url $url -headers $headers, -filePath $filePath
    $webRequest = Invoke-WebRequest $url -Headers $headers 
    $json = ConvertFrom-Json $webRequest.Content
    $json
    
    Write-Host "$filePath successfully generated from $filePath"
}

function WebServiceToCSV {
    param([Parameter(Mandatory=$true)]$json)

    
    $data = ConvertFrom-Json $json.returnValue
    $csv = $data | ConvertTo-Csv 
    $csv[1..($csv.count -1)] | Out-File $filePath
}