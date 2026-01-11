param(
  [Parameter(Mandatory=$true)][string]$ApiBaseUrl,
  [ValidateSet("log","email")][string]$Mode = "log",
  [string]$AdminUser = "admin",
  [string]$AdminPass = "Password123!",
  [string]$SmtpHost = "",
  [int]$SmtpPort = 587,
  [string]$SmtpUser = "",
  [string]$SmtpPass = "",
  [string]$MailTo = ""
)

Write-Host "Logging in as admin..."
$loginBody = @{ username = $AdminUser; password = $AdminPass } | ConvertTo-Json
$login = Invoke-RestMethod -Method Post -Uri "$ApiBaseUrl/api/auth/login" -ContentType "application/json" -Body $loginBody
$token = $login.token

$headers = @{ Authorization = "Bearer $token" }

Write-Host "Evaluating alerts..."
$result = Invoke-RestMethod -Method Post -Uri "$ApiBaseUrl/api/alerts/evaluate" -Headers $headers
Write-Host ("Created alerts: " + $result.Created)

if ($Mode -eq "log") {
  Write-Host "Mode=log (no email). Done."
  exit 0
}

if ([string]::IsNullOrWhiteSpace($SmtpHost) -or [string]::IsNullOrWhiteSpace($MailTo)) {
  Write-Host "Email mode requires -SmtpHost and -MailTo."
  exit 1
}

# Email sending (basic)
Add-Type -AssemblyName System.Net.Mail
$mail = New-Object System.Net.Mail.MailMessage
$mail.From = $SmtpUser
$mail.To.Add($MailTo)
$mail.Subject = "Construction Portal Alerts - Automation Run"
$mail.Body = "Alert evaluation executed. Created alerts: $($result.Created)."

$smtp = New-Object System.Net.Mail.SmtpClient($SmtpHost, $SmtpPort)
$smtp.EnableSsl = $true
$smtp.Credentials = New-Object System.Net.NetworkCredential($SmtpUser, $SmtpPass)
$smtp.Send($mail)

Write-Host "Email sent to $MailTo"
