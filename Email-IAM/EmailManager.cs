using System.Net;
using System.Net.Mail;  
using System.Net.Mime;
using static System.Text.Encoding;

namespace Email_IAM;

public class EmailManager
{
	private readonly string HostName;
    private readonly int Port;
    private readonly bool SSL;

	public EmailManager(string hostName, int port, bool ssl)
	{
		HostName = hostName;
        Port = port;
        SSL = ssl;
    }

    public void SendMail(EmailSendConfigure emailConfig, EmailContent content)  
        {  
            MailMessage msg = ConstructEmailMessage(emailConfig, content);  
            Send(msg, emailConfig);  
        }  
  
        // Put the properties of the email including "to", "cc", "from", "subject" and "email body"  
        private static MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)  
        {  
            MailMessage msg = new();  
            foreach (string to in emailConfig.TOs)  
            {  
                if (!string.IsNullOrEmpty(to))  
                {  
                    msg.To.Add(to);  
                }  
            }  
  
            foreach (string cc in emailConfig.CCs)  
            {  
                if (!string.IsNullOrEmpty(cc))  
                {  
                    msg.CC.Add(cc);  
                }  
            }  
  
            msg.From = new(emailConfig.From, emailConfig.FromDisplayName, UTF8);  
            msg.IsBodyHtml = content.IsHtml;  
            msg.Body = content.Content;  
            msg.Priority = emailConfig.Priority;  
            msg.Subject = emailConfig.Subject;  
            msg.BodyEncoding = UTF8;  
            msg.SubjectEncoding = UTF8;

            if (content.AttachFileName != null)
            {
                Attachment data = new(content.AttachFileName, MediaTypeNames.Application.Zip);  
                msg.Attachments.Add(data);
            }

            return msg;  
        }  
  
        //Send the email using the SMTP server  
        private void Send(MailMessage message, EmailSendConfigure emailConfig)  
        {  
            SmtpClient client = new();  
            client.UseDefaultCredentials = false;  
            client.Credentials = new NetworkCredential(emailConfig.ClientCredentialUserName, emailConfig.ClientCredentialPassword)
            {
                Domain = null,
                SecurePassword = null
            };  
            client.Host = HostName;  
            client.Port = Port;  // this is critical
            client.EnableSsl = SSL;  // this is critical
  
            try  
            {  
                client.Send(message);  
            }  
            catch (Exception e)  
            {  
                Console.WriteLine("Error in Send email: {0}", e.Message);  
                throw;  
            }  
            message.Dispose();  
        }
}  
