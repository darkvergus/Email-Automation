using System.Net.Mail;

namespace Email_IAM;

internal static class Program
{
	private const string SmtpServer = "smtp-mail.outlook.com";
	private const int SmtpPort = 25;
	private const bool SmtpSsl = true;
	
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();
		Application.Run(new MainForm());
		
		SendEmail(SmtpServer, SmtpPort, SmtpSsl);  
	}
	
	static void SendEmail(string smtpServer, int smtpPort, bool smtpSsl)  
	{
		EmailManager mailMan = new(smtpServer, smtpPort, smtpSsl);  
  
		EmailSendConfigure config = new()
		{
			// replace with your email userName  
			ClientCredentialUserName = "",
			// replace with your email account password
			ClientCredentialPassword = "",
			// get the list or single from a textbox?
			TOs = new[] { "emarvao@euronext.com.com" },
			CCs = Array.Empty<string>(),
			From = "accessreview@euronext.com",
			FromDisplayName = "Access Review",
			Priority = MailPriority.Normal,
			// get the subject from a textbox?
			Subject = ""
		};

		EmailContent content = new()
		{
			IsHtml = true,
			// get from an html page? or build it?
			Content = "",
			// get attachments from a dialog?
			AttachFileName = ""
		};

		mailMan.SendMail(config, content);  
	}  
}
