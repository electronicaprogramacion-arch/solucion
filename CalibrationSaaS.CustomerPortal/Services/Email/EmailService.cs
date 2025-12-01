using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace CalibrationSaaS.CustomerPortal.Services.Email;

/// <summary>
/// Email service implementation using MailKit
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;
    private readonly IWebHostEnvironment _environment;

    public EmailService(
        IOptions<EmailOptions> emailOptions,
        ILogger<EmailService> logger,
        IWebHostEnvironment environment)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
        _environment = environment;
    }

    public async Task<bool> SendTwoFactorCodeAsync(string email, string code, string customerName, int expirationMinutes)
    {
        try
        {
            var subject = "Your CalibrationSaaS Customer Portal Verification Code";
            var htmlBody = GenerateTwoFactorEmailHtml(new TwoFactorEmailData
            {
                CustomerName = customerName,
                Code = code,
                ExpirationMinutes = expirationMinutes
            });

            var textBody = GenerateTwoFactorEmailText(customerName, code, expirationMinutes);

            var result = await SendEmailAsync(email, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("2FA code sent successfully to {Email}", email);
            }
            else
            {
                _logger.LogWarning("Failed to send 2FA code to {Email}", email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending 2FA code to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendWelcomeEmailAsync(string email, string customerName, string contactName)
    {
        try
        {
            var subject = "Welcome to CalibrationSaaS Customer Portal";
            var htmlBody = GenerateWelcomeEmailHtml(customerName, contactName);
            var textBody = GenerateWelcomeEmailText(customerName, contactName);

            var result = await SendEmailAsync(email, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("Welcome email sent successfully to {Email}", email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending welcome email to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendAccountLockoutNotificationAsync(string email, string customerName, int lockoutDurationMinutes)
    {
        try
        {
            var subject = "Account Security Alert - CalibrationSaaS Customer Portal";
            var htmlBody = GenerateAccountLockoutEmailHtml(customerName, lockoutDurationMinutes);
            var textBody = GenerateAccountLockoutEmailText(customerName, lockoutDurationMinutes);

            var result = await SendEmailAsync(email, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("Account lockout notification sent successfully to {Email}", email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending account lockout notification to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendSecurityAlertAsync(string email, string customerName, string activityDescription, string ipAddress, DateTime timestamp)
    {
        try
        {
            var subject = "Security Alert - CalibrationSaaS Customer Portal";
            var htmlBody = GenerateSecurityAlertEmailHtml(customerName, activityDescription, ipAddress, timestamp);
            var textBody = GenerateSecurityAlertEmailText(customerName, activityDescription, ipAddress, timestamp);

            var result = await SendEmailAsync(email, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("Security alert sent successfully to {Email}", email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending security alert to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendEquipmentDueReminderAsync(string email, string customerName, List<EquipmentDueReminder> equipmentList)
    {
        try
        {
            var subject = "Equipment Calibration Due Date Reminder";
            var htmlBody = GenerateEquipmentDueReminderEmailHtml(customerName, equipmentList);
            var textBody = GenerateEquipmentDueReminderEmailText(customerName, equipmentList);

            var result = await SendEmailAsync(email, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("Equipment due reminder sent successfully to {Email}", email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending equipment due reminder to {Email}", email);
            return false;
        }
    }

    public async Task<bool> SendTestEmailAsync(string testEmail)
    {
        try
        {
            var subject = "Test Email - CalibrationSaaS Customer Portal";
            var htmlBody = GenerateTestEmailHtml();
            var textBody = GenerateTestEmailText();

            var result = await SendEmailAsync(testEmail, subject, htmlBody, textBody);
            
            if (result)
            {
                _logger.LogInformation("Test email sent successfully to {Email}", testEmail);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test email to {Email}", testEmail);
            return false;
        }
    }

    public async Task<Dictionary<string, int>> GetEmailStatsAsync(DateTime fromDate)
    {
        // TODO: Implement email statistics tracking
        // This would typically involve storing email send events in a database
        await Task.Delay(1); // Placeholder

        return new Dictionary<string, int>
        {
            ["TotalSent"] = 0,
            ["TwoFactorCodes"] = 0,
            ["WelcomeEmails"] = 0,
            ["SecurityAlerts"] = 0,
            ["EquipmentReminders"] = 0,
            ["Failed"] = 0
        };
    }

    private async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string textBody)
    {
        if (_environment.IsDevelopment() && string.IsNullOrEmpty(_emailOptions.SmtpUsername))
        {
            // In development mode without SMTP configuration, just log the email
            _logger.LogInformation("Development Mode - Email would be sent to {Email} with subject: {Subject}", toEmail, subject);
            _logger.LogDebug("Email content: {Content}", textBody);
            return true;
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = textBody
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // Configure timeout
            client.Timeout = _emailOptions.TimeoutSeconds * 1000;

            // Connect to SMTP server
            await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, 
                _emailOptions.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

            // Authenticate if credentials are provided
            if (!string.IsNullOrEmpty(_emailOptions.SmtpUsername))
            {
                await client.AuthenticateAsync(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword);
            }

            // Send the message
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            if (_emailOptions.EnableLogging)
            {
                _logger.LogInformation("Email sent successfully to {Email} with subject: {Subject}", toEmail, subject);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email} with subject: {Subject}", toEmail, subject);
            return false;
        }
    }

    private string GenerateTwoFactorEmailHtml(TwoFactorEmailData data)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Verification Code</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .code {{ background: #fff; border: 2px solid #667eea; border-radius: 8px; padding: 20px; text-align: center; margin: 20px 0; }}
        .code-number {{ font-size: 32px; font-weight: bold; color: #667eea; letter-spacing: 8px; }}
        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 14px; }}
        .warning {{ background: #fff3cd; border: 1px solid #ffeaa7; border-radius: 5px; padding: 15px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>CalibrationSaaS Customer Portal</h1>
        <p>Verification Code</p>
    </div>
    <div class='content'>
        <h2>Hello {data.CustomerName},</h2>
        <p>You have requested access to the CalibrationSaaS Customer Portal. Please use the verification code below to complete your login:</p>
        
        <div class='code'>
            <div class='code-number'>{data.Code}</div>
            <p><strong>This code will expire in {data.ExpirationMinutes} minutes</strong></p>
        </div>
        
        <div class='warning'>
            <strong>Security Notice:</strong> If you did not request this code, please ignore this email. Do not share this code with anyone.
        </div>
        
        <p>If you have any questions or need assistance, please contact our support team at {data.SupportEmail}.</p>
        
        <p>Best regards,<br>The {data.CompanyName} Team</p>
    </div>
    <div class='footer'>
        <p>This email was sent on {data.Timestamp:MMM dd, yyyy} at {data.Timestamp:HH:mm} UTC</p>
        <p>&copy; 2024 {data.CompanyName}. All rights reserved.</p>
    </div>
</body>
</html>";
    }

    private string GenerateTwoFactorEmailText(string customerName, string code, int expirationMinutes)
    {
        return $@"
CalibrationSaaS Customer Portal - Verification Code

Hello {customerName},

You have requested access to the CalibrationSaaS Customer Portal. Please use the verification code below to complete your login:

VERIFICATION CODE: {code}

This code will expire in {expirationMinutes} minutes.

SECURITY NOTICE: If you did not request this code, please ignore this email. Do not share this code with anyone.

If you have any questions or need assistance, please contact our support team at support@calibrationsaas.com.

Best regards,
The CalibrationSaaS Team

This email was sent on {DateTime.UtcNow:MMM dd, yyyy} at {DateTime.UtcNow:HH:mm} UTC
© 2024 CalibrationSaaS. All rights reserved.
";
    }

    private string GenerateWelcomeEmailHtml(string customerName, string contactName)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Welcome to CalibrationSaaS Customer Portal</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .features {{ background: #fff; border-radius: 8px; padding: 20px; margin: 20px 0; }}
        .feature {{ margin: 15px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 14px; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Welcome to CalibrationSaaS Customer Portal</h1>
    </div>
    <div class='content'>
        <h2>Hello {contactName},</h2>
        <p>Welcome to the CalibrationSaaS Customer Portal for {customerName}! We're excited to provide you with easy access to your calibration certificates and equipment information.</p>
        
        <div class='features'>
            <h3>What you can do with the Customer Portal:</h3>
            <div class='feature'>📋 <strong>View and download calibration certificates</strong></div>
            <div class='feature'>📅 <strong>Track equipment due dates</strong></div>
            <div class='feature'>📊 <strong>Access calibration history</strong></div>
            <div class='feature'>🔍 <strong>Search your equipment</strong></div>
            <div class='feature'>📧 <strong>Receive automated reminders</strong></div>
        </div>
        
        <p>To access the portal, simply visit our website and log in using your email address. You'll receive a verification code via email for secure access.</p>
        
        <p>If you have any questions or need assistance, please don't hesitate to contact our support team.</p>
        
        <p>Best regards,<br>The CalibrationSaaS Team</p>
    </div>
    <div class='footer'>
        <p>&copy; 2024 CalibrationSaaS. All rights reserved.</p>
    </div>
</body>
</html>";
    }

    private string GenerateWelcomeEmailText(string customerName, string contactName)
    {
        return $@"
Welcome to CalibrationSaaS Customer Portal

Hello {contactName},

Welcome to the CalibrationSaaS Customer Portal for {customerName}! We're excited to provide you with easy access to your calibration certificates and equipment information.

What you can do with the Customer Portal:
- View and download calibration certificates
- Track equipment due dates
- Access calibration history
- Search your equipment
- Receive automated reminders

To access the portal, simply visit our website and log in using your email address. You'll receive a verification code via email for secure access.

If you have any questions or need assistance, please don't hesitate to contact our support team.

Best regards,
The CalibrationSaaS Team

© 2024 CalibrationSaaS. All rights reserved.
";
    }

    private string GenerateAccountLockoutEmailHtml(string customerName, int lockoutDurationMinutes)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Account Security Alert</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #dc3545; color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .alert {{ background: #f8d7da; border: 1px solid #f5c6cb; border-radius: 5px; padding: 15px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Account Security Alert</h1>
    </div>
    <div class='content'>
        <h2>Hello {customerName},</h2>
        <div class='alert'>
            <strong>Your account has been temporarily locked due to multiple failed login attempts.</strong>
        </div>
        <p>For your security, your CalibrationSaaS Customer Portal account has been locked for {lockoutDurationMinutes} minutes.</p>
        <p>If this was not you, please contact our support team immediately.</p>
        <p>Best regards,<br>The CalibrationSaaS Team</p>
    </div>
</body>
</html>";
    }

    private string GenerateAccountLockoutEmailText(string customerName, int lockoutDurationMinutes)
    {
        return $@"
Account Security Alert

Hello {customerName},

Your account has been temporarily locked due to multiple failed login attempts.

For your security, your CalibrationSaaS Customer Portal account has been locked for {lockoutDurationMinutes} minutes.

If this was not you, please contact our support team immediately.

Best regards,
The CalibrationSaaS Team
";
    }

    private string GenerateSecurityAlertEmailHtml(string customerName, string activityDescription, string ipAddress, DateTime timestamp)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Security Alert</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #ffc107; color: #212529; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .details {{ background: #fff; border-radius: 8px; padding: 20px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Security Alert</h1>
    </div>
    <div class='content'>
        <h2>Hello {customerName},</h2>
        <p>We detected suspicious activity on your CalibrationSaaS Customer Portal account:</p>
        <div class='details'>
            <p><strong>Activity:</strong> {activityDescription}</p>
            <p><strong>IP Address:</strong> {ipAddress}</p>
            <p><strong>Time:</strong> {timestamp:MMM dd, yyyy HH:mm} UTC</p>
        </div>
        <p>If this was you, you can ignore this email. If not, please contact our support team immediately.</p>
        <p>Best regards,<br>The CalibrationSaaS Team</p>
    </div>
</body>
</html>";
    }

    private string GenerateSecurityAlertEmailText(string customerName, string activityDescription, string ipAddress, DateTime timestamp)
    {
        return $@"
Security Alert

Hello {customerName},

We detected suspicious activity on your CalibrationSaaS Customer Portal account:

Activity: {activityDescription}
IP Address: {ipAddress}
Time: {timestamp:MMM dd, yyyy HH:mm} UTC

If this was you, you can ignore this email. If not, please contact our support team immediately.

Best regards,
The CalibrationSaaS Team
";
    }

    private string GenerateEquipmentDueReminderEmailHtml(string customerName, List<EquipmentDueReminder> equipmentList)
    {
        var equipmentRows = new StringBuilder();
        foreach (var equipment in equipmentList)
        {
            var statusClass = equipment.Status.ToLower() switch
            {
                "overdue" => "color: #dc3545;",
                "due soon" => "color: #ffc107;",
                _ => "color: #28a745;"
            };

            equipmentRows.AppendLine($@"
                <tr>
                    <td>{equipment.SerialNumber}</td>
                    <td>{equipment.Model}</td>
                    <td>{equipment.EquipmentType}</td>
                    <td>{equipment.DueDate:MMM dd, yyyy}</td>
                    <td style='{statusClass}'><strong>{equipment.Status}</strong></td>
                </tr>");
        }

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Equipment Due Date Reminder</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        table {{ width: 100%; border-collapse: collapse; background: #fff; border-radius: 8px; overflow: hidden; }}
        th, td {{ padding: 12px; text-align: left; border-bottom: 1px solid #ddd; }}
        th {{ background: #f8f9fa; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Equipment Due Date Reminder</h1>
    </div>
    <div class='content'>
        <h2>Hello {customerName},</h2>
        <p>The following equipment requires calibration attention:</p>
        <table>
            <thead>
                <tr>
                    <th>Serial Number</th>
                    <th>Model</th>
                    <th>Type</th>
                    <th>Due Date</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                {equipmentRows}
            </tbody>
        </table>
        <p>Please contact us to schedule calibration services for any overdue or due soon equipment.</p>
        <p>Best regards,<br>The CalibrationSaaS Team</p>
    </div>
</body>
</html>";
    }

    private string GenerateEquipmentDueReminderEmailText(string customerName, List<EquipmentDueReminder> equipmentList)
    {
        var equipmentText = new StringBuilder();
        foreach (var equipment in equipmentList)
        {
            equipmentText.AppendLine($"- {equipment.SerialNumber} ({equipment.Model}) - Due: {equipment.DueDate:MMM dd, yyyy} - Status: {equipment.Status}");
        }

        return $@"
Equipment Due Date Reminder

Hello {customerName},

The following equipment requires calibration attention:

{equipmentText}

Please contact us to schedule calibration services for any overdue or due soon equipment.

Best regards,
The CalibrationSaaS Team
";
    }

    private string GenerateTestEmailHtml()
    {
        return @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Test Email</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }
        .content { background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }
    </style>
</head>
<body>
    <div class='header'>
        <h1>Test Email</h1>
    </div>
    <div class='content'>
        <h2>Email Configuration Test</h2>
        <p>This is a test email to verify that the CalibrationSaaS Customer Portal email configuration is working correctly.</p>
        <p>If you received this email, the email service is functioning properly.</p>
        <p>Best regards,<br>The CalibrationSaaS Team</p>
    </div>
</body>
</html>";
    }

    private string GenerateTestEmailText()
    {
        return @"
Test Email - CalibrationSaaS Customer Portal

Email Configuration Test

This is a test email to verify that the CalibrationSaaS Customer Portal email configuration is working correctly.

If you received this email, the email service is functioning properly.

Best regards,
The CalibrationSaaS Team
";
    }
}
