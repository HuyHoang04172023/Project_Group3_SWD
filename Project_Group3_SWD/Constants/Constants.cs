namespace Project_Group3_SWD.Constants
{
    public class EmailConstants
    {
        public const string GMAIL_REGISTER_EMAIL_SUBJECT = "Welcome to SHOPQUANAO!";

        public const string GMAIL_REGISTER_EMAIL_BODY = @"
                <html>
                <body>
                    <h2>Welcome to SHOPQUANAO!</h2>
                    <p>Your account has been successfully created via Google Sign-In. Here are your account details:</p>
                    <p><strong>Registered Email:</strong> {1}</p>
                    <p>If this wasn’t you, please contact our support team immediately.</p>
                    <br>
                    <p>Best regards,</p>
                    <p>SHOPQUANAO Team</p>
                    <p><a href='mailto:support@SHOPQUANAO.com'>support@SHOPQUANAO.com</a> | SHOPQUANAO</p>
                </body>
                </html>";
    }
}